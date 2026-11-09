import { describe, expect, it } from 'vitest'
import { groupOptions } from './options'
import type { OptionSchema } from './types'

function radio(key: string, optionValue: number, label: string, order: number): OptionSchema {
  return {
    kind: 'radio',
    key,
    group: key,
    label,
    optionValue,
    value: 0,
    order,
    valueType: 'int',
  }
}

function switchOption(key: string, order: number): OptionSchema {
  return { kind: 'switch', key, label: key, value: false, order, valueType: 'bool' }
}

describe('groupOptions', () => {
  it('merges the members of a radio group', () => {
    const result = groupOptions([
      radio('a', 0, 'A', 0),
      radio('a', 1, 'B', 1),
      radio('b', 0, 'C', 2),
    ])

    expect(result).toHaveLength(2)
    expect(result[0].kind).toBe('radio')
    expect(result[0].members).toHaveLength(2)
    expect(result[0].members.map((member) => member.label)).toEqual(['A', 'B'])
    expect(result[1].members).toHaveLength(1)
  })

  it('keeps the original order of the options', () => {
    const result = groupOptions([
      switchOption('s', 0),
      radio('a', 0, 'A', 1),
      radio('a', 1, 'B', 2),
      switchOption('t', 3),
    ])

    expect(result.map((item) => item.kind)).toEqual(['switch', 'radio', 'switch'])
    expect(result[1].members).toHaveLength(2)
  })

  it('leaves options that are not radios untouched', () => {
    const result = groupOptions([switchOption('s', 0)])

    expect(result).toHaveLength(1)
    expect(result[0].members).toHaveLength(0)
    expect(result[0].option.key).toBe('s')
  })

  it('falls back to the key when a radio has no group', () => {
    const orphan: OptionSchema = {
      kind: 'radio',
      key: 'orphan',
      label: 'O',
      optionValue: 0,
      value: 0,
      order: 0,
      valueType: 'int',
    }

    const result = groupOptions([orphan, { ...orphan, optionValue: 1, order: 1 }])

    expect(result).toHaveLength(1)
    expect(result[0].members).toHaveLength(2)
  })
})
