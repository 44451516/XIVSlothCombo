<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { RouterLink, RouterView } from 'vue-router'
import { NSpin } from 'naive-ui'
import { useMetaStore } from '@/stores/meta'
import { useConfigStore } from '@/stores/config'
import { toastError } from '@/composables/useToast'

const meta = useMetaStore()
const config = useConfigStore()

const navItems = computed(() => {
  const items = [
    { to: '/pve', label: 'PvE 功能' },
    { to: '/v8', label: '8.0 专区' },
    { to: '/pvp', label: 'PvP 功能' },
    { to: '/timeline/edit', label: '时间轴编辑' },
    { to: '/timeline', label: '时间轴' },
    { to: '/settings', label: '综合设置' },
    { to: '/about', label: '提出建议' },
    { to: '/afdian', label: '爱发电' },
  ]

  if (meta.debug) items.push({ to: '/debug', label: '调试模式' })
  return items
})

onMounted(async () => {
  const failures: string[] = []
  try {
    await meta.load()
  } catch (error) {
    failures.push(`职业数据：${error instanceof Error ? error.message : String(error)}`)
  }

  // The top level configuration is needed by the feature list as well, so that
  // settings such as "hide the options of disabled features" apply right away.
  try {
    await config.load()
  } catch (error) {
    failures.push(`设置：${error instanceof Error ? error.message : String(error)}`)
  }
  if (failures.length) toastError(`本地服务初始化失败。${failures.join('；')}`)
})
</script>

<template>
  <div class="shell">
    <nav class="shell-nav">
      <div class="shell-brand">
        <span class="shell-title">XIVSlothComboX</span>
        <span v-if="meta.version" class="shell-version">v{{ meta.version }}</span>
      </div>

      <div class="shell-links">
        <RouterLink
          v-for="item in navItems"
          :key="item.to"
          class="shell-link"
          :to="item.to"
          active-class="shell-link-active"
        >
          {{ item.label }}
        </RouterLink>
      </div>

      <div class="shell-foot">
        <span v-if="meta.port" class="sloth-muted">本地服务端口 {{ meta.port }}</span>
      </div>
    </nav>

    <main class="shell-main">
      <NSpin v-if="meta.loading && !meta.loaded" class="shell-spin" size="small" />
      <RouterView v-else />
    </main>
  </div>
</template>

<style scoped>
.shell {
  display: flex;
  height: 100vh;
  height: 100dvh;
  min-height: 0;
}

.shell-nav {
  flex: 0 0 180px;
  display: flex;
  flex-direction: column;
  gap: 12px;
  padding: 16px 12px;
  background: var(--sloth-bg-soft);
  border-right: 1px solid var(--sloth-border-soft);
}

.shell-brand {
  display: flex;
  flex-direction: column;
  gap: 2px;
  padding: 0 6px 8px;
  border-bottom: 1px solid var(--sloth-border-soft);
}

.shell-title {
  font-weight: 700;
  letter-spacing: 0;
}

.shell-version {
  font-size: 11px;
  color: var(--sloth-text-dim);
}

.shell-links {
  display: flex;
  flex-direction: column;
  gap: 2px;
  flex: 1 1 auto;
}

.shell-link {
  display: block;
  padding: 8px 10px;
  border-radius: var(--sloth-radius);
  color: var(--sloth-text);
  text-decoration: none;
  font-size: 13px;
}

.shell-link:hover {
  background: rgba(255, 255, 255, 0.04);
  color: var(--sloth-text);
}

.shell-link-active {
  background: var(--sloth-accent-soft);
  color: #fff;
}

.shell-foot {
  padding: 8px 6px 0;
  border-top: 1px solid var(--sloth-border-soft);
  font-size: 11px;
}

.shell-main {
  flex: 1 1 auto;
  min-width: 0;
  min-height: 0;
  padding: 16px 20px;
  display: flex;
  flex-direction: column;
}

.shell-spin {
  margin: 40px auto;
}

@media (max-width: 640px) {
  .shell {
    flex-direction: column;
  }

  .shell-nav {
    flex: 0 0 auto;
    min-width: 0;
    gap: 8px;
    padding: 10px 12px;
    border-right: 0;
    border-bottom: 1px solid var(--sloth-border-soft);
  }

  .shell-brand {
    flex-direction: row;
    align-items: baseline;
    gap: 8px;
    padding: 0;
    border-bottom: 0;
  }

  .shell-links {
    flex: 0 0 auto;
    flex-direction: row;
    overflow-x: auto;
    scrollbar-width: thin;
  }

  .shell-link {
    flex: 0 0 auto;
    white-space: nowrap;
  }

  .shell-foot {
    display: none;
  }

  .shell-main {
    padding: 12px;
  }
}
</style>
