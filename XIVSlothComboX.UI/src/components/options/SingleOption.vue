<script setup lang="ts">
import { computed } from 'vue'
import { NRadio, NRadioGroup, NTooltip } from 'naive-ui'
import FieldRow from '../FieldRow.vue'
import type { OptionSchema } from '@/api/types'

const props = defineProps<{ option: OptionSchema }>()
const emit = defineEmits<{ change: [value: number] }>()

const choices = computed(() => props.option.options ?? [])
const current = computed(() => Number(props.option.value ?? 0))

const columns = computed(() => {
  switch (props.option.layout) {
    case 'role-grid':
      return 5
    case 'job-grid':
      return 5
    default:
      return 0
  }
})
</script>

<template>
  <FieldRow :label="option.label" :tooltip="option.tooltip">
    <NRadioGroup
      class="single-wrap"
      :style="columns > 0 ? { gridTemplateColumns: `repeat(${columns}, minmax(0, 1fr))` } : undefined"
      :value="current"
      @update:value="(next: number) => emit('change', next)"
    >
      <div v-for="choice in choices" :key="choice.value" class="single-item">
        <NRadio :value="Number(choice.value)">{{ choice.label }}</NRadio>
        <NTooltip v-if="choice.tooltip" trigger="hover">
          <template #trigger>
            <span class="single-info" aria-label="说明">?</span>
          </template>
          <span class="single-tooltip">{{ choice.tooltip }}</span>
        </NTooltip>
      </div>
    </NRadioGroup>
  </FieldRow>
</template>

<style scoped>
.single-wrap {
  display: grid;
  gap: 2px 12px;
}

.single-item {
  display: flex;
  align-items: center;
  gap: 4px;
}

.single-info {
  color: var(--sloth-text-dim);
  cursor: help;
  font-size: 12px;
}

.single-tooltip {
  white-space: pre-wrap;
  max-width: 320px;
  display: block;
}
</style>
