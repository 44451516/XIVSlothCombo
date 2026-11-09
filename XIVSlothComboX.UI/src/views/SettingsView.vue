<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import {
  NCard,
  NColorPicker,
  NInputNumber,
  NSelect,
  NSwitch,
  type SelectOption,
} from 'naive-ui'
import { useConfigStore } from '@/stores/config'
import type { ColorDto, ConfigDto } from '@/api/types'
import { toastError } from '@/composables/useToast'

const store = useConfigStore()
const saving = ref(false)

onMounted(async () => {
  try {
    await store.load()
  } catch (error) {
    toastError(error instanceof Error ? error.message : String(error))
  }
})

async function patch(payload: Partial<ConfigDto>): Promise<void> {
  saving.value = true
  try {
    await store.patch(payload)
  } catch (error) {
    toastError(error instanceof Error ? error.message : String(error))
  } finally {
    saving.value = false
  }
}

/** Builds the colour string the picker works with. */
function toColorString(color: ColorDto): string {
  const r = Math.round(color.r * 255)
  const g = Math.round(color.g * 255)
  const b = Math.round(color.b * 255)

  if (color.a >= 0.999) {
    return `#${[r, g, b].map((part) => part.toString(16).padStart(2, '0')).join('')}`
  }

  return `rgba(${r}, ${g}, ${b}, ${color.a})`
}

/** Reads a colour string back into the components the plugin stores. */
function parseColor(value: string): ColorDto | null {
  const text = value.trim()

  if (text.startsWith('#')) {
    const hex = text.slice(1)
    const expanded =
      hex.length === 3 || hex.length === 4 ? hex.split('').map((part) => part + part).join('') : hex
    if (expanded.length < 6) return null

    return {
      r: parseInt(expanded.slice(0, 2), 16) / 255,
      g: parseInt(expanded.slice(2, 4), 16) / 255,
      b: parseInt(expanded.slice(4, 6), 16) / 255,
      a: expanded.length >= 8 ? parseInt(expanded.slice(6, 8), 16) / 255 : 1,
    }
  }

  const match = text.match(/^rgba?\(([^)]+)\)$/i)
  if (!match) return null

  const parts = match[1].split(',').map((part) => Number(part.trim()))
  if (parts.length < 3 || parts.some((part) => Number.isNaN(part))) return null

  return {
    r: parts[0] / 255,
    g: parts[1] / 255,
    b: parts[2] / 255,
    a: parts.length > 3 ? parts[3] : 1,
  }
}

const colorValue = computed(() => {
  const color = store.config?.targetHighlightColor
  return color ? toColorString(color) : '#ffffff'
})

function onColorChange(value: string): void {
  const color = parseColor(value)
  if (!color) return
  void patch({ targetHighlightColor: color })
}

/** The food is encoded as `id * 2 + hq`, so that both qualities stay selectable. */
const foodValue = computed(() => {
  const config = store.config
  if (!config || config.autoFoodId === 0) return 0
  return config.autoFoodId * 2 + (config.requiredFoodHq ? 1 : 0)
})

const foodOptions = computed<SelectOption[]>(() => {
  const foods = store.config?.foods ?? []
  const options: SelectOption[] = [{ label: '禁用', value: 0 }]

  const regular = foods
    .filter((food) => food.inInventory)
    .map((food) => ({ label: food.name, value: food.id * 2 }))
  if (regular.length > 0) options.push({ label: '普通品质', options: regular })

  const highQuality = foods
    .filter((food) => food.inInventoryHq)
    .map((food) => ({ label: food.name, value: food.id * 2 + 1 }))
  if (highQuality.length > 0) options.push({ label: '高品质', options: highQuality })

  return options
})

function onFoodChange(value: number): void {
  if (value === 0) {
    void patch({ autoFoodId: 0, requiredFoodHq: false })
    return
  }

  void patch({
    autoFoodId: Math.floor(value / 2),
    requiredFoodHq: value % 2 === 1,
  })
}
</script>

