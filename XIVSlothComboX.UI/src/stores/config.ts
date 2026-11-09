import { ref } from 'vue'
import { defineStore } from 'pinia'
import { api, ApiError } from '@/api/client'
import type { ConfigDto } from '@/api/types'

/** Holds the top level configuration of the plugin. */
export const useConfigStore = defineStore('config', () => {
  const config = ref<ConfigDto | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  async function load(force = false): Promise<void> {
    if (loading.value) return
    if (config.value && !force) return

    loading.value = true
    error.value = null
    try {
      config.value = await api.config()
    } catch (ex) {
      error.value = ex instanceof ApiError ? ex.message : String(ex)
      throw ex
    } finally {
      loading.value = false
    }
  }

  /** Applies a partial change and keeps the answer of the server as the new state. */
  async function patch(payload: Partial<ConfigDto>): Promise<void> {
    config.value = await api.patchConfig(payload)
  }

  return { config, loading, error, load, patch }
})
