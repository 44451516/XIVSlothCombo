<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import {
  NAlert,
  NButton,
  NEmpty,
  NForm,
  NFormItem,
  NInput,
  NInputNumber,
  NModal,
  NRadioButton,
  NRadioGroup,
  NSelect,
  NSpin,
  NSwitch,
  NTag,
  NTooltip,
  type InputInst,
} from 'naive-ui'
import { Check, Download, Keyboard, Play, RefreshCw, RotateCcw, Save, Square, Trash2, Undo2, Upload } from 'lucide-vue-next'
import { ApiError } from '@/api/client'
import {
  rotationsApi,
  type RotationControlRequest,
  type RotationMode,
  type RotationParameter,
  type RotationPlan,
  type RotationState,
  type RotationValue,
  type RotationsDto,
} from '@/api/rotations'
import JobIcon from '@/components/JobIcon.vue'
import { toastError, toastSuccess } from '@/composables/useToast'
import { useMetaStore } from '@/stores/meta'

const meta = useMetaStore()
const maximumDocumentBytes = 32 * 1024 * 1024
const supportedJobs = new Set([
  19, 20, 21, 22, 23, 24, 25, 27, 28, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42,
])
const snapshot = ref<RotationsDto>({ plans: [], jobs: {}, failClosed: false, initializationError: null })
const state = ref<RotationState>({ running: false })
const selectedJobId = ref<number | null>(null)
const selectedPlanId = ref<string | null>(null)
const jobSearch = ref('')
const busy = ref(false)
const starting = ref(false)
const stopping = ref(false)
const resetting = ref(false)
const savingHotkey = ref(false)
const exportingCatalog = ref(false)
const plansLoaded = ref(false)
const plansFresh = ref(false)
const stateLoaded = ref(false)
const stateReceivedAt = ref<number | null>(null)
const freshnessClock = ref(performance.now())
const pendingControls = ref(0)
const compensating = ref(false)
const pageError = ref('')
const stateError = ref('')
const lastUpdated = ref('')
const parameterValues = ref<Record<string, RotationValue | null>>({})
const parameterBaseline = ref<Record<string, RotationValue>>({})
const importPlan = ref<RotationPlan | null>(null)
const importMode = ref<'copy' | 'overwrite'>('copy')
const importError = ref('')
const fileInput = ref<HTMLInputElement | null>(null)
const hotkeyInput = ref<InputInst | null>(null)
const hotkey = ref('Ctrl+Alt+F12')
const savedHotkey = ref<string | null>(null)
const hotkeyEdited = ref(false)
const recordingHotkey = ref(false)

let disposed = false
let pollTimer: ReturnType<typeof setTimeout> | undefined
let freshnessTimer: ReturnType<typeof setInterval> | undefined
let stateRequest = 0
let plansRequest = 0
let pollFailures = 0
let stopEpoch = 0
const stateFreshnessMs = 6000

const jobs = computed(() => meta.jobs.filter((job) => supportedJobs.has(job.jobId)))
const filteredJobs = computed(() => {
  const keyword = jobSearch.value.trim().toLowerCase()
  return jobs.value.filter((job) => `${job.name} ${job.shorthand}`.toLowerCase().includes(keyword))
})
const currentJob = computed(() => jobs.value.find((job) => job.jobId === selectedJobId.value))
const currentSettings = computed(() =>
  selectedJobId.value === null ? undefined : snapshot.value.jobs[String(selectedJobId.value)],
)
const mode = computed<RotationMode>(() => currentSettings.value?.mode ?? 'legacy')
const jobPlans = computed(() =>
  snapshot.value.plans.filter((plan) => plan.jobId === selectedJobId.value),
)
const selectedPlan = computed(() => jobPlans.value.find((plan) => plan.id === selectedPlanId.value))
const activePlan = computed(() =>
  jobPlans.value.find((plan) => plan.id === currentSettings.value?.activePlanId),
)
const isActivePlan = computed(() =>
  Boolean(selectedPlan.value && selectedPlan.value.id === activePlan.value?.id),
)
const stateFresh = computed(() => stateLoaded.value && !stateError.value
  && stateReceivedAt.value !== null && freshnessClock.value - stateReceivedAt.value < stateFreshnessMs)
const stateProblem = computed(() => stateError.value
  || (stateLoaded.value && !stateFresh.value ? '运行状态超过 6 秒未更新，等待重新同步' : ''))
const controlsBusy = computed(() => starting.value || stopping.value || savingHotkey.value
  || resetting.value || compensating.value)
const locked = computed(() => busy.value || controlsBusy.value || !plansFresh.value
  || !stateFresh.value || pendingControls.value > 0)
const planOptions = computed(() =>
  jobPlans.value.map((plan) => ({
    value: plan.id,
    label: `${plan.name}${plan.id === activePlan.value?.id ? '（当前）' : ''}`,
  })),
)
const runtimeJobName = computed(() => {
  if (!stateFresh.value || !state.value.jobId) return '—'
  return meta.jobs.find((job) => job.jobId === state.value.jobId)?.name ?? String(state.value.jobId)
})
const runtimeLabel = computed(() => {
  if (stateProblem.value) return '状态未知'
  if (!stateLoaded.value) return '待连接'
  if (state.value.emergencyStopped) return '急停已锁定'
  if (state.value.available === false) return '引擎不可用'
  return state.value.running ? '运行中' : '已停止'
})
const storeError = computed(() => snapshot.value.failClosed
  ? snapshot.value.initializationError || '方案存储不可用'
  : '')
