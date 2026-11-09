<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { NCard, NEmpty, NInput, NSpin, NTabPane, NTabs } from 'naive-ui'
import { api } from '@/api/client'
import { useMetaStore } from '@/stores/meta'
import { toastError } from '@/composables/useToast'

interface StatusEntry {
  source?: string | null
  name: string
  id: number
  duration: string
}

interface Overview {
  loggedIn: boolean
  statuses?: StatusEntry[]
  targetStatuses?: StatusEntry[]
  player?: {
    job: string
    jobId: number
    zone?: string | null
    zoneId: number
    inPvP: boolean
    inCombat: boolean
    hitboxRadius: number
  }
  target?: {
    objectId?: number | null
    objectKind?: string | null
    dataId?: number | null
    isBattleChara: boolean
    isPlayerCharacter: boolean
    distance: number
    hitboxRadius?: number | null
    inMeleeRange: boolean
    relativePosition: string
    health?: string | null
  }
  lastAction?: Record<string, string | null>
  party?: {
    partyId: number
    size: number
    members: { name: string; job: string }[]
  }
  bluSpells?: string[]
}

interface ActionEntry {
  id: number
  name: string
  level: number
  isPvp: boolean
}

interface ActionDetail {
  found: boolean
  id?: number
  name?: string
  level?: number
  isPvp?: boolean
  iconId?: number
  status?: string
  statusText?: string
  actionType?: string
  quest?: string | null
  baseRecast?: string
  maxCharges?: number
  timeSinceLastUse?: string | null
}

const meta = useMetaStore()

const overview = ref<Overview | null>(null)
const actions = ref<ActionEntry[]>([])
const detail = ref<ActionDetail | null>(null)
const loading = ref(false)
const search = ref('')

async function loadOverview(): Promise<void> {
  loading.value = true
  try {
    const result = await api.get('/api/debug/overview')
    overview.value = result as unknown as Overview
  } catch (error) {
    toastError(error instanceof Error ? error.message : String(error))
  } finally {
    loading.value = false
  }
}

async function loadActions(): Promise<void> {
  loading.value = true
  try {
    const result = (await api.get('/api/debug/actions')) as { actions?: ActionEntry[] }
    actions.value = result.actions ?? []
  } catch (error) {
    toastError(error instanceof Error ? error.message : String(error))
  } finally {
    loading.value = false
  }
}

async function loadDetail(id: number): Promise<void> {
  try {
    const result = (await api.get(`/api/debug/action/${id}`)) as unknown as ActionDetail
    detail.value = result
  } catch (error) {
    toastError(error instanceof Error ? error.message : String(error))
  }
}

const filteredActions = () => {
  const keyword = search.value.trim().toLowerCase()
  if (!keyword) return actions.value
  return actions.value.filter((action) => `${action.name} ${action.id}`.toLowerCase().includes(keyword))
}

onMounted(async () => {
  if (!meta.debug) return
  await loadOverview()
  await loadActions()
})
</script>

