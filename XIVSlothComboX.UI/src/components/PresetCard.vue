<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { NButton, NCollapseTransition, NSpin, NSwitch, NTag } from 'naive-ui'
import { api } from '@/api/client'
import { groupOptions, type OptionChangePayload } from '@/api/options'
import type { OptionSchema, PresetNode } from '@/api/types'
import { toastError, toastInfo } from '@/composables/useToast'
import { useConfigStore } from '@/stores/config'
import OptionRenderer from './OptionRenderer.vue'
import PresetDetailPanel from './PresetDetailPanel.vue'

const props = withDefaults(
  defineProps<{
    preset: PresetNode
    /** Children of this feature; they are rendered as a nested tree. */
    children?: PresetNode[]
    depth?: number
  }>(),
  { children: () => [], depth: 0 },
)

const emit = defineEmits<{
  toggled: [id: string, enabled: boolean, forcedDisabled: string[]]
}>()

const configStore = useConfigStore()

const expanded = ref(false)
const options = ref<OptionSchema[]>([])
const loading = ref(false)
const saving = ref(false)
const toggling = ref(false)

/**
 * The legacy window could hide the options of a disabled feature. The setting is
 * still part of the configuration, so it keeps working here.
 */
const showOptions = computed(
  () => !(configStore.config?.hideChildren === true && !props.preset.enabled),
)

const renderable = computed(() => groupOptions(options.value))
const hasChildren = computed(() => props.children.length > 0)

async function loadOptions(): Promise<void> {
  loading.value = true
  try {
    const result = await api.presetOptions(props.preset.id)
    options.value = result.options ?? []
  } catch (error) {
    toastError(error instanceof Error ? error.message : String(error))
  } finally {
    loading.value = false
  }
}

watch(
  expanded,
  (open) => {
    if (open && options.value.length === 0 && !loading.value) void loadOptions()
  },
)

async function onToggle(enabled: boolean): Promise<void> {
  toggling.value = true
  try {
    const result = await api.togglePreset(props.preset.id, enabled)
    emit('toggled', props.preset.id, result.enabled, result.forcedDisabled ?? [])

    if (result.forcedDisabled?.length) {
      toastInfo(`已自动关闭与该功能冲突的功能：${result.forcedDisabled.join('、')}`)
    }

    // The options of a feature may depend on the state of its parents.
    if (options.value.length > 0) await loadOptions()
  } catch (error) {
    toastError(error instanceof Error ? error.message : String(error))
  } finally {
    toggling.value = false
  }
}

/** Writes the new value into the local copy so the control keeps showing it. */
function applyLocalValue(payload: OptionChangePayload): void {
  for (const option of options.value) {
    if (option.key !== payload.key) continue
    option.value = payload.value
  }
}

async function onChange(payload: OptionChangePayload): Promise<void> {
  saving.value = true
  try {
    await api.setValue(payload.key, payload.valueType, payload.value)
    applyLocalValue(payload)

    // A switch can reveal further options, so the schema has to be rebuilt.
    if (payload.reload) await loadOptions()
  } catch (error) {
    toastError(error instanceof Error ? error.message : String(error))
    await loadOptions()
  } finally {
    saving.value = false
  }
}
</script>

<template>
  <div class="preset-card" :class="{ 'preset-hidden': preset.hidden }" :style="{ marginLeft: `${depth * 18}px` }">
    <div class="preset-head">
      <NSwitch
        class="preset-switch"
        :value="preset.enabled"
        :loading="toggling"
        :disabled="preset.hidden"
        @update:value="(value: boolean) => onToggle(value)"
      />

      <div class="preset-title" @click="expanded = !expanded">
        <div class="preset-name-row">
          <span class="preset-name">{{ preset.fancyName }}</span>
          <NTag v-if="preset.hidden" size="small" type="warning" :bordered="false">
            与已启用的功能冲突
          </NTag>
          <NTag v-if="preset.isVariant" size="small" :bordered="false">下水道</NTag>
          <NTag v-if="preset.isBozja" size="small" :bordered="false">博兹雅</NTag>
          <NTag v-if="preset.isEureka" size="small" :bordered="false">优雷卡</NTag>
        </div>
        <div v-if="preset.description" class="preset-desc sloth-muted">{{ preset.description }}</div>
      </div>

      <NButton quaternary size="small" @click="expanded = !expanded">
        {{ expanded ? '收起' : '选项' }}
      </NButton>
    </div>

    <NCollapseTransition>
      <div v-if="expanded && showOptions" class="preset-body">
        <NSpin v-if="loading" size="small" />
        <template v-else>
          <div v-if="renderable.length === 0" class="sloth-muted preset-empty">
            该功能没有可配置的选项。
          </div>
          <div v-for="item in renderable" :key="`${item.kind}-${item.option.key}-${item.option.order}`" class="preset-option">
            <OptionRenderer :item="item" @change="onChange" />
          </div>
          <PresetDetailPanel :preset-id="preset.id" />
        </template>
      </div>
    </NCollapseTransition>

    <div v-if="hasChildren" class="preset-children">
      <PresetCard
        v-for="child in children"
        :key="child.id"
        :preset="child"
        :children="[]"
        :depth="depth + 1"
        @toggled="(id, enabled, forced) => emit('toggled', id, enabled, forced)"
      />
    </div>
  </div>
</template>

<style scoped>
.preset-card {
  background: var(--sloth-panel);
  border: 1px solid var(--sloth-border-soft);
  border-radius: var(--sloth-radius);
  padding: 10px 14px;
  margin-bottom: 8px;
}

.preset-hidden {
  opacity: 0.6;
}

.preset-head {
  display: flex;
  align-items: flex-start;
  gap: 12px;
}

.preset-switch {
  flex: 0 0 auto;
  margin-top: 4px;
}

.preset-title {
  flex: 1 1 auto;
  min-width: 0;
  cursor: pointer;
}

.preset-name-row {
  display: flex;
  align-items: center;
  gap: 8px;
  flex-wrap: wrap;
}

.preset-name {
  font-weight: 600;
}

.preset-desc {
  font-size: 12px;
  margin-top: 2px;
}

.preset-body {
  margin-top: 6px;
  padding-top: 4px;
  border-top: 1px dashed var(--sloth-border-soft);
}

.preset-empty {
  padding: 8px 0;
  font-size: 13px;
}

.preset-option + .preset-option {
  border-top: 1px solid rgba(255, 255, 255, 0.03);
}

.preset-children {
  margin-top: 8px;
  padding-left: 4px;
  border-left: 2px solid var(--sloth-border-soft);
}
</style>