const reasonLabels: Record<string, string> = {
  stopped: '已停止',
  stop: '已手动停止',
  pause: '已暂停',
  reset: '急停锁存已解除，保持停止',
  starting: '正在启动',
  disposed: '运行时已卸载',
  'not-initialized': '运行引擎尚未初始化',
  'emergency-hotkey': '急停热键已触发',
  'emergency-latched:reset-required': '急停已锁定，需要先解除急停',
  emergency: '急停已触发',
  emergencyStop: '急停已触发',
  'manual-yield': '正在等待手动操作结束',
  'waiting-for-action-effect': '正在等待技能效果确认',
  'submitted-awaiting-confirmation': '技能已提交，等待确认',
  'native-cast-lock-or-manual-queue': '正在读条、动作锁定或手动技能队列中',
  'no-matching-rule': '当前没有符合条件的规则',
  'waiting-for-combat-or-countdown': '正在等待战斗或倒计时',
  'waiting-for-sequence-condition': '正在等待序列条件',
  'sequence-aborted-on-timeout': '序列超时，已中止',
  'sequence-step-skipped-on-timeout': '序列步骤超时，已跳过',
  'prepull-not-authorized-or-countdown-inactive': '未授权战前施放或倒计时未开始',
  'action-acknowledgement-timeout': '技能确认超时，已停止',
  'native-submission-rejected': '游戏未接受技能提交',
  'ambiguous-native-action-result': '技能提交结果不明确，已停止',
  'player-job-or-territory-changed': '职业或区域已改变，已停止',
  'active-v8-rotation-unavailable': '当前职业未启用有效的 8.0 方案',
  'active-rotation-unavailable': '当前方案不可用',
  'rotation-or-parameters-changed': '方案或参数已改变，已停止',
  'rotation-plan-imported': '方案已导入，运行已停止',
  'rotation-plan-deleted': '方案已删除，运行已停止',
  'rotation-plan-selected': '当前方案已改变，运行已停止',
  'rotation-mode-changed': '职业模式已改变，运行已停止',
  'rotation-parameters-changed': '方案参数已改变，运行已停止',
  'player-unavailable': '当前没有可用角色',
  'player-dead': '角色已死亡',
  'player-occupied': '角色正忙，已停止',
  'zone-or-logout': '正在切换区域或退出登录',
  'pvp-unavailable': 'PvP 中不可运行',
  'target-unavailable': '当前没有可用目标',
  'moving-cast': '移动中，无法咏唱',
  'action-not-unlocked': '技能尚未解锁',
  'action-job-mismatch': '技能与当前职业不匹配',
  'rotation-job-mismatch': '方案与当前职业不匹配',
  'native-action-api-unavailable': '游戏技能接口不可用',
  'native-engine-capabilities-unavailable': '运行引擎所需的游戏接口不可用',
  'action-manager-unavailable': '技能管理器不可用',
  'native-conditions-unavailable': '游戏状态接口不可用',
  'conditions-unavailable': '游戏条件不可用',
  'invalid-emergency-hotkey': '急停热键格式无效',
  'emergency-hotkey-needs-modifier-and-key': '急停热键需要修饰键和字母、数字或功能键',
  'action-observation-buffer-overflow': '技能观测缓冲区已满，已停止',
}
const reasonLabel = computed(() => {
  const reason = state.value.reason ?? ''
  return reasonLabels[reason] ?? (reason.startsWith('unavailable:')
    ? '运行所需的游戏能力不可用，已停止'
    : reason.startsWith('condition-unavailable:') ? '部分条件数据不可用'
      : reason.startsWith('invalid-control:') ? '控制请求无效，已停止' : '运行时返回了未识别的状态')
})
const parameterDirty = computed(() =>
  (selectedPlan.value?.parameters ?? []).some(
    (parameter) => parameterValues.value[parameter.id] !== parameterBaseline.value[parameter.id],
  ),
)
const parameterErrors = computed(() => {
  const errors: Record<string, string> = {}
  for (const parameter of selectedPlan.value?.parameters ?? []) {
    const value = parameterValues.value[parameter.id]
    if (parameter.type === 'boolean') {
      if (typeof value !== 'boolean') errors[parameter.id] = '请选择开关状态'
    } else if (typeof value !== 'number' || !Number.isFinite(value)) {
      errors[parameter.id] = '请输入有效数字'
    } else if (parameter.min !== undefined && value < parameter.min) {
      errors[parameter.id] = `不能小于 ${parameter.min}`
    } else if (parameter.max !== undefined && value > parameter.max) {
      errors[parameter.id] = `不能大于 ${parameter.max}`
    }
  }
  return errors
})
const hotkeyValid = computed(() => hotkey.value.trim().length > 0 && hotkey.value.trim().length <= 80)
const startBlocked = computed(() => {
  if (snapshot.value.failClosed) return '方案存储已锁定，须先修复文件'
  if (!plansFresh.value) return '方案数据尚未确认'
  if (pendingControls.value > 0) return '仍有控制请求未收到响应，暂不可启动'
  if (!stateFresh.value) return stateProblem.value || '运行状态尚未确认'
  if (state.value.available !== true) return '运行所需的游戏接口尚未确认可用'
  if (state.value.emergencyStopped) return '急停已锁定，须先解除急停'
  if (!state.value.jobId) return '当前角色职业尚未确认'
  if (state.value.jobId !== selectedJobId.value) return '页面职业与游戏当前职业不一致'
  if (mode.value !== 'v8') return '当前职业使用传统连招'
  if (!activePlan.value) return '当前职业尚未选择方案'
  if (!isActivePlan.value) return '查看中的方案不是当前方案'
  if (parameterDirty.value) return '参数有未保存的修改'
  if (!hotkeyValid.value) return '急停热键不能为空'
  return ''
})
const duplicatePlan = computed(() =>
  snapshot.value.plans.find((plan) => plan.id === importPlan.value?.id),
)

watch(
  jobs,
  (list) => {
    if (!list.some((job) => job.jobId === selectedJobId.value)) {
      selectedJobId.value = list[0]?.jobId ?? null
    }
  },
  { immediate: true },
)
watch([selectedJobId, jobPlans, () => currentSettings.value?.activePlanId], () => {
  if (!jobPlans.value.some((plan) => plan.id === selectedPlanId.value)) {
    selectedPlanId.value = activePlan.value?.id ?? jobPlans.value[0]?.id ?? null
  }
})
watch(selectedPlan, (plan, previous) => {
  if (plan?.id === previous?.id && parameterDirty.value) return
  resetParameters()
}, { immediate: true })

function savedValue(parameter: RotationParameter): RotationValue {
  return selectedPlan.value?.parameterValues?.[parameter.id] ?? parameter.default
}

function resetParameters(): void {
  const values = Object.fromEntries(
    (selectedPlan.value?.parameters ?? []).map((parameter) => [parameter.id, savedValue(parameter)]),
  )
  parameterValues.value = { ...values }
  parameterBaseline.value = values
}

function messageOf(error: unknown): string {
  if (error instanceof ApiError && ['ROTATION_CONTROL_REJECTED', 'ROTATION_CONTROL_FAILED'].includes(error.code)) {
    const label = reasonLabels[error.message]
    return label ? `${label}（${error.message}）` : error.message
  }
  return error instanceof Error ? error.message : String(error)
}

function report(error: unknown, prefix = '', notify = true): void {
  pageError.value = `${prefix}${messageOf(error)}`
  if (notify) toastError(pageError.value)
}

