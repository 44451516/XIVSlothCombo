<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { iconUrl } from '@/api/client'

const props = withDefaults(
  defineProps<{
    iconId: number
    size?: number
    hd?: boolean
  }>(),
  { size: 24, hd: false },
)

const source = computed(() => (props.iconId > 0 ? iconUrl(props.iconId, props.hd) : ''))
const failed = ref(false)

watch(source, () => {
  failed.value = false
})
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
    @error="failed = true"
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
