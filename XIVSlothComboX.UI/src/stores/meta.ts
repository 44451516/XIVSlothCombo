import { computed, ref } from 'vue'
import { defineStore } from 'pinia'
import { api, ApiError } from '@/api/client'
import type { JobInfo, PresetNode } from '@/api/types'

/**
 * Holds the whole preset tree of the plugin. The tree is fetched once and then
 * kept in memory; enabling or disabling a feature only patches the affected node.
 */
export const useMetaStore = defineStore('meta', () => {
  const version = ref('')
  const port = ref(0)
  const jobs = ref<JobInfo[]>([])
  const presets = ref<PresetNode[]>([])
  const debug = ref(false)
  const loaded = ref(false)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const byId = computed(() => {
    const map = new Map<string, PresetNode>()
    for (const preset of presets.value) map.set(preset.id, preset)
    return map
  })

  /** Whether the feature itself, or one of its parents, belongs to PvP. */
  function isPvP(id: string): boolean {
    const visited = new Set<string>()
    let current = byId.value.get(id)

    while (current) {
      if (current.isPvP) return true
      if (!current.parentId || visited.has(current.id)) return false
      visited.add(current.id)
      current = byId.value.get(current.parentId)
    }

    return false
  }

  const pvePresets = computed(() => presets.value.filter((preset) => !isPvP(preset.id)))
  const pvpPresets = computed(() => presets.value.filter((preset) => isPvP(preset.id)))

  /** Groups the features by the job they belong to, keeping the server side order. */
  function groupByJob(list: PresetNode[]): Map<string, PresetNode[]> {
    const groups = new Map<string, PresetNode[]>()
    for (const preset of list) {
      const bucket = groups.get(preset.jobName)
      if (bucket) bucket.push(preset)
      else groups.set(preset.jobName, [preset])
    }
    return groups
  }

  const pveGroups = computed(() => groupByJob(pvePresets.value))
  const pvpGroups = computed(() => groupByJob(pvpPresets.value))

  const jobByName = computed(() => {
    const map = new Map<string, JobInfo>()
    for (const job of jobs.value) map.set(job.name, job)
    return map
  })

  /** Direct children of a feature. */
  function childrenOf(id: string): PresetNode[] {
    return presets.value.filter((preset) => preset.parentId === id)
  }

  /** Human readable name of a feature, including its parent chain. */
  function describe(id: string): string {
    const chain: string[] = []
    let current = byId.value.get(id)
    const visited = new Set<string>()

    while (current && !visited.has(current.id)) {
      visited.add(current.id)
      chain.unshift(current.fancyName || current.id)
      current = current.parentId ? byId.value.get(current.parentId) : undefined
    }

    return chain.join(' -> ')
  }

  async function load(force = false): Promise<void> {
    if (loading.value) return
    if (loaded.value && !force) return

    loading.value = true
    error.value = null
    try {
      const meta = await api.meta()
      version.value = meta.version
      port.value = meta.port
      jobs.value = meta.jobs
      presets.value = meta.presets
      debug.value = meta.debug ?? false
      loaded.value = true
    } catch (ex) {
      error.value = ex instanceof ApiError ? ex.message : String(ex)
      throw ex
    } finally {
      loading.value = false
    }
  }

  /** Applies the result of an enable or disable request to the local tree. */
  function applyToggle(id: string, enabled: boolean, forcedDisabled: string[]): void {
    const preset = byId.value.get(id)
    if (preset) preset.enabled = enabled

    for (const forced of forcedDisabled) {
      const other = byId.value.get(forced)
      if (other) other.enabled = false
    }
  }

  /** Refreshes the enabled state of a single feature without reloading the whole tree. */
  function refreshEnabled(id: string): void {
    const preset = byId.value.get(id)
    if (preset) preset.enabled = !preset.enabled
  }

  return {
    version,
    port,
    jobs,
    presets,
    debug,
    loaded,
    loading,
    error,
    byId,
    pvePresets,
    pvpPresets,
    pveGroups,
    pvpGroups,
    jobByName,
    isPvP,
    childrenOf,
    describe,
    load,
    applyToggle,
    refreshEnabled,
  }
})