function canDiscardParameters(): boolean {
  return !parameterDirty.value || window.confirm('放弃尚未保存的参数修改？')
}

function chooseJob(jobId: number): void {
  if (jobId !== selectedJobId.value && !locked.value && canDiscardParameters()) {
    selectedPlanId.value = null
    selectedJobId.value = jobId
  }
}

function choosePlan(planId: string): void {
  if (planId !== selectedPlanId.value && !locked.value && canDiscardParameters()) {
    selectedPlanId.value = planId
  }
}

async function loadPlans(): Promise<void> {
  const requestId = ++plansRequest
  try {
    const result = await rotationsApi.list()
    if (!result || !Array.isArray(result.plans) || !result.jobs || typeof result.jobs !== 'object'
      || Array.isArray(result.jobs) || typeof result.failClosed !== 'boolean') {
      throw new Error('方案接口返回了无效数据')
    }
    if (disposed || requestId !== plansRequest) {
      throw new ApiError('ROTATION_SNAPSHOT_SUPERSEDED', '方案读取已被更新的请求替代')
    }
    snapshot.value = result
    plansLoaded.value = true
    plansFresh.value = !result.failClosed
  } catch (error) {
    if (!disposed && requestId === plansRequest) {
      plansFresh.value = false
      // The shared client intentionally discards data on failed envelopes.
      if (error instanceof ApiError && error.code === 'ROTATION_STORE_INVALID') {
        snapshot.value = { ...snapshot.value, failClosed: true, initializationError: error.message }
      }
    }
    throw error
  }
}

function invalidateState(message: string): void {
  ++stateRequest
  stateLoaded.value = false
  stateReceivedAt.value = null
  stateError.value = message
}

async function loadState(force = false): Promise<RotationState> {
  const requestId = ++stateRequest
  try {
    const result = await rotationsApi.state(force)
    if (!result || typeof result.running !== 'boolean') throw new Error('运行状态接口返回了无效数据')
    if (disposed || requestId !== stateRequest) {
      throw new ApiError('ROTATION_STATE_SUPERSEDED', '状态读取已被更新的请求替代')
    }
    state.value = result
    stateLoaded.value = true
    stateError.value = ''
    const receivedAt = performance.now()
    stateReceivedAt.value = receivedAt
    freshnessClock.value = receivedAt
    lastUpdated.value = new Date().toLocaleTimeString('zh-CN', { hour12: false })
    if (typeof result.emergencyHotkey === 'string') {
      savedHotkey.value = result.emergencyHotkey
      if (!hotkeyEdited.value && !recordingHotkey.value) hotkey.value = result.emergencyHotkey
    }
    return result
  } catch (error) {
    if (!disposed && requestId === stateRequest) invalidateState(messageOf(error))
    throw error
  }
}

async function performControl(
  body: RotationControlRequest,
  onSettled?: () => void,
): Promise<RotationState> {
  invalidateState('控制请求待确认，正在同步运行状态')
  ++pendingControls.value
  let expired = false
  try {
    await rotationsApi.control(body, {
      onSettled: () => {
        --pendingControls.value
        if (expired && !disposed) invalidateState('迟到的控制请求已返回，等待重新同步')
        onSettled?.()
      },
    })
  } catch (error) {
    expired = true
    invalidateState(`控制结果未确认：${messageOf(error)}`)
    try {
      await loadState(true)
    } catch (recoveryError) {
      throw new Error(`${messageOf(error)}；状态重读失败：${messageOf(recoveryError)}`)
    }
    throw error
  }
  return loadState(true)
}

async function reloadSnapshot(): Promise<void> {
  const results = await Promise.allSettled([loadPlans(), loadState(true)])
  for (const result of results) {
    if (result.status === 'rejected') throw result.reason
  }
}

async function pollState(): Promise<void> {
  if (disposed) return
  try {
    if (!busy.value && !controlsBusy.value) {
      await loadState()
      pollFailures = 0
    }
  } catch (error) {
    // loadState already exposes the error; polling only manages the retry delay.
    if (!(error instanceof ApiError && error.code === 'ROTATION_STATE_SUPERSEDED')) {
      pollFailures = Math.min(pollFailures + 1, 4)
    }
  } finally {
    if (!disposed) pollTimer = setTimeout(pollState, Math.min(1500 * 2 ** pollFailures, 15000))
  }
}

async function refresh(reloadMeta = false): Promise<void> {
  if (busy.value || controlsBusy.value || !canDiscardParameters()) return
  busy.value = true
  pageError.value = ''
  try {
    if (reloadMeta && !meta.loaded) await meta.load()
    if (disposed) return
    await reloadSnapshot()
  } catch (error) {
    report(error, '刷新失败：', reloadMeta)
  } finally {
    busy.value = false
  }
}

async function ensureStopped(): Promise<void> {
  if (!stateFresh.value || state.value.running) {
    ++stopEpoch
    const confirmed = await performControl({ command: 'stop' })
    if (confirmed.running) throw new Error('运行时尚未确认停止，未提交修改')
  }
}

async function mutate(action: () => Promise<unknown>, success: string): Promise<boolean> {
  if (locked.value) return false
  busy.value = true
  pageError.value = ''
  let committed = false
  try {
    await ensureStopped()
    await action()
    committed = true
    plansFresh.value = false
    await reloadSnapshot()
    resetParameters()
    toastSuccess(success)
    return true
  } catch (error) {
    report(error, committed ? '修改已提交，但刷新失败：' : '操作失败：')
    return false
  } finally {
    busy.value = false
  }
}

async function changeMode(value: string | number | boolean): Promise<void> {
  if ((value !== 'legacy' && value !== 'v8') || value === mode.value || selectedJobId.value === null) return
  if (!canDiscardParameters()) return
  const jobId = selectedJobId.value
  await mutate(() => rotationsApi.mode(jobId, value), '已切换职业模式，运行保持停止')
}

async function activatePlan(): Promise<void> {
  const plan = selectedPlan.value
  if (!plan || isActivePlan.value || !canDiscardParameters()) return
  await mutate(() => rotationsApi.select(plan.jobId, plan.id), '已选择方案，运行保持停止')
}

async function saveParameters(): Promise<void> {
  const plan = selectedPlan.value
  if (!plan || !isActivePlan.value || Object.keys(parameterErrors.value).length > 0) return
  const values: Record<string, RotationValue> = {}
  for (const parameter of plan.parameters) {
    const value = parameterValues.value[parameter.id]
    if (typeof value === 'number' || typeof value === 'boolean') values[parameter.id] = value
  }
  await mutate(() => rotationsApi.parameters(plan.jobId, values), '参数已保存，运行保持停止')
}

