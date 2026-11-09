<script setup lang="ts">
import { computed } from 'vue'
import type { OptionChangePayload, RenderableOption } from '@/api/options'
import type { ValueType } from '@/api/types'
import DragOption from './options/DragOption.vue'
import MultiOption from './options/MultiOption.vue'
import PriorityOption from './options/PriorityOption.vue'
import RadioOption from './options/RadioOption.vue'
import SingleOption from './options/SingleOption.vue'
import SliderOption from './options/SliderOption.vue'
import SwitchOption from './options/SwitchOption.vue'

const props = defineProps<{ item: RenderableOption }>()
const emit = defineEmits<{ change: [payload: OptionChangePayload] }>()

const valueType = computed<ValueType>(() => props.item.option.valueType ?? 'int')

function report(value: unknown, reload = false): void {
  emit('change', {
    key: props.item.option.key,
    valueType: valueType.value,
    value,
    reload,
  })
}
</script>

<template>
  <SliderOption
    v-if="item.kind === 'slider'"
    :option="item.option"
    @change="(value: number) => report(value)"
  />
  <DragOption
    v-else-if="item.kind === 'drag'"
    :option="item.option"
    @change="(value: number) => report(value)"
  />
  <SwitchOption
    v-else-if="item.kind === 'switch'"
    :option="item.option"
    @change="(value: boolean) => report(value, true)"
  />
  <RadioOption
    v-else-if="item.kind === 'radio'"
    :option="item.option"
    :members="item.members"
    @change="(value: number) => report(value)"
  />
  <MultiOption
    v-else-if="item.kind === 'multi'"
    :option="item.option"
    @change="(value: boolean[]) => report(value)"
  />
  <SingleOption
    v-else-if="item.kind === 'single'"
    :option="item.option"
    @change="(value: number) => report(value)"
  />
  <PriorityOption
    v-else-if="item.kind === 'priority'"
    :option="item.option"
    @change="(value: number[]) => report(value)"
  />
</template>
