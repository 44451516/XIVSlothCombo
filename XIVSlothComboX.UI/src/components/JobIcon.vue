<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { iconHosts, iconUrl } from '@/api/icons'

const props = withDefaults(
  defineProps<{
    iconId: number
    size?: number
    hd?: boolean
  }>(),
  { size: 24, hd: false },
)

/** The host the picture is taken from. It moves on when a host cannot be reached. */
const host = ref(iconHosts[0])

/** Set once every host has failed, so that the placeholder is shown. */
const failed = ref(false)

watch(
  () => props.iconId,
  () => {
    host.value = iconHosts[0]
    failed.value = false
  },
)

const source = computed(() =>
  props.iconId > 0 && !failed.value ? iconUrl(props.iconId, props.hd, host.value) : '',
)

/** Moves on to the next host when the picture cannot be loaded. */
function onError() {
  const next = iconHosts.indexOf(host.value) + 1
  if (next < iconHosts.length) {
    host.value = iconHosts[next]
    return
  }

  failed.value = true
}
</script>

<template>
  <img
    v-if="source"
    class="sloth-icon"
    :src="source"
    :style="{ width: `${size}px`, height: `${size}px` }"
    alt=""
    loading="lazy"
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
