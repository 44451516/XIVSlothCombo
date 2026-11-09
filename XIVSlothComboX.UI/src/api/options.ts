import type { OptionKind, OptionSchema, ValueType } from './types'

/**
 * A radio group is delivered as one entry per choice. The frontend merges those
 * entries into a single control so that the choices stay mutually exclusive.
 */
export interface RenderableOption {
  kind: OptionKind
  /** The schema of the first member of the group. */
  option: OptionSchema
  /** All members of a radio group; empty for every other kind. */
  members: OptionSchema[]
}

/** What a control reports back when the user changes a value. */
export interface OptionChangePayload {
  key: string
  valueType: ValueType
  value: unknown
  /** Whether the options of the feature have to be reloaded. */
  reload: boolean
}

function radioGroupKey(option: OptionSchema): string {
  return option.group ?? option.key
}

/**
 * Merges radio entries into groups and keeps every other option untouched.
 * The original order of the options is preserved.
 */
export function groupOptions(options: OptionSchema[]): RenderableOption[] {
  const result: RenderableOption[] = []
  const radioGroups = new Map<string, RenderableOption>()

  for (const option of options) {
    if (option.kind !== 'radio') {
      result.push({ kind: option.kind, option, members: [] })
      continue
    }

    const key = radioGroupKey(option)
    const existing = radioGroups.get(key)
    if (existing) {
      existing.members.push(option)
    } else {
      const created: RenderableOption = { kind: 'radio', option, members: [option] }
      radioGroups.set(key, created)
      result.push(created)
    }
  }

  return result
}
