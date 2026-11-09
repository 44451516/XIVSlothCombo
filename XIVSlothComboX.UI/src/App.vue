<script setup lang="ts">
import { NConfigProvider, NGlobalStyle, darkTheme, type GlobalThemeOverrides } from 'naive-ui'
import AppShell from '@/components/AppShell.vue'
import ToastHost from '@/components/ToastHost.vue'

const themeOverrides: GlobalThemeOverrides = {
  common: {
    primaryColor: '#7d8cd6',
    primaryColorHover: '#8e9bdf',
    primaryColorPressed: '#6d7cc4',
    primaryColorSuppl: '#7d8cd6',
    infoColor: '#7d8cd6',
    bodyColor: '#14161f',
    cardColor: '#222634',
    modalColor: '#1b1e2a',
    popoverColor: '#222634',
    tableColor: '#222634',
    borderColor: '#303647',
    textColorBase: '#dfe3ee',
    fontSize: '14px',
    borderRadius: '8px',
  },
}
</script>

<template>
  <NConfigProvider :theme="darkTheme" :theme-overrides="themeOverrides">
    <NGlobalStyle />
    <AppShell />
    <ToastHost />
  </NConfigProvider>
</template>