async function deletePlan(): Promise<void> {
  const plan = selectedPlan.value
  if (!plan || !window.confirm(`确定删除方案“${plan.name}”？此操作不可撤销，运行中的方案将先停止。`)) return
  await mutate(() => rotationsApi.delete(plan.id), '方案已删除')
}

async function startRuntime(): Promise<void> {
  if (busy.value || controlsBusy.value || startBlocked.value || state.value.running) return
  starting.value = true
  pageError.value = ''
  const epoch = stopEpoch
  const expectedJobId = selectedJobId.value
  const requestedHotkey = hotkey.value.trim()
  let dispatched = false
  let settled = false
  let stopLateReply = false
  let errorMessage: string | null = null
  let successMessage = ''
  try {
    const latest = await loadState(true)
    if (disposed || epoch !== stopEpoch) throw new Error('启动已被停止操作取消')
    if (expectedJobId === null || selectedJobId.value !== expectedJobId || latest.jobId !== expectedJobId) {
      throw new Error('页面职业与游戏最新职业不一致，未提交启动')
    }
    if (startBlocked.value) throw new Error(startBlocked.value)
    if (latest.running) throw new Error('运行时已经在运行，请先停止')
    dispatched = true
    const confirmed = await performControl({
      command: 'start', expectedJobId, emergencyHotkey: requestedHotkey,
    }, () => {
      settled = true
      if (stopLateReply || epoch !== stopEpoch) void compensateLateStart()
    })
    if (epoch === stopEpoch) {
      if (typeof confirmed.emergencyHotkey === 'string') {
        hotkey.value = confirmed.emergencyHotkey
        hotkeyEdited.value = false
      }
      successMessage = confirmed.running ? '方案已开始运行' : '启动请求已处理，当前已停止'
    }
  } catch (error) {
    errorMessage = messageOf(error)
  } finally {
    // Reconcile stop intent on both success and failure, including a timed-out start's late reply.
    if (dispatched && epoch !== stopEpoch) {
      stopLateReply = !settled
      try {
        const confirmed = await performControl({ command: 'stop' })
        if (confirmed.running) throw new Error('运行时仍在运行')
      } catch (error) {
        errorMessage = `${errorMessage ? `${errorMessage}；` : ''}补偿停止未确认：${messageOf(error)}`
      }
    }
    starting.value = false
  }
  if (errorMessage !== null) report(new Error(errorMessage), '启动未完成：')
  else if (!disposed && epoch === stopEpoch && successMessage) toastSuccess(successMessage)
}

async function compensateLateStart(): Promise<void> {
  compensating.value = true
  try {
    const confirmed = await performControl({ command: 'stop' })
    if (confirmed.running) throw new Error('运行时仍在运行')
  } catch (error) {
    report(error, '迟到启动请求的补偿停止未确认：')
  } finally {
    compensating.value = false
  }
}

async function stopRuntime(): Promise<void> {
  if (stopping.value) return
  stopping.value = true
  ++stopEpoch
  pageError.value = ''
  try {
    const confirmed = await performControl({ command: 'stop' })
    if (confirmed.running) throw new Error('运行时仍在运行，请再次停止')
    toastSuccess('已停止')
  } catch (error) {
    report(error, '停止状态未确认：')
  } finally {
    stopping.value = false
  }
}

async function resetEmergency(): Promise<void> {
  if (locked.value) return
  resetting.value = true
  ++stopEpoch
  pageError.value = ''
  try {
    const confirmed = await performControl({ command: 'reset' })
    if (confirmed.running || confirmed.emergencyStopped !== false) throw new Error('运行时尚未确认解除急停并停止')
    toastSuccess('急停已解除，运行保持停止')
  } catch (error) {
    report(error, '解除急停失败：')
  } finally {
    resetting.value = false
  }
}

async function saveEmergencyHotkey(): Promise<void> {
  if (locked.value || !hotkeyValid.value) return
  savingHotkey.value = true
  ++stopEpoch
  const requestedHotkey = hotkey.value.trim()
  pageError.value = ''
  try {
    const confirmed = await performControl({ command: 'stop', emergencyHotkey: requestedHotkey })
    if (confirmed.running) throw new Error('运行时尚未确认停止')
    if (typeof confirmed.emergencyHotkey !== 'string') throw new Error('运行时尚未确认急停热键')
    hotkey.value = confirmed.emergencyHotkey
    hotkeyEdited.value = false
    toastSuccess('急停热键已保存，运行已停止')
  } catch (error) {
    report(error, '保存急停热键失败：')
  } finally {
    savingHotkey.value = false
  }
}

function recordHotkey(event: KeyboardEvent): void {
  if (!recordingHotkey.value) return
  event.preventDefault()
  event.stopPropagation()
  if (event.repeat || ['Control', 'Alt', 'Shift', 'Meta'].includes(event.key)) return
  const key = event.code.startsWith('Key') || event.code.startsWith('Digit')
    ? event.code.replace(/^(Key|Digit)/, '')
    : event.key === ' ' ? 'Space' : event.key
  hotkey.value = [
    event.ctrlKey ? 'Ctrl' : '',
    event.altKey ? 'Alt' : '',
    event.shiftKey ? 'Shift' : '',
    event.metaKey ? 'Win' : '',
    key,
  ].filter(Boolean).join('+')
  hotkeyEdited.value = true
  recordingHotkey.value = false
}

function beginRecording(): void {
  if (locked.value) return
  recordingHotkey.value = true
  hotkeyInput.value?.focus()
}

async function readImport(event: Event): Promise<void> {
  const input = event.target as HTMLInputElement
  const file = input.files?.[0]
  input.value = ''
  if (!file || locked.value) return
  importError.value = ''
  try {
    if (file.size > maximumDocumentBytes) throw new Error('文件超过文档处理容量 32 MiB')
    const value: unknown = JSON.parse((await file.text()).replace(/^\uFEFF/, ''))
    if (!value || typeof value !== 'object' || Array.isArray(value)) throw new Error('方案文件必须是 JSON 对象')
    const root = value as Record<string, unknown>
    if (root.format !== 'xiv-rotation' || root.version !== 1) throw new Error('只支持 xiv-rotation version 1 方案')
    if (typeof root.id !== 'string' || !root.id.trim() || typeof root.name !== 'string' || !root.name.trim()) {
      throw new Error('方案缺少有效的 id 或名称')
    }
    if (typeof root.jobId !== 'number' || !jobs.value.some((job) => job.jobId === root.jobId)) {
      throw new Error('方案职业不在当前支持的 22 个 PvE 战斗职业中')
    }
    // The server validates the entire document before the local plan list changes.
    importPlan.value = value as RotationPlan
    importMode.value = 'copy'
  } catch (error) {
    report(error, '读取导入文件失败：')
  }
}

