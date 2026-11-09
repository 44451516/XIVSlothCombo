<script setup lang="ts">
import { computed, onMounted, reactive, ref } from 'vue'
import { NButton, NCard, NEmpty, NInputNumber, NModal, NSelect, NSpace } from 'naive-ui'
import { useTimelineStore } from '@/stores/timeline'
import type { TimelineActionDto } from '@/api/types'
import { toastError, toastSuccess } from '@/composables/useToast'

const store = useTimelineStore()
const busy = ref(false)

interface Draft {
  actionId: number
  useTimeStart: number
  useTimeEnd: number
  targetType: number
  customActionType: number
  customActionTypeSub: number
  x: number
  y: number
  z: number
}

/** `null` closes the dialog, a number edits the action at that index. */
const editing = ref<number | null>(null)
const inserting = ref<{ at: number } | null>(null)
const draft = reactive<Draft>({
  actionId: 0,
  useTimeStart: 0,
  useTimeEnd: 0,
  targetType: 0,
  customActionType: 1,
  customActionTypeSub: 0,
  x: 0,
  y: 0,
  z: 0,
})

const dialogOpen = computed(() => editing.value !== null || inserting.value !== null)

onMounted(async () => {
  try {
    await store.load()
  } catch (error) {
    toastError(error instanceof Error ? error.message : String(error))
  }
})

const typeOptions = computed(() =>
  store.customTypes.map((type) => ({ label: type.label, value: type.value })),
)

const typeSubOptions = computed(() =>
  store.customTypeSubs.map((type) => ({ label: type.label, value: type.value })),
)

async function runRaw(
  action: string,
  payload: Record<string, unknown> = {},
): Promise<Record<string, unknown>> {
  busy.value = true
  try {
    return await store.run(action, payload)
  } catch (error) {
    toastError(error instanceof Error ? error.message : String(error))
    throw error
  } finally {
    busy.value = false
  }
}

async function run(action: string, payload: Record<string, unknown> = {}): Promise<void> {
  await runRaw(action, payload).catch(() => undefined)
}

function openEditor(index: number): void {
  const action = store.working[index]
  if (!action) return

  draft.actionId = action.actionId
  draft.useTimeStart = action.useTimeStart
  draft.useTimeEnd = action.useTimeEnd
  draft.targetType = action.targetType
  draft.customActionType = action.customActionType
  draft.customActionTypeSub = action.customActionTypeSub
  draft.x = action.x
  draft.y = action.y
  draft.z = action.z

  editing.value = index
  inserting.value = null
}

function openInsert(at: number): void {
  draft.actionId = 0
  draft.useTimeStart = 0
  draft.useTimeEnd = 0
  draft.targetType = 0
  draft.customActionType = store.customTypes[0]?.value ?? 1
  draft.customActionTypeSub = store.customTypeSubs[0]?.value ?? 0
  draft.x = 0
  draft.y = 0
  draft.z = 0

  inserting.value = { at }
  editing.value = null
}

function closeDialog(): void {
  editing.value = null
  inserting.value = null
}

function toDraft(): TimelineActionDto {
  return {
    actionId: draft.actionId,
    actionName: null,
    actionIcon: null,
    useTimeStart: draft.useTimeStart,
    useTimeEnd: draft.useTimeEnd,
    targetType: draft.targetType,
    customActionType: draft.customActionType,
    customActionTypeSub: draft.customActionTypeSub,
    x: draft.x,
    y: draft.y,
    z: draft.z,
  }
}

/** Writes the working copy back into the plugin. */
async function pushWorking(actions: TimelineActionDto[]): Promise<void> {
  await run('setWorking', { actions })
}

async function saveDraft(): Promise<void> {
  const entry = toDraft()

  try {
    if (editing.value !== null) {
      const next = store.working.slice()
      next[editing.value] = entry
      await pushWorking(next)
      toastSuccess('已保存')
    } else if (inserting.value) {
      const next = store.working.slice()
      next.splice(inserting.value.at, 0, entry)
      await pushWorking(next)
      toastSuccess('已插入')
    }

    closeDialog()
  } catch {
    // The error was already reported.
  }
}

async function removeAction(index: number): Promise<void> {
  if (!window.confirm('确定要删除这个动作吗？')) return

  const next = store.working.slice()
  next.splice(index, 1)
  await pushWorking(next)
  toastSuccess('已删除')
}

async function readRunning(): Promise<void> {
  await run('load')
  toastSuccess('已读取当前使用的时间轴')
}

async function save(): Promise<void> {
  await run('save')
  toastSuccess('已保存到当前时间轴')
}

async function saveAs(): Promise<void> {
  const name = window.prompt('请输入名字')
  if (name === null) return
  if (name.trim() === '') {
    toastError('请输入名字。')
    return
  }

  await run('saveAs', { name: name.trim() })
  toastSuccess('已另存为')
}

async function clearWorking(): Promise<void> {
  if (!window.confirm('确定要清空当前编辑中的时间轴吗？')) return
  await run('clearWorking')
  toastSuccess('已清空')
}

