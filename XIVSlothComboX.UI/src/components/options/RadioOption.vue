<script setup lang="ts">
import { computed } from 'vue'
import { NRadio, NRadioGroup, NTooltip } from 'naive-ui'
import FieldRow from '../FieldRow.vue'
import type { OptionSchema } from '@/api/types'

const props = defineProps<{ option: OptionSchema; members: OptionSchema[] }>()
const emit = defineEmits<{ change: [value: number] }>()

/** The radio entries all carry the current value of their group. */
const current = computed(() => Number(props.option.value ?? 0))
const horizontal = computed(() => props.members.some((member) => member.layout === 'horizontal'))
</script>

<template>
  <FieldRow :label="option.label" :tooltip="option.tooltip">
    <NRadioGroup
      class="radio-group"
      :class="{ 'radio-group-horizontal': horizontal }"
      :value="current"
      @update:value="(next: number) => emit('change', next)"
    >
      <div
        v-for="(member, index) in members"
        :key="member.optionValue ?? index"
        class="radio-item"
      >
        <NRadio :value="member.optionValue ?? 0">{{ member.label }}</NRadio>
        <NTooltip v-if="member.tooltip" trigger="hover">
          <template #trigger>
            <span class="radio-info" aria-label="说明">?</span>
          </template>
          <span class="radio-tooltip">{{ member.tooltip }}</span>
        </NTooltip>
      </div>
    </NRadioGroup>
  </FieldRow>
</template>

<style scoped>
.radio-group {
  display: flex;
  flex-direction: column;
  gap: 2px;
}

.radio-group-horizontal {
  flex-direction: row;
  flex-wrap: wrap;
  gap: 4px 20px;
}

.radio-item {
  display: flex;
  align-items: center;
  gap: 4px;
}

.radio-info {
  color: var(--sloth-text-dim);
  cursor: help;
}

.radio-tooltip {
  white-space: pre-wrap;
  max-width: 320px;
  display: block;
}
</style>
