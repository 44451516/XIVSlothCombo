import { api, ApiError } from './client'

export type RotationMode = 'legacy' | 'v8'
export type RotationValue = number | boolean

export interface RotationParameter {
  id: string
  name: string
  type: 'number' | 'boolean'
  default: RotationValue
  min?: number
  max?: number
}

export interface RotationPlan {
  [key: string]: unknown
  format: 'xiv-rotation'
  version: 1
  id: string
  name: string
  jobId: number
  entryPhaseId: string
  parameters: RotationParameter[]
  parameterValues: Record<string, RotationValue>
  variables: unknown[]
  phases: unknown[]
}

export interface RotationJob {
  mode: RotationMode
  activePlanId?: string | null
}

export interface RotationsDto {
  plans: RotationPlan[]
  jobs: Record<string, RotationJob | undefined>
  failClosed: boolean
  initializationError: string | null
}

export interface RotationState {
  running: boolean
  available?: boolean
  initialized?: boolean
  emergencyStopped?: boolean
  jobId?: number | null
  phaseId?: string | null
  pendingActionId?: number | null
  reason?: string | null
  emergencyHotkey?: string | null
}

export type RotationControlRequest =
  | { command: 'start'; expectedJobId: number; emergencyHotkey?: string }
  | { command: 'stop' | 'reset'; emergencyHotkey?: string }

const requestTimeoutMs = 8000

function withTimeout<T>(request: Promise<T>, label: string, onSettled?: () => void): Promise<T> {
  return new Promise<T>((resolve, reject) => {
    const timer = setTimeout(() => {
      reject(new ApiError('ROTATION_TIMEOUT', `${label}超过 8 秒未确认，请勿假定操作已取消`))
    }, requestTimeoutMs)
    // The shared client has no abort option; observe late settlement without accepting late data.
    request.finally(onSettled).then(
      (value) => {
        clearTimeout(timer)
        resolve(value)
      },
      (error: unknown) => {
        clearTimeout(timer)
        reject(error)
      },
    )
  })
}

let stateRead: Promise<RotationState> | undefined

function readState(force: boolean): Promise<RotationState> {
  if (!force && stateRead) return stateRead
  const request: Promise<RotationState> = withTimeout(api.get<RotationState>('/api/rotations/state'), '状态读取', () => {
    if (stateRead === request) stateRead = undefined
  })
  // Retain an expired read until its transport settles; polling must not queue unlimited fetches.
  stateRead = request
  return request
}

// All endpoints share the existing client's token discovery and ApiResult decoding.
export const rotationsApi = {
  list: () => withTimeout(api.get<RotationsDto>('/api/rotations'), '方案读取'),
  state: (force = false) => readState(force),
  catalog: () => api.get<unknown>('/api/rotations/catalog'),
  importPlan: (plan: RotationPlan, overwrite = false) =>
    api.post<unknown>('/api/rotations', { operation: 'import', plan, overwrite }),
  mode: (jobId: number, mode: RotationMode) =>
    api.post<unknown>('/api/rotations', { operation: 'mode', jobId, mode }),
  select: (jobId: number, planId: string) =>
    api.post<unknown>('/api/rotations', { operation: 'select', jobId, planId }),
  parameters: (jobId: number, values: Record<string, RotationValue>) =>
    api.post<unknown>('/api/rotations', { operation: 'parameters', jobId, values }),
  delete: (planId: string) =>
    api.post<unknown>('/api/rotations', { operation: 'delete', planId }),
  control: (body: RotationControlRequest, options?: { onSettled: () => void }) => {
    const request = api.post<RotationState & { ok?: boolean }>('/api/rotations/control', body)
      .then((result) => {
        if (result?.ok === false) {
          throw new ApiError('ROTATION_CONTROL_REJECTED', result.reason ?? '运行时拒绝了控制请求')
        }
        if (!result || typeof result.running !== 'boolean') {
          throw new ApiError('INVALID_ROTATION_CONTROL_REPLY', '控制接口返回了无效状态')
        }
        return result
      })
    return withTimeout(request, '控制请求', options?.onSettled)
  },
}
