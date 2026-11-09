<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { NEmpty, NInput, NSpin, NTabPane, NTabs } from 'naive-ui'
import { useMetaStore } from '@/stores/meta'
import type { PresetNode } from '@/api/types'
import JobIcon from './JobIcon.vue'
import PresetCard from './PresetCard.vue'

const props = defineProps<{ mode: 'pve' | 'pvp' }>()

const meta = useMetaStore()

const selectedJob = ref('')
const jobSearch = ref('')
const featureSearch = ref('')
const activeTab = ref('normal')

const isPvp = computed(() => props.mode === 'pvp')

/** The jobs that own at least one feature of the current mode. */
const jobList = computed(() => {
  const groups = isPvp.value ? meta.pvpGroups : meta.pveGroups
  return meta.jobs.filter((job) => groups.has(job.name))
})

const filteredJobs = computed(() => {
  const keyword = jobSearch.value.trim().toLowerCase()
  if (!keyword) return jobList.value

  return jobList.value.filter((job) =>
    `${job.name} ${job.shorthand}`.toLowerCase().includes(keyword),
  )
})

/** The top level features of the selected job. */
const roots = computed<PresetNode[]>(() => {
  const list = isPvp.value ? meta.pvpPresets : meta.pvePresets
  return list.filter((preset) => preset.jobName === selectedJob.value && !preset.parentId)
})

const filteredRoots = computed(() => {
  const keyword = featureSearch.value.trim().toLowerCase()
  if (!keyword) return roots.value

  return roots.value.filter((preset) =>
    `${preset.fancyName} ${preset.description}`.toLowerCase().includes(keyword),
  )
})

type TabKey = 'normal' | 'variant' | 'bozja' | 'eureka'

const tabs = computed(() => {
  const result: { key: TabKey; label: string }[] = []
  const list = filteredRoots.value

  if (list.some((preset) => !preset.isVariant && !preset.isBozja && !preset.isEureka)) {
    result.push({ key: 'normal', label: '常规' })
  }
  if (list.some((preset) => preset.isVariant)) result.push({ key: 'variant', label: '下水道' })
  if (list.some((preset) => preset.isBozja)) result.push({ key: 'bozja', label: '博兹雅' })
  if (list.some((preset) => preset.isEureka)) result.push({ key: 'eureka', label: '优雷卡' })

  return result
})

function matchesTab(preset: PresetNode, tab: TabKey): boolean {
  switch (tab) {
    case 'variant':
      return preset.isVariant
    case 'bozja':
      return preset.isBozja
    case 'eureka':
      return preset.isEureka
    default:
      return !preset.isVariant && !preset.isBozja && !preset.isEureka
  }
}

const visibleRoots = computed(() => filteredRoots.value.filter((preset) => matchesTab(preset, activeTab.value as TabKey)))

const currentJob = computed(() => meta.jobByName.get(selectedJob.value) ?? null)

watch(
  jobList,
  (list) => {
    if (list.length === 0) {
      selectedJob.value = ''
      return
    }
    if (!list.some((job) => job.name === selectedJob.value)) {
      selectedJob.value = list[0].name
    }
  },
  { immediate: true },
)

watch(tabs, (list) => {
  if (list.length === 0) return
  if (!list.some((tab) => tab.key === activeTab.value)) {
    activeTab.value = list[0].key
  }
})

function onToggled(id: string, enabled: boolean, forcedDisabled: string[]): void {
  meta.applyToggle(id, enabled, forcedDisabled)
}
</script>

