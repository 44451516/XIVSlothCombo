import type {
  ApiEnvelope,
  ConfigDto,
  HealthDto,
  MetaDto,
  PresetDetailDto,
  PresetOptionsDto,
  TimelineStateDto,
  ToggleResult,
  ValueType,
} from './types'

/** Error raised by the plugin's API. */
export class ApiError extends Error {
  constructor(
    readonly code: string,
    message: string,
  ) {
    super(message)
    this.name = 'ApiError'
  }
}

const TOKEN_STORAGE_KEY = 'sloth-webui-token'

let cachedToken: string | null = null

/**
 * Resolves the session token of the plugin. In production the token arrives in the
 * query string of the page the plugin opened; in development it is baked into the
 * bundle by the Vite configuration.
 */
export function resolveToken(): string {
  if (cachedToken) return cachedToken

  try {
    const fromQuery = new URLSearchParams(window.location.search).get('t')
    if (fromQuery) {
      cachedToken = fromQuery
      try {
        window.sessionStorage.setItem(TOKEN_STORAGE_KEY, fromQuery)
      } catch {
        // A browser that refuses to store the token is not a problem.
      }
      return cachedToken
    }

    const stored = window.sessionStorage.getItem(TOKEN_STORAGE_KEY)
    if (stored) {
      cachedToken = stored
      return cachedToken
    }
  } catch {
    // Fall through to the build time token.
  }

  cachedToken = typeof __SLOTH_TOKEN__ === 'string' ? __SLOTH_TOKEN__ : ''
  return cachedToken
}

let discovery: Promise<string> | null = null

/**
 * In development the token is not part of the URL, so the dev server is asked for it.
 */
async function ensureToken(): Promise<string> {
  const token = resolveToken()
  if (token) return token

  if (!discovery) {
    discovery = fetch('/__sloth_info')
      .then((response) => (response.ok ? response.json() : null))
      .then((info: { token?: string } | null) => info?.token ?? '')
      .catch(() => '')
  }

  return discovery
}

async function request<T>(method: string, path: string, body?: unknown): Promise<T> {
  const headers: Record<string, string> = {}
  const token = await ensureToken()
  if (token) headers['X-Sloth-Token'] = token

  let payload: string | undefined
  if (body !== undefined) {
    headers['Content-Type'] = 'application/json'
    payload = JSON.stringify(body)
  }

  let response: Response
  try {
    response = await fetch(path, {
      method,
      headers,
      body: payload,
      credentials: 'same-origin',
      cache: 'no-store',
    })
  } catch (error) {
    throw new ApiError('NETWORK_ERROR', `无法连接到插件的本地服务：${(error as Error).message}`)
  }

  const text = await response.text()
  let envelope: ApiEnvelope<T> | null = null
  if (text) {
    try {
      envelope = JSON.parse(text) as ApiEnvelope<T>
    } catch {
      envelope = null
    }
  }

  if (!response.ok || !envelope || envelope.ok !== true) {
    const code = envelope?.error?.code ?? `HTTP_${response.status}`
    const message = envelope?.error?.message ?? `请求失败（HTTP ${response.status}）`
    throw new ApiError(code, message)
  }

  return envelope.data as T
}

/** Builds the address of a game icon. The token travels in the query string. */
export function iconUrl(iconId: number, hd = false): string {
  const token = resolveToken()
  const query = `hd=${hd ? 1 : 0}${token ? `&t=${encodeURIComponent(token)}` : ''}`
  return `/api/icon/${iconId}?${query}`
}

export const api = {
  get: <T>(path: string) => request<T>('GET', path),

  post: <T>(path: string, body?: unknown) => request<T>('POST', path, body),

  patch: <T>(path: string, body?: unknown) => request<T>('PATCH', path, body),

  health: () => request<HealthDto>('GET', '/api/health'),

  meta: () => request<MetaDto>('GET', '/api/meta'),

  preset: (id: string) =>
    request<PresetDetailDto>('GET', `/api/presets/${encodeURIComponent(id)}`),

  presetOptions: (id: string) =>
    request<PresetOptionsDto>('GET', `/api/presets/${encodeURIComponent(id)}/options`),

  togglePreset: (id: string, enabled: boolean) =>
    request<ToggleResult>('POST', `/api/presets/${encodeURIComponent(id)}/toggle`, { enabled }),

  getValue: (key: string, type: ValueType) =>
    request<{ key: string; type: ValueType; value: unknown }>(
      'GET',
      `/api/value?key=${encodeURIComponent(key)}&type=${encodeURIComponent(type)}`,
    ),

  setValue: (key: string, type: ValueType, value: unknown) =>
    request<{ key: string; type: ValueType; value: unknown }>('POST', '/api/value', {
      key,
      type,
      value,
    }),

  config: () => request<ConfigDto>('GET', '/api/config'),

  patchConfig: (payload: Partial<ConfigDto>) =>
    request<ConfigDto>('PATCH', '/api/config', payload),

  timeline: () => request<TimelineStateDto>('GET', '/api/timeline'),

  timelineAction: (action: string, payload: Record<string, unknown> = {}) =>
    request<Record<string, unknown>>('POST', '/api/timeline/action', { action, ...payload }),
}
