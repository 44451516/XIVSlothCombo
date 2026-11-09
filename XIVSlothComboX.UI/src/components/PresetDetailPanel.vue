<script setup lang="ts">
import { ref, watch } from 'vue'
import { NSpin, NTag } from 'naive-ui'
import { api } from '@/api/client'
import type { PresetDetail } from '@/api/types'
import { toastError } from '@/composables/useToast'
import JobIcon from './JobIcon.vue'

const props = defineProps<{ presetId: string }>()

const detail = ref<PresetDetail | null>(null)
const loading = ref(false)

watch(
  () => props.presetId,
  async (id) => {
    if (!id) return
    loading.value = true
    try {
      const result = await api.preset(id)
      detail.value = result.detail
    } catch (error) {
      toastError(error instanceof Error ? error.message : String(error))
    } finally {
      loading.value = false
    }
  },
  { immediate: true },
)
</script>

<template>
  <div class="detail">
    <NSpin v-if="loading" size="small" />
    <template v-else-if="detail">
      <div v-if="detail.parentChain.length" class="detail-row">
        <span class="detail-key">所属功能</span>
        <span class="detail-value">{{ detail.parentChain.join(' → ') }}</span>
      </div>

      <div v-if="detail.conflictNames.length" class="detail-row">
        <span class="detail-key">冲突功能</span>
        <span class="detail-value">
          <NTag
            v-for="name in detail.conflictNames"
            :key="name"
            size="small"
            type="warning"
            :bordered="false"
            class="detail-tag"
          >
            {{ name }}
          </NTag>
        </span>
      </div>

      <div v-if="detail.replaces.length" class="detail-row">
        <span class="detail-key">替换技能</span>
        <span class="detail-value detail-replaces">
          <span v-for="(name, index) in detail.replaces" :key="name" class="detail-replace">
            <JobIcon v-if="detail.replaceIcons[index]" :icon-id="detail.replaceIcons[index]" :size="20" />
            <span>{{ name }}</span>
          </span>
        </span>
      </div>

      <div v-if="detail.variantParents.length" class="detail-row">
        <span class="detail-key">下水道来源</span>
        <span class="detail-value">{{ detail.variantParents.join('、') }}</span>
      </div>

      <div v-if="detail.bozjaParents.length" class="detail-row">
        <span class="detail-key">博兹雅来源</span>
        <span class="detail-value">{{ detail.bozjaParents.join('、') }}</span>
      </div>

      <div v-if="detail.eurekaParents.length" class="detail-row">
        <span class="detail-key">优雷卡来源</span>
        <span class="detail-value">{{ detail.eurekaParents.join('、') }}</span>
      </div>

      <div v-if="detail.bluStatus === 'missing'" class="detail-row">
        <span class="detail-key">缺少蓝魔技能</span>
        <span class="detail-value">{{ detail.bluMissing }}</span>
      </div>
      <div v-else-if="detail.bluStatus === 'ready'" class="detail-row">
        <span class="detail-key">蓝魔技能</span>
        <span class="detail-value">已就绪</span>
      </div>
    </template>
  </div>
</template>

<style scoped>
.detail {
  padding: 8px 0 4px;
  border-top: 1px dashed var(--sloth-border-soft);
  margin-top: 6px;
}

.detail-row {
  display: flex;
  gap: 12px;
  padding: 3px 0;
  font-size: 12px;
}

.detail-key {
  flex: 0 0 96px;
  color: var(--sloth-text-dim);
}

.detail-value {
  flex: 1 1 auto;
  min-width: 0;
  overflow-wrap: anywhere;
}

.detail-tag {
  margin-right: 6px;
}

.detail-replaces {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}

.detail-replace {
  display: inline-flex;
  align-items: center;
  gap: 4px;
}
</style>
