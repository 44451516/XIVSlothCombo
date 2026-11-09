<script setup lang="ts">
import { computed } from 'vue'
import { NCheckbox, NTooltip } from 'naive-ui'
import FieldRow from '../FieldRow.vue'
import type { OptionSchema } from '@/api/types'

const props = defineProps<{ option: OptionSchema }>()
const emit = defineEmits<{ change: [value: boolean[]] }>()

const choices = computed(() => props.option.options ?? [])

const stored = computed<boolean[]>(() => {
  const raw = props.option.value
  return Array.isArray(raw) ? (raw as boolean[]).map((entry) => entry === true) : []
})

const layout = computed(() => props.option.layout ?? 'horizontal')

const columns = computed(() => {
  if (layout.value.startsWith('grid-')) {
    const parsed = Number(layout.value.slice('grid-'.length))
    return Number.isFinite(parsed) && parsed > 0 ? parsed : 2
  }
  switch (layout.value) {
    case 'role-grid':
      return 5
    case 'job-grid':
      return 5
    case 'pvp-status':
      return 4
    default:
      return 0
  }
})

const horizontal = computed(() => layout.value === 'horizontal')

function isChecked(index: number): boolean {
  return stored.value[index] === true
}

function toggle(index: number, checked: boolean): void {
  const length = Math.max(choices.value.length, stored.value.length)
  const next = new Array<boolean>(length).fill(false)
  for (let i = 0; i < stored.value.length; i++) next[i] = stored.value[i]
  next[index] = checked
  emit('change', next)
}
</script>

<template>
  <FieldRow :label="option.label" :tooltip="option.tooltip">
    <div
      class="multi-wrap"
      :class="{ 'multi-horizontal': horizontal }"
      :style="columns > 0 ? { gridTemplateColumns: `repeat(${columns}, minmax(0, 1fr))` } : undefined"
    >
      <div v-for="(choice, index) in choices" :key="choice.value" class="multi-item">
        <NCheckbox
          :checked="isChecked(index)"
          @update:checked="(checked: boolean) => toggle(index, checked)"
        >
          {{ choice.label }}
        </NCheckbox>
        <NTooltip v-if="choice.tooltip" trigger="hover">
          <template #trigger>
            <span class="multi-info" aria-label="说明">?</span>
          </template>
          <span class="multi-tooltip">{{ choice.tooltip }}</span>
        </NTooltip>
      </div>
    </div>
  </FieldRow>
</template>

<style scoped>
.multi-wrap {
  display: grid;
  gap: 4px 12px;
}

.multi-horizontal {
  display: flex;
  flex-wrap: wrap;
  gap: 4px 20px;
}

.multi-item {
  display: flex;
  align-items: center;
  gap: 4px;
}

.multi-info {
  color: var(--sloth-text-dim);
  cursor: help;
  font-size: 12px;
}

.multi-tooltip {
  white-space: pre-wrap;
  max-width: 320px;
  display: block;
}
</style>
