<script setup lang="ts">
import { computed } from 'vue'
import { NSwitch } from 'naive-ui'
import FieldRow from '../FieldRow.vue'
import type { OptionSchema } from '@/api/types'

const props = defineProps<{ option: OptionSchema }>()
const emit = defineEmits<{ change: [value: boolean] }>()

const value = computed(() => props.option.value === true)
</script>

<template>
  <FieldRow
    :label="option.label"
    :tooltip="option.tooltip"
    :conditional="option.conditional"
  >
    <NSwitch
      :value="value"
      @update:value="(next: boolean) => emit('change', next)"
    />
  </FieldRow>
</template>
