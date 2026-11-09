<script setup lang="ts">
import { computed } from 'vue'
import { NSlider } from 'naive-ui'
import FieldRow from '../FieldRow.vue'
import type { OptionSchema } from '@/api/types'

const props = defineProps<{ option: OptionSchema }>()
const emit = defineEmits<{ change: [value: number] }>()

const value = computed(() => Number(props.option.value ?? 0))
const min = computed(() => props.option.min ?? 0)
const max = computed(() => props.option.max ?? 100)
const step = computed(() => props.option.step ?? 1)
const digits = computed(() => props.option.digits ?? 0)

function format(input: number): string {
  return digits.value > 0 ? input.toFixed(digits.value) : String(input)
}
</script>

<template>
  <FieldRow :label="option.label" :tooltip="option.tooltip">
    <div class="slider-wrap">
      <NSlider
        class="slider"
        :value="value"
        :min="min"
        :max="max"
        :step="step"
        :format-tooltip="format"
        @update:value="(next: number) => emit('change', next)"
      />
      <span class="slider-value">{{ format(value) }}</span>
    </div>
  </FieldRow>
</template>

<style scoped>
.slider-wrap {
  display: flex;
  align-items: center;
  gap: 12px;
}

.slider {
  flex: 1 1 auto;
}

.slider-value {
  flex: 0 0 64px;
  text-align: right;
  font-variant-numeric: tabular-nums;
  color: var(--sloth-text-dim);
}
</style>
