import { ref } from 'vue'
import { defineStore } from 'pinia'
import { api, ApiError } from '@/api/client'
import type { CustomTypeDto, TimelineActionDto, TimelineDto } from '@/api/types'

/** Holds the custom timelines of the plugin and the working copy the editor edits. */
export const useTimelineStore = defineStore('timeline', () => {
  const timelines = ref<TimelineDto[]>([])
  const working = ref<TimelineActionDto[]>([])
  const current = ref<TimelineDto | null>(null)
  const useItem = ref(false)
  const customTypes = ref<CustomTypeDto[]>([])
  const customTypeSubs = ref<CustomTypeDto[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  function apply(payload: {
    timelines: TimelineDto[]
    working: TimelineDto[]
    current: TimelineDto | null
    useItem: boolean
    customTypes: CustomTypeDto[]
    customTypeSubs: CustomTypeDto[]
  }): void {
    timelines.value = payload.timelines
    working.value = payload.working.flatMap((timeline) => timeline.actionList)
    current.value = payload.current
    useItem.value = payload.useItem
    customTypes.value = payload.customTypes
    customTypeSubs.value = payload.customTypeSubs
  }

  async function load(force = false): Promise<void> {
    if (loading.value) return
    if (timelines.value.length > 0 && !force) return

    loading.value = true
    error.value = null
    try {
      apply(await api.timeline())
    } catch (ex) {
      error.value = ex instanceof ApiError ? ex.message : String(ex)
      throw ex
    } finally {
      loading.value = false
    }
  }

  /**
   * Sends an action to the plugin. The server answers with the state of the working
   * copy for the actions that change it.
   */
  async function run(action: string, payload: Record<string, unknown> = {}): Promise<Record<string, unknown>> {
    const result = await api.timelineAction(action, payload)
    if (action === 'setWorking') {
      // The server does not echo the working copy, so it is refreshed explicitly.
      await load(true)
    } else {
      await load(true)
    }
    return result
  }

  return {
    timelines,
    working,
    current,
    useItem,
    customTypes,
    customTypeSubs,
    loading,
    error,
    load,
    run,
  }
})
