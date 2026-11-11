import { createRouter, createWebHashHistory, type RouteRecordRaw } from 'vue-router'

const routes: RouteRecordRaw[] = [
  { path: '/', redirect: '/pve' },
  { path: '/pve', name: 'pve', component: () => import('@/views/PvEView.vue') },
  { path: '/v8', name: 'v8', component: () => import('@/views/RotationView.vue') },
  { path: '/pvp', name: 'pvp', component: () => import('@/views/PvPView.vue') },
  { path: '/timeline', name: 'timeline', component: () => import('@/views/TimelineMainView.vue') },
  {
    path: '/timeline/edit',
    name: 'timeline-edit',
    component: () => import('@/views/TimelineEditView.vue'),
  },
  { path: '/settings', name: 'settings', component: () => import('@/views/SettingsView.vue') },
  { path: '/about', name: 'about', component: () => import('@/views/AboutView.vue') },
  { path: '/afdian', name: 'afdian', component: () => import('@/views/AfdianView.vue') },
  { path: '/debug', name: 'debug', component: () => import('@/views/DebugView.vue') },
]

// The hash history keeps the SPA working on the plugin's own server, which does
// not know how to rewrite unknown paths to the entry point.
export const router = createRouter({
  history: createWebHashHistory(),
  routes,
})

export default router
