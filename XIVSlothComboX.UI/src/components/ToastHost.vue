<script setup lang="ts">
import { useToasts } from '@/composables/useToast'
import { NAlert } from 'naive-ui'

const toasts = useToasts()
</script>

<template>
  <div class="toast-host">
    <NAlert
      v-for="item in toasts"
      :key="item.id"
      class="toast-item"
      :type="item.kind === 'error' ? 'error' : item.kind === 'success' ? 'success' : 'info'"
      :closable="false"
      role="status"
    >
      {{ item.content }}
    </NAlert>
  </div>
</template>

<style scoped>
.toast-host {
  position: fixed;
  top: 16px;
  right: 16px;
  z-index: 9999;
  display: flex;
  flex-direction: column;
  gap: 8px;
  max-width: 420px;
}

.toast-item {
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.45);
}
</style>
