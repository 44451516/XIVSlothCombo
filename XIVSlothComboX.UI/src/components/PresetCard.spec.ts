import { mount } from '@vue/test-utils'
import { createPinia } from 'pinia'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import type { PresetNode } from '@/api/types'

const presetOptions = vi.fn()
const togglePreset = vi.fn()

vi.mock('@/api/client', () => ({
  api: {
    presetOptions: (...args: unknown[]) => presetOptions(...args),
    preset: vi.fn().mockResolvedValue({
      detail: {
        id: 'x',
        parentChain: [],
        conflicts: [],
        conflictNames: [],
        replaces: [],
        replaceIcons: [],
        bluStatus: 'none',
        variantParents: [],
        bozjaParents: [],
        eurekaParents: [],
      },
      options: [],
    }),
    togglePreset: (...args: unknown[]) => togglePreset(...args),
    setValue: vi.fn(),
  },
  iconUrl: (iconId: number) => `/api/icon/${iconId}`,
  ApiError: class ApiError extends Error {},
}))

import PresetCard from './PresetCard.vue'

function makePreset(id: string, name: string, overrides: Partial<PresetNode> = {}): PresetNode {
  return {
    id,
    fancyName: name,
    description: `${name} 的说明`,
    jobId: 1,
    jobName: '战士',
    jobShorthand: 'WAR',
    order: 0,
    parentId: null,
    childIds: [],
    conflictIds: [],
    isVariant: false,
    isBozja: false,
    isEureka: false,
    isPvP: false,
    enabled: true,
    hidden: false,
    ...overrides,
  }
}

async function mountCard(preset: PresetNode, children: PresetNode[] = []) {
  const wrapper = mount(PresetCard, {
    props: { preset, children },
    global: { plugins: [createPinia()] },
  })
  // Let the watchers settle.
  await wrapper.vm.$nextTick()
  return wrapper
}

describe('PresetCard', () => {
  beforeEach(() => {
    presetOptions.mockReset()
    presetOptions.mockResolvedValue({ presetId: 'x', options: [] })
    togglePreset.mockReset()
    togglePreset.mockResolvedValue({ enabled: true, forcedDisabled: [] })
  })

  it('shows the name and the description of the feature', async () => {
    const wrapper = await mountCard(makePreset('A', '单体连击'))

    expect(wrapper.text()).toContain('单体连击')
    expect(wrapper.text()).toContain('单体连击 的说明')
  })

  it('renders the child features as a nested tree', async () => {
    const parent = makePreset('A', '父功能', { childIds: ['B'] })
    const child = makePreset('B', '子功能', { parentId: 'A' })

    const wrapper = await mountCard(parent, [child])

    expect(wrapper.text()).toContain('父功能')
    expect(wrapper.text()).toContain('子功能')
  })

  it('asks the server for the options when it is opened', async () => {
    const wrapper = await mountCard(makePreset('A', '单体连击'))

    await wrapper.find('.preset-title').trigger('click')
    await wrapper.vm.$nextTick()

    expect(presetOptions).toHaveBeenCalledWith('A')
  })

  it('marks a feature that conflicts with an enabled one', async () => {
    const wrapper = await mountCard(makePreset('A', '冲突功能', { hidden: true }))

    expect(wrapper.text()).toContain('与已启用的功能冲突')
  })
})