<template>
  <div class="settings sloth-scroll">
    <NCard v-if="!store.config" title="综合设置" :bordered="false">加载中……</NCard>

    <template v-else>
      <NCard title="界面显示" :bordered="false" class="settings-card">
        <div class="settings-row">
          <div class="settings-label">
            隐藏子连击选项
            <span class="sloth-muted">（可能有 BUG）</span>
          </div>
          <NSwitch
            :value="store.config.hideChildren"
            :disabled="saving"
            @update:value="(value: boolean) => patch({ hideChildren: value })"
          />
        </div>

        <div class="settings-row">
          <div class="settings-label">
            隐藏冲突的连击
            <span class="sloth-muted">（可能有 BUG）</span>
          </div>
          <NSwitch
            :value="store.config.hideConflictedCombos"
            :disabled="saving"
            @update:value="(value: boolean) => patch({ hideConflictedCombos: value })"
          />
        </div>

        <div class="settings-row">
          <div class="settings-label">隐藏登录资讯</div>
          <NSwitch
            :value="store.config.hideMessageOfTheDay"
            :disabled="saving"
            @update:value="(value: boolean) => patch({ hideMessageOfTheDay: value })"
          />
        </div>

        <div class="settings-row">
          <div class="settings-label">特殊活动命名</div>
          <NSwitch
            :value="store.config.specialEvent"
            :disabled="saving"
            @update:value="(value: boolean) => patch({ specialEvent: value })"
          />
        </div>
      </NCard>

      <NCard title="输出与战斗" :bordered="false" class="settings-card">
        <div class="settings-row">
          <div class="settings-label">向聊天框输出日志</div>
          <NSwitch
            :value="store.config.enabledOutputLog"
            :disabled="saving"
            @update:value="(value: boolean) => patch({ enabledOutputLog: value })"
          />
        </div>

        <div class="settings-row">
          <div class="settings-label">状态变化输出到聊天</div>
          <NSwitch
            :value="store.config.setOutChat"
            :disabled="saving"
            @update:value="(value: boolean) => patch({ setOutChat: value })"
          />
        </div>

        <div class="settings-row">
          <div class="settings-label">近战距离偏移量</div>
          <NInputNumber
            class="settings-number"
            :value="store.config.meleeOffset"
            :disabled="saving"
            :show-button="false"
            @update:value="(value: number | null) => patch({ meleeOffset: Number(value ?? 0) })"
          />
        </div>

        <div class="settings-row">
          <div class="settings-label">移动延时</div>
          <NInputNumber
            class="settings-number"
            :value="store.config.movementLeeway"
            :disabled="saving"
            :show-button="false"
            @update:value="(value: number | null) => patch({ movementLeeway: Number(value ?? 0) })"
          />
        </div>

        <div class="settings-row">
          <div class="settings-label">目标高亮颜色</div>
          <NColorPicker
            class="settings-color"
            :value="colorValue"
            :show-alpha="true"
            :modes="['rgb']"
            :disabled="saving"
            @update:value="onColorChange"
          />
        </div>
      </NCard>

      <NCard title="自动消耗" :bordered="false" class="settings-card">
        <div class="settings-row">
          <div class="settings-label">自动精炼</div>
          <NSwitch
            :value="store.config.autoRefine"
            :disabled="saving"
            @update:value="(value: boolean) => patch({ autoRefine: value })"
          />
        </div>

        <div class="settings-row">
          <div class="settings-label">只精炼亚力山大</div>
          <NSwitch
            :value="store.config.onlyRefineAlexander"
            :disabled="saving"
            @update:value="(value: boolean) => patch({ onlyRefineAlexander: value })"
          />
        </div>

        <div class="settings-row">
          <div class="settings-label">自动精炼药</div>
          <NSwitch
            :value="store.config.autoRefinePotion"
            :disabled="saving"
            @update:value="(value: boolean) => patch({ autoRefinePotion: value })"
          />
        </div>

        <div class="settings-row">
          <div class="settings-label">自动食物</div>
          <NSwitch
            :value="store.config.autoFood"
            :disabled="saving"
            @update:value="(value: boolean) => patch({ autoFood: value })"
          />
        </div>

        <div class="settings-row">
          <div class="settings-label">消耗的食物</div>
          <NSelect
            class="settings-select"
            :value="foodValue"
            :options="foodOptions"
            :disabled="saving || !store.config.autoFood"
            @update:value="(value: number) => onFoodChange(value)"
          />
        </div>
      </NCard>
    </template>
  </div>
</template>

<style scoped>
.settings {
  max-width: 900px;
}

.settings-card {
  margin-bottom: 12px;
}

.settings-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 20px;
  padding: 8px 0;
}

.settings-label {
  flex: 1 1 auto;
  min-width: 0;
}

.settings-number {
  flex: 0 0 160px;
}

.settings-color {
  flex: 0 0 auto;
}

.settings-select {
  flex: 0 0 260px;
}
</style>