<template>
  <div class="debug sloth-scroll">
    <NCard v-if="!meta.debug" title="调试模式" :bordered="false">
      调试功能仅在 Debug 版本中可用。
    </NCard>

    <template v-else>
      <NSpin v-if="loading" size="small" />

      <NTabs type="line" animated>
        <NTabPane name="overview" tab="总览">
          <NCard v-if="overview && !overview.loggedIn" :bordered="false">尚未登录游戏。</NCard>

          <template v-else-if="overview">
            <NCard title="玩家" :bordered="false" class="debug-card">
              <dl class="debug-list">
                <div><dt>职业</dt><dd>{{ overview.player?.job }}（{{ overview.player?.jobId }}）</dd></div>
                <div><dt>区域</dt><dd>{{ overview.player?.zone ?? '未知' }}（{{ overview.player?.zoneId }}）</dd></div>
                <div><dt>战斗状态</dt><dd>{{ overview.player?.inCombat ? '战斗中' : '非战斗' }}</dd></div>
                <div><dt>PvP</dt><dd>{{ overview.player?.inPvP ? '是' : '否' }}</dd></div>
                <div><dt>碰撞半径</dt><dd>{{ overview.player?.hitboxRadius }}</dd></div>
              </dl>
            </NCard>

            <NCard title="目标" :bordered="false" class="debug-card">
              <dl class="debug-list">
                <div><dt>对象</dt><dd>{{ overview.target?.objectKind ?? '无' }} / {{ overview.target?.objectId ?? '-' }}</dd></div>
                <div><dt>距离</dt><dd>{{ overview.target?.distance }}</dd></div>
                <div><dt>方位</dt><dd>{{ overview.target?.relativePosition || '未知' }}</dd></div>
                <div><dt>近战范围内</dt><dd>{{ overview.target?.inMeleeRange ? '是' : '否' }}</dd></div>
                <div><dt>血量</dt><dd>{{ overview.target?.health ?? '-' }}</dd></div>
              </dl>
            </NCard>

            <NCard title="队伍" :bordered="false" class="debug-card">
              <p class="sloth-muted">队伍 {{ overview.party?.partyId }}，共 {{ overview.party?.size }} 人</p>
              <ul class="debug-list-plain">
                <li v-for="member in overview.party?.members ?? []" :key="member.name">
                  {{ member.name }} · {{ member.job }}
                </li>
              </ul>
            </NCard>

            <NCard title="最近一次动作" :bordered="false" class="debug-card">
              <dl class="debug-list">
                <div v-for="(value, key) in overview.lastAction ?? {}" :key="key">
                  <dt>{{ key }}</dt>
                  <dd>{{ value ?? '-' }}</dd>
                </div>
              </dl>
            </NCard>

            <NCard title="状态" :bordered="false" class="debug-card">
              <div class="debug-columns">
                <div>
                  <h4 class="debug-heading">自身</h4>
                  <ul class="debug-list-plain">
                    <li v-for="status in overview.statuses ?? []" :key="`s-${status.id}`">
                      {{ status.name }} <span class="sloth-muted">{{ status.duration }}</span>
                    </li>
                  </ul>
                </div>
                <div>
                  <h4 class="debug-heading">目标</h4>
                  <ul class="debug-list-plain">
                    <li v-for="status in overview.targetStatuses ?? []" :key="`t-${status.id}`">
                      {{ status.name }} <span class="sloth-muted">{{ status.duration }}</span>
                    </li>
                  </ul>
                </div>
              </div>
            </NCard>

            <NCard title="蓝魔技能" :bordered="false" class="debug-card">
              <p v-if="(overview.bluSpells ?? []).length === 0" class="sloth-muted">尚未启用任何蓝魔技能。</p>
              <ul class="debug-list-plain">
                <li v-for="spell in overview.bluSpells ?? []" :key="spell">{{ spell }}</li>
              </ul>
            </NCard>
          </template>
        </NTabPane>

        <NTabPane name="actions" tab="技能">
          <div class="debug-toolbar">
            <NInput v-model:value="search" placeholder="搜索技能" clearable size="small" />
          </div>

          <NEmpty v-if="filteredActions().length === 0" description="没有匹配的技能" size="small" />

          <ul class="debug-list-plain">
            <li v-for="action in filteredActions()" :key="action.id">
              <button type="button" class="debug-action" @click="loadDetail(action.id)">
                <span>{{ action.name }}</span>
                <span class="sloth-muted">[{{ action.id }}] Lv{{ action.level }}</span>
              </button>
            </li>
          </ul>

          <NCard v-if="detail" :title="detail.name ?? '技能详情'" :bordered="false" class="debug-card">
            <dl v-if="detail.found" class="debug-list">
              <div><dt>Id</dt><dd>{{ detail.id }}</dd></div>
              <div><dt>等级</dt><dd>{{ detail.level }}</dd></div>
              <div><dt>PvP</dt><dd>{{ detail.isPvp ? '是' : '否' }}</dd></div>
              <div><dt>类型</dt><dd>{{ detail.actionType }}</dd></div>
              <div><dt>状态</dt><dd>{{ detail.status }} · {{ detail.statusText }}</dd></div>
              <div><dt>冷却</dt><dd>{{ detail.baseRecast }}</dd></div>
              <div><dt>充能</dt><dd>{{ detail.maxCharges }}</dd></div>
              <div><dt>解锁任务</dt><dd>{{ detail.quest ?? '-' }}</dd></div>
              <div><dt>距上次使用</dt><dd>{{ detail.timeSinceLastUse ?? '-' }} 秒</dd></div>
            </dl>
            <NEmpty v-else description="找不到该技能" size="small" />
          </NCard>
        </NTabPane>
      </NTabs>
    </template>
  </div>
</template>

<style scoped>
.debug {
  max-width: 1000px;
}

.debug-card {
  margin-bottom: 12px;
}

.debug-list {
  margin: 0;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 6px 20px;
}

.debug-list > div {
  display: flex;
  gap: 10px;
  font-size: 13px;
}

.debug-list dt {
  flex: 0 0 110px;
  color: var(--sloth-text-dim);
}

.debug-list dd {
  margin: 0;
  flex: 1 1 auto;
  overflow-wrap: anywhere;
}

.debug-list-plain {
  margin: 4px 0 0;
  padding-left: 18px;
  font-size: 13px;
}

.debug-heading {
  margin: 0 0 6px;
  font-size: 13px;
}

.debug-columns {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 16px;
}

.debug-toolbar {
  margin-bottom: 8px;
  max-width: 280px;
}

.debug-action {
  display: flex;
  gap: 10px;
  width: 100%;
  padding: 3px 0;
  border: none;
  background: transparent;
  color: inherit;
  font: inherit;
  text-align: left;
  cursor: pointer;
}

.debug-action:hover {
  color: var(--sloth-accent);
}
</style>
