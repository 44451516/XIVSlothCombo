<script setup lang="ts">
import { NButton } from 'naive-ui'
import { useMetaStore } from '@/stores/meta'

const meta = useMetaStore()

const ban = '[XIVSlothComboX]请不要去卫月频道交流此插件,包括提BUG'

function open(url: string): void {
  window.open(url, '_blank', 'noopener,noreferrer')
}
</script>

<template>
  <div class="about sloth-scroll">
    <h2 class="about-title">XIVSlothComboX{{ meta.version ? ` - v${meta.version}` : '' }}</h2>

    <div class="about-actions">
      <NButton type="primary" @click="open('https://docs.qq.com/sheet/DT0JZdlVHaklSSkN6&tab=cr8tma')">
        有问题来这里提
      </NButton>
      <NButton @click="open('https://qm.qq.com/q/5UdSTydlfi')">交流群</NButton>
    </div>

    <div class="about-warn">
      <p v-for="index in 3" :key="index" class="about-ban">{{ ban }}</p>
    </div>
  </div>
</template>

<style scoped>
.about {
  max-width: 720px;
}

.about-title {
  margin: 0 0 16px;
  font-size: 20px;
  color: var(--sloth-ok);
}

.about-actions {
  display: flex;
  gap: 12px;
  margin-bottom: 24px;
}

.about-warn {
  margin-top: 32px;
}

.about-ban {
  margin: 4px 0;
  color: var(--sloth-danger);
  font-weight: 600;
}
</style>