async function copyWorking(): Promise<void> {
  let result: Record<string, unknown>
  try {
    result = await runRaw('exportWorking')
  } catch {
    return
  }

  const json = result.json
  if (typeof json !== 'string') {
    toastError('导出失败。')
    return
  }

  try {
    await navigator.clipboard.writeText(json)
    toastSuccess('已复制到剪贴板')
  } catch {
    toastError('无法写入剪贴板，请确认浏览器已授予权限。')
  }
}
</script>

<template>
  <div class="editor sloth-scroll">
    <NCard title="时间轴编辑" :bordered="false" class="editor-card">
      <p class="sloth-muted editor-hint">
        这里编辑的是当前生效的时间轴副本。修改后点击「保存」写回正在使用的时间轴。
      </p>

      <div class="editor-actions">
        <NButton :disabled="busy" @click="copyWorking">复制</NButton>
        <NButton :disabled="busy" @click="readRunning">读取</NButton>
        <NButton :disabled="busy" type="primary" @click="save">保存</NButton>
        <NButton :disabled="busy" @click="saveAs">另存为</NButton>
        <NButton :disabled="busy" type="warning" @click="clearWorking">清空</NButton>
      </div>
    </NCard>

    <NCard title="动作列表" :bordered="false" class="editor-card">
      <NEmpty v-if="store.working.length === 0" description="当前没有正在编辑的动作" />

      <div v-for="(action, index) in store.working" :key="index" class="editor-item">
        <span class="editor-index sloth-muted">{{ index + 1 }}</span>

        <span class="editor-name sloth-nowrap">
          {{ action.actionName ?? `技能 ${action.actionId}` }}
          <span class="sloth-muted">[{{ action.actionId }}] [{{ action.useTimeStart }}]</span>
        </span>

        <NSpace :size="8">
          <NButton size="small" :disabled="busy" @click="openEditor(index)">编辑</NButton>
          <NButton size="small" :disabled="busy" @click="openInsert(index)">前插</NButton>
          <NButton size="small" :disabled="busy" @click="openInsert(index + 1)">后插</NButton>
          <NButton size="small" type="error" :disabled="busy" @click="removeAction(index)">删除</NButton>
        </NSpace>
      </div>
    </NCard>

    <NModal
      :show="dialogOpen"
      preset="card"
      :title="editing !== null ? '编辑动作' : '插入动作'"
      style="max-width: 520px"
      @update:show="(show: boolean) => !show && closeDialog()"
    >
      <div class="editor-form">
        <label class="editor-field">
          <span class="editor-field-label">技能 Id</span>
          <NInputNumber v-model:value="draft.actionId" :min="0" :show-button="false" />
        </label>

        <label class="editor-field">
          <span class="editor-field-label">释放开始时间</span>
          <NInputNumber v-model:value="draft.useTimeStart" :step="0.1" :show-button="false" />
        </label>

        <label class="editor-field">
          <span class="editor-field-label">释放最晚时间</span>
          <NInputNumber v-model:value="draft.useTimeEnd" :step="0.1" :show-button="false" />
        </label>

        <label class="editor-field">
          <span class="editor-field-label">目标类别</span>
          <NInputNumber v-model:value="draft.targetType" :min="0" :max="20" :show-button="false" />
        </label>

        <label class="editor-field">
          <span class="editor-field-label">技能类别</span>
          <NSelect v-model:value="draft.customActionType" :options="typeOptions" />
        </label>

        <label class="editor-field">
          <span class="editor-field-label">替换类别</span>
          <NSelect v-model:value="draft.customActionTypeSub" :options="typeSubOptions" />
        </label>

        <label class="editor-field">
          <span class="editor-field-label">X</span>
          <NInputNumber v-model:value="draft.x" :step="0.1" :show-button="false" />
        </label>

        <label class="editor-field">
          <span class="editor-field-label">Y</span>
          <NInputNumber v-model:value="draft.y" :step="0.1" :show-button="false" />
        </label>

        <label class="editor-field">
          <span class="editor-field-label">Z</span>
          <NInputNumber v-model:value="draft.z" :step="0.1" :show-button="false" />
        </label>
      </div>

      <template #footer>
        <NSpace justify="end">
          <NButton @click="closeDialog">关闭</NButton>
          <NButton type="primary" @click="saveDraft">保存</NButton>
        </NSpace>
      </template>
    </NModal>
  </div>
</template>

<style scoped>
.editor {
  max-width: 980px;
}

.editor-card {
  margin-bottom: 12px;
}

.editor-hint {
  margin: 0 0 8px;
  font-size: 12px;
}

.editor-actions {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.editor-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 7px 0;
  border-bottom: 1px solid var(--sloth-border-soft);
}

.editor-item:last-child {
  border-bottom: none;
}

.editor-index {
  flex: 0 0 32px;
  font-variant-numeric: tabular-nums;
}

.editor-name {
  flex: 1 1 auto;
  min-width: 0;
}

.editor-form {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 12px 16px;
}

.editor-field {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.editor-field-label {
  font-size: 12px;
  color: var(--sloth-text-dim);
}
</style>