<template>
  <div class="browser">
    <aside class="browser-jobs">
      <NInput
        v-model:value="jobSearch"
        class="job-search"
        placeholder="搜索职业"
        clearable
        size="small"
      />

      <div class="job-list sloth-scroll">
        <button
          v-for="job in filteredJobs"
          :key="job.name"
          type="button"
          class="job-item"
          :class="{ 'job-item-active': job.name === selectedJob }"
          @click="selectedJob = job.name"
        >
          <JobIcon :icon-id="job.iconId" :size="28" />
          <span class="job-text">
            <span class="job-name sloth-nowrap">
              {{ job.name }}
              <span v-if="job.shorthand" class="sloth-muted">· {{ job.shorthand }}</span>
            </span>
            <span class="job-meta sloth-muted">
              <span v-if="job.version">v{{ job.version }}</span>
              <span v-if="job.disabled" class="job-disabled">因更新而禁用</span>
            </span>
          </span>
        </button>

        <NEmpty v-if="filteredJobs.length === 0" description="没有匹配的职业" size="small" />
      </div>
    </aside>

    <section class="browser-main sloth-scroll">
      <header class="browser-head">
        <div class="browser-title">
          <JobIcon v-if="currentJob" :icon-id="currentJob.iconId" :size="30" />
          <h2 class="browser-heading">
            {{ currentJob?.name ?? (isPvp ? 'PvP 功能' : 'PvE 功能') }}
          </h2>
        </div>

        <NInput
          v-model:value="featureSearch"
          class="feature-search"
          placeholder="在本职业内搜索功能"
          clearable
          size="small"
        />
      </header>

      <p v-if="currentJob?.notice" class="browser-notice">{{ currentJob.notice }}</p>

      <NSpin v-if="!meta.loaded" size="small" />

      <NEmpty
        v-else-if="jobList.length === 0"
        :description="isPvp ? '当前没有 PvP 功能' : '当前没有 PvE 功能'"
      />

      <NTabs v-else v-model:value="activeTab" type="line" animated class="browser-tabs">
        <NTabPane v-for="tab in tabs" :key="tab.key" :name="tab.key" :tab="tab.label">
          <NEmpty v-if="visibleRoots.length === 0" description="没有匹配的功能" size="small" />
          <PresetCard
            v-for="preset in visibleRoots"
            :key="preset.id"
            :preset="preset"
            :children="meta.childrenOf(preset.id)"
            @toggled="onToggled"
          />
        </NTabPane>
      </NTabs>
    </section>
  </div>
</template>

<style scoped>
.browser {
  display: flex;
  height: 100%;
  min-height: 0;
  gap: 16px;
}

.browser-jobs {
  flex: 0 0 250px;
  min-width: 0;
  display: flex;
  flex-direction: column;
  gap: 8px;
  padding-right: 12px;
  border-right: 1px solid var(--sloth-border-soft);
  min-height: 0;
}

.job-search {
  flex: 0 0 auto;
}

.job-list {
  flex: 1 1 auto;
  min-height: 0;
  display: flex;
  flex-direction: column;
  gap: 2px;
  padding-right: 4px;
}

.job-item {
  display: flex;
  align-items: center;
  gap: 10px;
  width: 100%;
  padding: 6px 8px;
  border: 1px solid transparent;
  border-radius: var(--sloth-radius);
  background: transparent;
  color: inherit;
  font: inherit;
  text-align: left;
  cursor: pointer;
}

.job-item:hover {
  background: var(--sloth-bg-soft);
}

.job-item-active {
  background: var(--sloth-accent-soft);
  border-color: var(--sloth-accent);
}

.job-text {
  display: flex;
  flex-direction: column;
  min-width: 0;
  flex: 1 1 auto;
}

.job-name {
  font-size: 13px;
}

.job-meta {
  font-size: 11px;
  display: flex;
  gap: 8px;
}

.job-disabled {
  color: var(--sloth-warning);
}

.browser-main {
  flex: 1 1 auto;
  min-width: 0;
  padding-right: 8px;
}

.browser-head {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 16px;
  padding: 4px 0 8px;
}

.browser-title {
  display: flex;
  align-items: center;
  gap: 10px;
}

.browser-heading {
  margin: 0;
  font-size: 18px;
}

.feature-search {
  flex: 0 0 240px;
}

.browser-notice {
  margin: 0 0 8px;
  padding: 8px 12px;
  border-radius: var(--sloth-radius);
  background: var(--sloth-accent-soft);
  color: var(--sloth-text);
  font-size: 12px;
  white-space: pre-wrap;
}

.browser-tabs {
  margin-top: 4px;
}
</style>