async function submitImport(): Promise<void> {
  const original = importPlan.value
  if (!original || locked.value) return
  const overwrite = importMode.value === 'overwrite' && Boolean(duplicatePlan.value)
  if (overwrite) {
    if (!stateFresh.value || state.value.running) {
      importError.value = '覆盖前请先确认运行已停止'
      return
    }
    if (!window.confirm(`覆盖已保存的方案“${duplicatePlan.value?.name}”？原内容将被替换。`)) return
  }
  busy.value = true
  importError.value = ''
  pageError.value = ''
  let committed = false
  try {
    // Only the root identity changes; variable IDs, phase IDs and all references remain untouched.
    const plan = duplicatePlan.value && !overwrite ? { ...original, id: crypto.randomUUID() } : original
    await rotationsApi.importPlan(plan, overwrite)
    committed = true
    plansFresh.value = false
    await loadPlans()
    if (!parameterDirty.value) {
      selectedJobId.value = plan.jobId
      selectedPlanId.value = plan.id
    }
    importPlan.value = null
    toastSuccess('方案已通过服务端校验并导入')
  } catch (error) {
    importError.value = `${committed ? '导入已提交，但列表刷新失败：' : '导入失败：'}${messageOf(error)}`
    toastError(importError.value)
  } finally {
    busy.value = false
  }
}

function downloadJson(value: unknown, filename: string): void {
  const options = { type: 'application/json;charset=utf-8' }
  let blob = new Blob([`${JSON.stringify(value, null, 2)}\n`], options)
  if (blob.size > maximumDocumentBytes) {
    blob = new Blob([`${JSON.stringify(value)}\n`], options)
    if (blob.size > maximumDocumentBytes) throw new Error('导出文件超过文档处理容量 32 MiB')
  }
  const url = URL.createObjectURL(blob)
  const link = document.createElement('a')
  link.href = url
  link.download = filename
  document.body.appendChild(link)
  try {
    link.click()
  } finally {
    link.remove()
    setTimeout(() => URL.revokeObjectURL(url), 1000)
  }
}

