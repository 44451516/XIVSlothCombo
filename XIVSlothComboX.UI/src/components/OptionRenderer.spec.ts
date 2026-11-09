import { mount } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import OptionRenderer from './OptionRenderer.vue'
import { groupOptions } from '@/api/options'
import type { OptionKind, OptionSchema } from '@/api/types'

function makeStub(kind: OptionKind, value: unknown) {
  return {
    props: ['option'],
    emits: ['change'],
    template: `<button class="stub-${kind}" @click="$emit('change', ${JSON.stringify(value)})" />`,
  }
}

const stubs = {
  SliderOption: makeStub('slider', 12),
  DragOption: makeStub('drag', 1.5),
  SwitchOption: makeStub('switch', true),
  RadioOption: makeStub('radio', 3),
  MultiOption: makeStub('multi', [true, false]),
  SingleOption: makeStub('single', 2),
  PriorityOption: makeStub('priority', [1, 2]),
}

function option(kind: OptionKind, overrides: Partial<OptionSchema> = {}): OptionSchema {
  return {
    kind,
    key: 'the_key',
    label: '标签',
    value: 0,
    order: 0,
    valueType: 'int',
    ...overrides,
  }
}

function render(item: OptionSchema) {
  const grouped = groupOptions([item])
  return mount(OptionRenderer, {
    props: { item: grouped[0] },
    global: { stubs },
  })
}

describe('OptionRenderer', () => {
  it('renders a slider and reports the new value', async () => {
    const wrapper = render(option('slider', { valueType: 'int' }))
    expect(wrapper.find('.stub-slider').exists()).toBe(true)

    await wrapper.find('.stub-slider').trigger('click')

    const payload = wrapper.emitted('change')?.[0]?.[0] as {
      key: string
      valueType: string
      value: unknown
      reload: boolean
    }
    expect(payload.key).toBe('the_key')
    expect(payload.valueType).toBe('int')
    expect(payload.value).toBe(12)
    expect(payload.reload).toBe(false)
  })

  it('renders a drag input', async () => {
    const wrapper = render(option('drag', { valueType: 'float' }))
    expect(wrapper.find('.stub-drag').exists()).toBe(true)

    await wrapper.find('.stub-drag').trigger('click')
    const payload = wrapper.emitted('change')?.[0]?.[0] as { valueType: string; value: unknown }
    expect(payload.valueType).toBe('float')
    expect(payload.value).toBe(1.5)
  })

  it('renders a switch and asks for a reload of the options', async () => {
    const wrapper = render(option('switch', { valueType: 'bool' }))
    expect(wrapper.find('.stub-switch').exists()).toBe(true)

    await wrapper.find('.stub-switch').trigger('click')
    const payload = wrapper.emitted('change')?.[0]?.[0] as { valueType: string; reload: boolean }
    expect(payload.valueType).toBe('bool')
    expect(payload.reload).toBe(true)
  })

  it('renders a radio group', async () => {
    const wrapper = render(option('radio', { valueType: 'int', group: 'the_key' }))
    expect(wrapper.find('.stub-radio').exists()).toBe(true)

    await wrapper.find('.stub-radio').trigger('click')
    const payload = wrapper.emitted('change')?.[0]?.[0] as { value: unknown; reload: boolean }
    expect(payload.value).toBe(3)
    expect(payload.reload).toBe(false)
  })

  it('renders a multi choice', async () => {
    const wrapper = render(option('multi', { valueType: 'boolArray' }))
    expect(wrapper.find('.stub-multi').exists()).toBe(true)

    await wrapper.find('.stub-multi').trigger('click')
    const payload = wrapper.emitted('change')?.[0]?.[0] as { valueType: string; value: unknown }
    expect(payload.valueType).toBe('boolArray')
    expect(payload.value).toEqual([true, false])
  })

  it('renders a single choice', async () => {
    const wrapper = render(option('single', { valueType: 'int' }))
    expect(wrapper.find('.stub-single').exists()).toBe(true)

    await wrapper.find('.stub-single').trigger('click')
    const payload = wrapper.emitted('change')?.[0]?.[0] as { value: unknown }
    expect(payload.value).toBe(2)
  })

  it('renders a priority list', async () => {
    const wrapper = render(option('priority', { valueType: 'intArray' }))
    expect(wrapper.find('.stub-priority').exists()).toBe(true)

    await wrapper.find('.stub-priority').trigger('click')
    const payload = wrapper.emitted('change')?.[0]?.[0] as { valueType: string; value: unknown }
    expect(payload.valueType).toBe('intArray')
    expect(payload.value).toEqual([1, 2])
  })
})
