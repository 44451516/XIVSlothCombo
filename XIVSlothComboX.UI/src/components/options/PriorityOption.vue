<script setup lang="ts">
import { computed } from 'vue'
import { NInputNumber, NTooltip } from 'naive-ui'
import FieldRow from '../FieldRow.vue'
import type { OptionSchema } from '@/api/types'

const props = defineProps<{ option: OptionSchema }>()
const emit = defineEmits<{ change: [value: number[]] }>()

const choices = computed(() => props.option.options ?? [])

const stored = computed<number[]>(() => {
  const raw = props.option.value
  return Array.isArray(raw) ? (raw as number[]).map((entry) => Number(entry)) : []
})

/**
 * Only the slots owned by the current feature carry a label. The remaining slots of
 * the shared list stay anonymous, exactly like in the legacy window.
 */
const rows = computed(() =>
  choices.value
    .map((choice, index) => ({
      index,
      label: choice.label,
      value: stored.value[index] ?? Number(choice.value),
    }))
    .filter((row) => row.label !== ''),
)

/**
 * Applies the same rule as the legacy window: a priority that is already taken by
 * another entry swaps the two entries.
 */
function change(index: number, next: number): void {
  const array = stored.value.slice()
  const oldValue = array[index]

  if (oldValue === next) return

  for (let i = 0; i < array.length; i++) {
    if (i === index) continue
    if (array[i] === next) {
      array[i] = oldValue
      array[index] = next
      emit('change', array)
      return
    }
  }

  array[index] = next
  emit('change', array)
}
</script>

<template>
  <FieldRow :label="option.label || '优先级'" :tooltip="option.tooltip">
    <div class="priority-wrap">
      <div v-for="row in rows" :key="row.index" class="priority-row">
        <span class="priority-label sloth-nowrap">{{ row.label }}</span>
        <NInputNumber
          class="priority-input"
          :value="row.value"
          size="small"
          :show-button="false"
          @update:value="(next: number | null) => change(row.index, Number(next ?? 0))"
        />
      </div>
      <NTooltip v-if="rows.length === 0" trigger="hover">
        <template #trigger>
          <span class="sloth-muted">该功能的优先级由其它功能共享，请前往对应功能调整。</span>
        </template>
        优先级列表在多个功能之间共享，每个功能只能调整属于自己的那一项。
      </NTooltip>
    </div>
  </FieldRow>
</template>

<style scoped>
.priority-wrap {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.priority-row {
  display: flex;
  align-items: center;
  gap: 12px;
}

.priority-label {
  flex: 1 1 auto;
  min-width: 0;
}

.priority-input {
  flex: 0 0 88px;
}
</style>