function exportPlan(): void {
  if (!selectedPlan.value || !plansFresh.value) return
  if (parameterDirty.value) {
    toastError('请先保存或放弃参数修改，再导出方案')
    return
  }
  try {
    const name = selectedPlan.value.name
      .replace(/\.rotation\.json$/i, '')
      .replace(/[<>:"/\\|?*\u0000-\u001f]/g, '-')
      .replace(/[. ]+$/g, '')
      .slice(0, 80) || 'rotation'
    downloadJson(selectedPlan.value, `${name}.rotation.json`)
    toastSuccess('方案已导出')
  } catch (error) {
    report(error, '导出方案失败：')
  }
}

async function exportCatalog(): Promise<void> {
  if (exportingCatalog.value) return
  exportingCatalog.value = true
  try {
    downloadJson(await rotationsApi.catalog(), 'xiv.rotation-catalog.json')
    toastSuccess('目录已导出')
  } catch (error) {
    report(error, '导出目录失败：')
  } finally {
    exportingCatalog.value = false
  }
}

onMounted(async () => {
  freshnessTimer = setInterval(() => { freshnessClock.value = performance.now() }, 500)
  await refresh()
  if (!disposed) pollTimer = setTimeout(pollState, 1500)
})

onBeforeUnmount(() => {
  disposed = true
  ++stateRequest
  ++plansRequest
  clearTimeout(pollTimer)
  clearInterval(freshnessTimer)
})
</script>

<template>
  <div class="rotations">
    <header class="rotation-header">
      <h2>8.0 专区</h2>
      <div class="rotation-actions">
        <NTooltip v-if="!currentJob">
          <template #trigger>
            <span class="rotation-tool-trigger">
              <NButton type="error" :loading="stopping" :disabled="stopping" @click="stopRuntime">
                <template #icon><Square :size="16" aria-hidden="true" /></template>
                停止 / 急停
              </NButton>
            </span>
          </template>
          停止运行
        </NTooltip>
        <NTooltip v-if="!currentJob && state.emergencyStopped">
          <template #trigger>
            <span class="rotation-tool-trigger">
              <NButton :loading="resetting" :disabled="locked" @click="resetEmergency">
                <template #icon><RotateCcw :size="16" aria-hidden="true" /></template>
                解除急停
              </NButton>
            </span>
          </template>
          清除急停锁存，保持停止
        </NTooltip>
        <NTooltip>
          <template #trigger>
            <span class="rotation-tool-trigger">
              <NButton class="rotation-icon-button" aria-label="导入方案" :disabled="locked" @click="fileInput?.click()">
                <Upload :size="18" aria-hidden="true" />
              </NButton>
            </span>
          </template>
          导入方案
        </NTooltip>
        <NTooltip>
          <template #trigger>
            <span class="rotation-tool-trigger">
              <NButton class="rotation-icon-button" aria-label="导出目录" :loading="exportingCatalog" @click="exportCatalog">
                <Download :size="18" aria-hidden="true" />
              </NButton>
            </span>
          </template>
          导出技能与字段目录
        </NTooltip>
        <NTooltip>
          <template #trigger>
            <span class="rotation-tool-trigger">
              <NButton class="rotation-icon-button" aria-label="刷新" :loading="busy" :disabled="busy || controlsBusy" @click="refresh(true)">
                <RefreshCw :size="18" aria-hidden="true" />
              </NButton>
            </span>
          </template>
          刷新方案与运行状态
        </NTooltip>
      </div>
      <input ref="fileInput" class="rotation-file" type="file" accept=".rotation.json,.json,application/json" @change="readImport" />
    </header>

    <NAlert v-if="snapshot.failClosed" type="error" title="方案存储已锁定" class="rotation-alert">
      <p class="rotation-store-message">请先修复 rotation-store.json 并重新加载运行时。当前禁止启动、导入及配置写入，停止操作仍可使用。</p>
      <div class="rotation-error-detail">{{ storeError }}</div>
    </NAlert>
    <NAlert v-if="pageError" type="error" class="rotation-alert" closable @close="pageError = ''">
      {{ pageError }}
    </NAlert>
    <NAlert v-if="meta.error" type="error" class="rotation-alert">职业数据加载失败：{{ meta.error }}</NAlert>
    <NAlert v-if="stateProblem && !currentJob" type="warning" class="rotation-alert">状态待确认：{{ stateProblem }}</NAlert>
    <NAlert v-if="pendingControls > 0 && !controlsBusy" type="warning" class="rotation-alert">
      仍有控制请求未收到响应，已禁止启动及配置写入；可再次停止。
    </NAlert>

    <div class="rotation-layout">
      <aside class="rotation-jobs">
        <NInput v-model:value="jobSearch" placeholder="搜索职业" size="small" clearable />
        <div class="rotation-job-list sloth-scroll">
          <button
            v-for="job in filteredJobs"
            :key="job.jobId"
            type="button"
            class="rotation-job"
            :class="{ 'rotation-job-active': job.jobId === selectedJobId }"
            :disabled="locked"
            :aria-pressed="job.jobId === selectedJobId"
            @click="chooseJob(job.jobId)"
          >
            <JobIcon :icon-id="job.iconId" :size="28" />
            <span class="rotation-job-text">
              <span>{{ job.name }} <span class="sloth-muted">{{ job.shorthand }}</span></span>
              <small class="sloth-muted">{{ !plansFresh ? '模式未确认' : snapshot.jobs[String(job.jobId)]?.mode === 'v8' ? '8.0 方案' : '传统连招' }}</small>
            </span>
          </button>
          <NEmpty v-if="filteredJobs.length === 0" description="没有匹配的职业" size="small" />
        </div>
      </aside>

      <main class="rotation-content sloth-scroll">
        <NSpin v-if="busy && !plansLoaded" size="small" />
        <NEmpty v-else-if="!currentJob" description="没有可用的 PvE 战斗职业" />
        <template v-else>
          <section class="rotation-section">
            <div class="rotation-section-head">
              <div class="rotation-title">
                <JobIcon :icon-id="currentJob.iconId" :size="30" />
                <h3>{{ currentJob.name }}</h3>
              </div>
              <NRadioGroup v-if="plansFresh" :value="mode" :disabled="locked" size="small" aria-label="职业连招模式" @update:value="changeMode">
                <NRadioButton value="legacy">传统连招</NRadioButton>
                <NRadioButton value="v8">8.0 方案</NRadioButton>
              </NRadioGroup>
              <NTag v-else size="small" type="warning">{{ snapshot.failClosed ? '存储锁定' : '模式未确认' }}</NTag>
            </div>
            <p v-if="currentJob.notice" class="rotation-notice">{{ currentJob.notice }}</p>
            <NAlert v-if="plansFresh && mode === 'v8' && !activePlan" type="warning">
              当前职业仍为 8.0 模式，尚未选择方案；运行保持停止，不回退传统连招。
            </NAlert>
          </section>

          <section class="rotation-section" aria-label="运行状态">
            <div class="rotation-section-head">
              <h3>运行状态</h3>
              <NTag :type="stateProblem ? 'warning' : state.emergencyStopped ? 'error' : state.available === false ? 'warning' : stateFresh && state.running ? 'success' : 'default'" size="small">
                {{ runtimeLabel }}
              </NTag>
            </div>
            <NAlert v-if="stateProblem" type="warning" class="rotation-alert">状态待确认：{{ stateProblem }}</NAlert>
            <dl class="rotation-status">
              <div><dt>运行职业</dt><dd>{{ runtimeJobName }}</dd></div>
              <div><dt>当前阶段</dt><dd>{{ stateFresh ? state.phaseId ?? '—' : '—' }}</dd></div>
              <div><dt>待执行技能</dt><dd>{{ stateFresh ? state.pendingActionId || '—' : '—' }}</dd></div>
              <div><dt>最近同步</dt><dd>{{ lastUpdated || '—' }}</dd></div>
            </dl>
            <template v-if="stateFresh && state.reason">
              <p class="rotation-reason">{{ reasonLabel }}</p>
              <details class="rotation-reason-detail">
                <summary>状态详情</summary>
                <code>{{ state.reason }}</code>
              </details>
            </template>
            <div class="rotation-actions">
              <NTooltip>
                <template #trigger>
                  <span class="rotation-tool-trigger">
                    <NButton type="primary" :loading="starting" :disabled="busy || controlsBusy || Boolean(startBlocked) || state.running" @click="startRuntime">
                      <template #icon><Play :size="16" aria-hidden="true" /></template>
                      开始
                    </NButton>
                  </span>
                </template>
                {{ startBlocked || (state.running ? '方案正在运行' : '开始运行当前方案') }}
              </NTooltip>
              <NTooltip>
                <template #trigger>
                  <span class="rotation-tool-trigger">
                    <NButton type="error" :loading="stopping" :disabled="stopping" @click="stopRuntime">
                      <template #icon><Square :size="16" aria-hidden="true" /></template>
                      停止 / 急停
                    </NButton>
                  </span>
                </template>
                停止运行
              </NTooltip>
              <NTooltip v-if="state.emergencyStopped">
                <template #trigger>
                  <span class="rotation-tool-trigger">
                    <NButton :loading="resetting" :disabled="locked" @click="resetEmergency">
                      <template #icon><RotateCcw :size="16" aria-hidden="true" /></template>
                      解除急停
                    </NButton>
                  </span>
                </template>
                清除急停锁存，保持停止
              </NTooltip>
            </div>
            <p v-if="startBlocked && (!stateFresh || !state.running)" class="rotation-hint sloth-muted">{{ startBlocked }}</p>
            <div class="rotation-hotkey">
              <label for="rotation-hotkey">急停热键</label>
              <NInput
                ref="hotkeyInput"
                :value="hotkey"
                :input-props="{ id: 'rotation-hotkey', 'aria-label': '急停热键' }"
                :readonly="recordingHotkey"
                :disabled="locked"
                placeholder="Ctrl+Alt+F12"
                @update:value="(value: string) => { hotkey = value; hotkeyEdited = true }"
                @keydown="recordHotkey"
                @blur="recordingHotkey = false"
              />
              <NTooltip>
                <template #trigger>
                  <span class="rotation-tool-trigger">
                    <NButton :disabled="locked" @click="beginRecording">
                      <template #icon><Keyboard :size="16" aria-hidden="true" /></template>
                      {{ recordingHotkey ? '等待按键' : '录制' }}
                    </NButton>
                  </span>
                </template>
                录制急停热键
              </NTooltip>
              <NTooltip>
                <template #trigger>
                  <span class="rotation-tool-trigger">
                    <NButton :loading="savingHotkey" :disabled="locked || !hotkeyValid" @click="saveEmergencyHotkey">
                      <template #icon><Save :size="16" aria-hidden="true" /></template>
                      保存并停止
                    </NButton>
                  </span>
                </template>
                保存急停热键并停止运行
              </NTooltip>
              <span v-if="hotkeyEdited || savedHotkey === null" class="sloth-muted rotation-hint">未保存</span>
            </div>
          </section>

          <section class="rotation-section">
            <div class="rotation-section-head">
              <h3>方案管理</h3>
              <span class="sloth-muted">{{ jobPlans.length }} 个方案</span>
            </div>
            <NEmpty v-if="jobPlans.length === 0" description="当前职业暂无方案" size="small">
              <template #extra>
                <NButton :disabled="locked" @click="fileInput?.click()">
                  <template #icon><Upload :size="16" aria-hidden="true" /></template>
                  导入方案
                </NButton>
              </template>
            </NEmpty>
            <template v-else>
              <div class="rotation-plan-picker">
                <NSelect :value="selectedPlanId" :options="planOptions" :disabled="locked" filterable placeholder="选择方案" @update:value="choosePlan" />
                <NButton :disabled="locked || isActivePlan || !selectedPlan" @click="activatePlan">
                  <template #icon><Check :size="16" aria-hidden="true" /></template>
                  设为当前
                </NButton>
              </div>
              <template v-if="selectedPlan">
                <dl class="rotation-plan-meta">
                  <div><dt>方案 ID</dt><dd>{{ selectedPlan.id }}</dd></div>
                  <div><dt>入口阶段</dt><dd>{{ selectedPlan.entryPhaseId }}</dd></div>
                  <div><dt>阶段 / 变量</dt><dd>{{ selectedPlan.phases.length }} / {{ selectedPlan.variables.length }}</dd></div>
                </dl>
                <div class="rotation-actions">
                  <NTag v-if="isActivePlan" size="small" type="success">当前方案</NTag>
                  <NTooltip>
                    <template #trigger>
                      <span class="rotation-tool-trigger">
                        <NButton class="rotation-icon-button" aria-label="导出方案" :disabled="locked" @click="exportPlan">
                          <Download :size="18" aria-hidden="true" />
                        </NButton>
                      </span>
                    </template>
                    导出当前查看的方案
                  </NTooltip>
                  <NTooltip>
                    <template #trigger>
                      <span class="rotation-tool-trigger">
                        <NButton class="rotation-icon-button" aria-label="删除方案" type="error" :disabled="locked" @click="deletePlan">
                          <Trash2 :size="18" aria-hidden="true" />
                        </NButton>
                      </span>
                    </template>
                    删除当前查看的方案
                  </NTooltip>
                </div>
              </template>
            </template>
          </section>

          <section v-if="selectedPlan" class="rotation-section">
            <div class="rotation-section-head">
              <h3>方案参数</h3>
              <NTag v-if="parameterDirty" size="small" type="warning">未保存</NTag>
            </div>
            <NEmpty v-if="selectedPlan.parameters.length === 0" description="此方案未声明可调参数" size="small" />
            <template v-else>
              <NForm label-placement="left" label-width="auto" class="rotation-parameters" :disabled="locked || !isActivePlan">
                <NFormItem
                  v-for="parameter in selectedPlan.parameters"
                  :key="parameter.id"
                  :validation-status="parameterErrors[parameter.id] ? 'error' : undefined"
                  :feedback="parameterErrors[parameter.id]"
                >
                  <template #label>
                    <span class="rotation-parameter-label">{{ parameter.name }}<small class="sloth-muted">{{ parameter.id }}</small></span>
                  </template>
                  <NSwitch
                    v-if="parameter.type === 'boolean'"
                    :value="parameterValues[parameter.id] === true"
                    :aria-label="parameter.name"
                    @update:value="(value: boolean) => parameterValues[parameter.id] = value"
                  />
                  <NInputNumber
                    v-else
                    :value="typeof parameterValues[parameter.id] === 'number' ? Number(parameterValues[parameter.id]) : null"
                    :min="parameter.min"
                    :max="parameter.max"
                    :aria-label="parameter.name"
                    @update:value="(value: number | null) => parameterValues[parameter.id] = value"
                  />
                </NFormItem>
              </NForm>
              <div class="rotation-actions">
                <NButton type="primary" :disabled="locked || !isActivePlan || !parameterDirty || Object.keys(parameterErrors).length > 0" @click="saveParameters">
                  <template #icon><Save :size="16" aria-hidden="true" /></template>
                  保存参数
                </NButton>
                <NButton :disabled="locked || !parameterDirty" @click="resetParameters">
                  <template #icon><Undo2 :size="16" aria-hidden="true" /></template>
                  放弃修改
                </NButton>
                <span v-if="!isActivePlan" class="sloth-muted rotation-hint">非当前方案，参数只读</span>
              </div>
            </template>
          </section>
        </template>
      </main>
    </div>

    <NModal
      :show="importPlan !== null"
      preset="card"
      title="导入方案"
      class="rotation-import"
      :mask-closable="!busy"
      :close-on-esc="!busy"
      :closable="!busy"
      @update:show="(show: boolean) => { if (!show && !busy) importPlan = null }"
    >
      <template v-if="importPlan">
        <dl class="rotation-plan-meta">
          <div><dt>名称</dt><dd>{{ importPlan.name }}</dd></div>
          <div><dt>职业</dt><dd>{{ jobs.find((job) => job.jobId === importPlan?.jobId)?.name }}</dd></div>
          <div><dt>方案 ID</dt><dd>{{ importPlan.id }}</dd></div>
        </dl>
        <NRadioGroup v-if="duplicatePlan" v-model:value="importMode" :disabled="busy" aria-label="重复方案处理">
          <NRadioButton value="copy">另存新方案</NRadioButton>
          <NRadioButton value="overwrite">覆盖同 ID 方案</NRadioButton>
        </NRadioGroup>
        <NAlert v-if="duplicatePlan" type="warning" class="rotation-import-notice">已存在同 ID 方案：{{ duplicatePlan.name }}</NAlert>
        <NAlert v-if="importError" type="error" class="rotation-import-notice">{{ importError }}</NAlert>
      </template>
      <template #footer>
        <div class="rotation-actions rotation-footer">
          <NButton :disabled="busy" @click="importPlan = null">取消</NButton>
          <NButton type="primary" :loading="busy" :disabled="locked" @click="submitImport">
            <template #icon><Upload :size="16" aria-hidden="true" /></template>
            校验并导入
          </NButton>
        </div>
      </template>
    </NModal>
  </div>
</template>

<style scoped>
.rotations {
  display: flex;
  flex-direction: column;
  gap: 12px;
  height: 100%;
  min-height: 0;
  letter-spacing: 0;
}

.rotation-header,
.rotation-section-head,
.rotation-title,
.rotation-actions {
  display: flex;
  align-items: center;
  gap: 10px;
}

.rotation-header,
.rotation-section-head {
  justify-content: space-between;
  flex-wrap: wrap;
}

.rotation-header h2 {
  margin: 0;
  font-size: 18px;
}

.rotation-actions {
  flex-wrap: wrap;
}

.rotation-tool-trigger {
  display: inline-flex;
  align-items: center;
  flex: 0 0 auto;
  max-width: 100%;
}

.rotation-icon-button {
  width: 34px;
  height: 34px;
  padding: 0;
}

.rotation-file {
  display: none;
}

.rotation-layout {
  display: flex;
  flex: 1 1 auto;
  gap: 16px;
  min-height: 0;
}

.rotation-jobs {
  flex: 0 0 220px;
  display: flex;
  flex-direction: column;
  gap: 8px;
  min-height: 0;
  min-width: 0;
  padding-right: 12px;
  border-right: 1px solid var(--sloth-border-soft);
}

.rotation-job-list {
  min-height: 0;
  flex: 1;
}

.rotation-job {
  display: flex;
  align-items: center;
  gap: 10px;
  width: 100%;
  min-height: 52px;
  padding: 6px 8px;
  border: 1px solid transparent;
  border-radius: var(--sloth-radius);
  background: transparent;
  color: inherit;
  font: inherit;
  text-align: left;
  cursor: pointer;
}

.rotation-job:hover {
  background: var(--sloth-bg-soft);
}

.rotation-job:disabled {
  cursor: default;
}

.rotation-job-active {
  border-color: var(--sloth-accent);
  background: var(--sloth-accent-soft);
}

.rotation-job-text,
.rotation-parameter-label {
  display: flex;
  flex-direction: column;
  min-width: 0;
  overflow-wrap: anywhere;
}

.rotation-job-text small,
.rotation-parameter-label small,
.rotation-hint {
  font-size: 12px;
}

.rotation-content {
  flex: 1;
  min-width: 0;
  padding-right: 8px;
}

.rotation-section {
  padding: 4px 0 18px;
  margin-bottom: 14px;
  border-bottom: 1px solid var(--sloth-border-soft);
}

.rotation-section h3 {
  margin: 0;
  font-size: 15px;
}

.rotation-section-head {
  margin-bottom: 12px;
}

.rotation-status {
  display: grid;
  grid-template-columns: repeat(4, minmax(0, 1fr));
  gap: 12px;
  margin: 0 0 12px;
}

.rotation-status dt,
.rotation-plan-meta dt {
  color: var(--sloth-text-dim);
  font-size: 12px;
}

.rotation-status dd,
.rotation-plan-meta dd {
  margin: 0;
  overflow-wrap: anywhere;
  white-space: pre-wrap;
}

.rotation-reason,
.rotation-notice {
  margin: 8px 0 12px;
  white-space: pre-wrap;
  overflow-wrap: anywhere;
}

.rotation-reason-detail {
  margin: 0 0 12px;
  color: var(--sloth-text-dim);
  font-size: 12px;
  overflow-wrap: anywhere;
}

.rotation-reason-detail summary {
  width: fit-content;
  cursor: pointer;
}

.rotation-reason-detail code {
  display: block;
  margin-top: 4px;
  white-space: pre-wrap;
}

.rotation-store-message {
  margin: 0 0 8px;
}

.rotation-error-detail {
  white-space: pre-wrap;
  overflow-wrap: anywhere;
}

.rotation-hint {
  margin: 6px 0 0;
}

.rotation-hotkey {
  display: grid;
  grid-template-columns: auto minmax(130px, 250px) auto auto;
  align-items: center;
  justify-content: start;
  gap: 8px;
  margin-top: 16px;
}

.rotation-hotkey .rotation-hint {
  grid-column: 2 / -1;
  margin: 0;
}

.rotation-plan-picker {
  display: grid;
  grid-template-columns: minmax(0, 1fr) max-content;
  gap: 8px;
}

.rotation-plan-meta {
  display: grid;
  gap: 8px;
  margin: 12px 0;
}

.rotation-plan-meta > div {
  display: grid;
  grid-template-columns: 88px minmax(0, 1fr);
  align-items: baseline;
  gap: 8px;
}

.rotation-parameters {
  max-width: 640px;
}

.rotation-parameters :deep(.n-input-number) {
  width: 200px;
  max-width: 100%;
}

.rotation-parameters :deep(.n-form-item-label) {
  max-width: 50%;
}

.rotation-alert {
  flex: 0 0 auto;
  overflow-wrap: anywhere;
}

.rotation-import {
  width: min(520px, calc(100vw - 32px));
}

.rotation-import-notice {
  margin-top: 12px;
  overflow-wrap: anywhere;
}

.rotation-footer {
  justify-content: flex-end;
}

@media (max-width: 1100px) {
  .rotation-jobs {
    flex-basis: 180px;
  }

  .rotation-status {
    grid-template-columns: repeat(2, minmax(0, 1fr));
  }

  .rotation-hotkey {
    grid-template-columns: auto minmax(0, 1fr);
  }

  .rotation-hotkey .rotation-hint {
    grid-column: auto;
  }
}

@media (max-width: 700px) {
  .rotation-layout {
    flex-direction: column;
  }

  .rotation-jobs {
    flex: 0 0 148px;
    padding: 0 0 8px;
    border-right: 0;
    border-bottom: 1px solid var(--sloth-border-soft);
  }

  .rotation-job-list {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
  }

  .rotation-content {
    min-height: 0;
    padding-right: 0;
  }

  .rotation-parameters :deep(.n-form-item) {
    grid-template-columns: minmax(0, 1fr);
    grid-template-areas: "label" "blank" "feedback";
    grid-template-rows: auto auto auto;
  }

  .rotation-parameters :deep(.n-form-item-label) {
    max-width: 100%;
    justify-content: flex-start;
    padding: 0 0 6px;
    text-align: left;
    white-space: normal;
  }
}
</style>
