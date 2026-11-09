<script setup lang="ts">
import { computed } from 'vue'
import { NInputNumber, NSlider } from 'naive-ui'
import FieldRow from '../FieldRow.vue'
import type { OptionSchema } from '@/api/types'

const props = defineProps<{ option: OptionSchema }>()
const emit = defineEmits<{ change: [value: number] }>()

const value = computed(() => Number(props.option.value ?? 0))
const min = computed(() => props.option.min ?? 0)
const max = computed(() => props.option.max ?? 100)
const step = computed(() => {
  if (props.option.step !== null && props.option.step !== undefined) return props.option.step
  return (props.option.digits ?? 0) > 0 ? 0.01 : 1
})
const digits = computed(() => props.option.digits ?? 0)

function format(input: number): string {
  return digits.value > 0 ? input.toFixed(digits.value) : String(input)
}
</script>

<template>
  <FieldRow :label="option.label" :tooltip="option.tooltip">
    <div class="drag-wrap">
      <NSlider
        class="drag-slider"
        :value="value"
        :min="min"
        :max="max"
        :step="step"
        :format-tooltip="format"
        @update:value="(next: number) => emit('change', next)"
      />
      <NInputNumber
        class="drag-input"
        :value="value"
        :min="min"
        :max="max"
        :step="step"
        size="small"
        :show-button="false"
        @update:value="(next: number | null) => emit('change', Number(next ?? 0))"
      />
    </div>
  </FieldRow>
</template>

<style scoped>
.drag-wrap {
  display: flex;
  align-items: center;
  gap: 12px;
}

.drag-slider {
  flex: 1 1 auto;
}

.drag-input {
  flex: 0 0 96px;
}
</style>
