<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { iconAssetUrl, iconHosts, iconUrl } from '@/api/icons'

const props = withDefaults(
  defineProps<{
    iconId: number
    size?: number
    hd?: boolean
  }>(),
  { size: 24, hd: false },
)

/**
 * The places the picture is taken from, in the order they are tried.
 *
 * The first one is the asset service of the international host, which hands
 * the picture of the game files out as a png. The ones after it are the hosts
 * that serve the pictures of the game data, which are used when the service
 * cannot be reached.
 */
const sources = computed(() => {
  const asset = iconAssetUrl(props.iconId, props.hd)
  return asset ? [asset, ...iconHosts.map((host) => iconUrl(props.iconId, props.hd, host))] : []
})

/** The source that is tried at the moment. */
const current = ref(0)

/** Set once every source has failed, so that the placeholder is shown. */
const failed = ref(false)

watch(
  () => props.iconId,
  () => {
    current.value = 0
    failed.value = false
  },
)

const source = computed(() => (failed.value ? '' : (sources.value[current.value] ?? '')))

/** Moves on to the next source when the picture cannot be loaded. */
function onError() {
  if (current.value + 1 < sources.value.length) {
    current.value += 1
    return
  }

  failed.value = true
}
</script>

<template>
  <img
    v-if="source && !failed"
    class="sloth-icon"
    :src="source"
    :style="{ width: `${size}px`, height: `${size}px` }"
    alt=""
    loading="lazy"
    referrerpolicy="no-referrer"
    @error="onError"
  />
  <span v-else class="sloth-icon sloth-icon-fallback" :style="{ width: `${size}px`, height: `${size}px` }" />
</template>

<style scoped>
.sloth-icon-fallback {
  display: inline-block;
  border-radius: 4px;
  background: var(--sloth-bg-soft);
  vertical-align: middle;
}
</style>
