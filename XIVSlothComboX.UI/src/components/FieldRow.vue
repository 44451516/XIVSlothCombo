<script setup lang="ts">
import { NTag, NTooltip } from 'naive-ui'

withDefaults(
  defineProps<{
    label: string
    tooltip?: string | null
    /** Marks a switch that reveals further options when it is turned on. */
    conditional?: boolean
  }>(),
  { tooltip: null, conditional: false },
)
</script>

<template>
  <div class="field-row">
    <div class="field-label">
      <NTooltip v-if="tooltip" trigger="hover" placement="top">
        <template #trigger>
          <span class="field-label-text">{{ label }}</span>
        </template>
        <span class="field-tooltip">{{ tooltip }}</span>
      </NTooltip>
      <span v-else class="field-label-text">{{ label }}</span>
      <NTag v-if="conditional" size="small" type="info" :bordered="false">
        可展开更多
      </NTag>
    </div>
    <div class="field-control">
      <slot />
    </div>
  </div>
</template>

<style scoped>
.field-row {
  display: flex;
  align-items: flex-start;
  gap: 16px;
  padding: 7px 0;
}

.field-label {
  flex: 0 0 46%;
  min-width: 0;
  display: flex;
  align-items: center;
  gap: 8px;
  padding-top: 2px;
}

.field-label-text {
  overflow-wrap: anywhere;
}

.field-tooltip {
  white-space: pre-wrap;
  max-width: 320px;
  display: block;
}

.field-control {
  flex: 1 1 auto;
  min-width: 0;
}
</style>
