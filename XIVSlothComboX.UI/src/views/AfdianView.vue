<script setup lang="ts">
import { NButton } from 'naive-ui'

const notice =
  '如果你认可我的工作, 可以给我买杯蜜雪冰城，每一份支持都是在传递善意\n注: 捐赠均为【无偿】性质, 我无法因为捐赠给出任何承诺或回报, 请务必三思而后行'

function open(url: string): void {
  window.open(url, '_blank', 'noopener,noreferrer')
}
</script>

<template>
  <div class="afdian sloth-scroll">
    <p
      v-for="index in 3"
      :key="index"
      class="afdian-notice"
      :class="index === 2 ? 'afdian-notice-warn' : ''"
    >
      {{ notice }}
    </p>

    <NButton type="primary" @click="open('https://afdian.com/a/a_44451516')">爱发电</NButton>
  </div>
</template>

<style scoped>
.afdian {
  max-width: 760px;
}

.afdian-notice {
  margin: 8px 0;
  white-space: pre-wrap;
  color: var(--sloth-ok);
}

.afdian-notice-warn {
  color: var(--sloth-danger);
}
</style>
