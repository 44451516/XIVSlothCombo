<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { NButton, NCard, NEmpty, NInput, NModal, NSpace, NSwitch } from 'naive-ui'
import { useTimelineStore } from '@/stores/timeline'
import { jobIconId } from '@/api/icons'
import JobIcon from '@/components/JobIcon.vue'
import { toastError, toastSuccess } from '@/composables/useToast'

const store = useTimelineStore()
const busy = ref(false)
const renaming = ref<{ index: number; name: string } | null>(null)

onMounted(async () => {
  try {
    await store.load()
  } catch (error) {
    toastError(error instanceof Error ? error.message : String(error))
  }
})

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

async function loadTimeline(index: number): Promise<void> {
  await run('loadTimeline', { index })
  toastSuccess('已加载时间轴')
}

async function rename(): Promise<void> {
  if (!renaming.value) return
  const { index, name } = renaming.value
  if (name.trim() === '') {
    toastError('请输入名字。')
    return
  }

  renaming.value = null
  await run('rename', { index, name: name.trim() })
  toastSuccess('已重命名')
}

async function remove(index: number): Promise<void> {
  if (!window.confirm('确定要删除这个时间轴吗？')) return
  await run('delete', { index })
  toastSuccess('已删除')
}

async function clearAll(): Promise<void> {
  if (!window.confirm('确定要清空全部时间轴吗？此操作不可撤销。')) return
  await run('clearAll')
  toastSuccess('已清空')
}

async function importFromClipboard(): Promise<void> {
  let text: string
  try {
    text = await navigator.clipboard.readText()
  } catch {
    toastError('无法读取剪贴板，请确认浏览器已授予权限。')
    return
  }

  if (!text.trim()) {
    toastError('剪贴板内容为空。')
    return
  }

  await run('import', { json: text })
  toastSuccess('导入成功')
}

async function exportToClipboard(index: number): Promise<void> {
  let result: Record<string, unknown>
  try {
    result = await runRaw('export', { index })
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
  <div class="timeline sloth-scroll">
    <NCard title="时间轴" :bordered="false" class="timeline-card">
      <p v-if="store.current" class="sloth-muted timeline-current">
        当前使用的时间轴是：{{ store.current.index }} - {{ store.current.jobId }} - {{ store.current.name }}
      </p>

      <div class="timeline-row">
        <div class="timeline-label">是否使用爆发药</div>
        <NSwitch
          :value="store.useItem"
          :disabled="busy"
          @update:value="(value: boolean) => run('setUseItem', { value })"
        />
      </div>

      <div class="timeline-actions">
        <NButton :disabled="busy" @click="run('disableAll')">停用</NButton>
        <NButton :disabled="busy" @click="importFromClipboard">导入</NButton>
        <NButton :disabled="busy" type="warning" @click="clearAll">清空</NButton>
        <NButton
          tag="a"
          href="https://docs.qq.com/doc/DT2hlRGl5WWhVTGNM"
          target="_blank"
          rel="noopener noreferrer"
        >
          帮助
        </NButton>
      </div>
    </NCard>

    <NCard title="已保存的时间轴" :bordered="false" class="timeline-card">
      <NEmpty v-if="store.timelines.length === 0" description="还没有保存任何时间轴" />

      <div v-for="timeline in store.timelines" :key="timeline.index" class="timeline-item">
        <JobIcon :icon-id="jobIconId(timeline.jobId)" :size="26" />

        <span class="timeline-name sloth-nowrap">
          {{ timeline.index }}. {{ timeline.name }}
          <span class="sloth-muted">（{{ timeline.actionList.length }} 个动作）</span>
        </span>

        <NSpace :size="8">
          <NButton size="small" :disabled="busy" @click="loadTimeline(timeline.index - 1)">加载</NButton>
          <NButton
            size="small"
            :disabled="busy"
            @click="renaming = { index: timeline.index - 1, name: timeline.name }"
          >
            重命名
          </NButton>
          <NButton size="small" :disabled="busy" @click="exportToClipboard(timeline.index - 1)">导出</NButton>
          <NButton size="small" type="error" :disabled="busy" @click="remove(timeline.index - 1)">删除</NButton>
        </NSpace>
      </div>
    </NCard>

    <NModal
      :show="renaming !== null"
      preset="card"
      title="重命名时间轴"
      style="max-width: 420px"
      @update:show="(show: boolean) => !show && (renaming = null)"
    >
      <NInput
        v-model:value="renaming!.name"
        placeholder="请输入名字"
        @keyup.enter="rename"
      />
      <template #footer>
        <NSpace justify="end">
          <NButton @click="renaming = null">取消</NButton>
          <NButton type="primary" @click="rename">保存</NButton>
        </NSpace>
      </template>
    </NModal>
  </div>
</template>

<style scoped>
.timeline {
  max-width: 980px;
}

.timeline-card {
  margin-bottom: 12px;
}

.timeline-current {
  margin: 0 0 8px;
  font-size: 13px;
}

.timeline-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 20px;
  padding: 8px 0;
}

.timeline-label {
  flex: 1 1 auto;
}

.timeline-actions {
  display: flex;
  gap: 10px;
  margin-top: 8px;
}

.timeline-item {
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 8px 0;
  border-bottom: 1px solid var(--sloth-border-soft);
}

.timeline-item:last-child {
  border-bottom: none;
}

.timeline-name {
  flex: 1 1 auto;
  min-width: 0;
}
</style>
