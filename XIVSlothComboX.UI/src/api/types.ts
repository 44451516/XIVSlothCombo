/**
 * Wire format of the plugin's local HTTP API.
 *
 * Every response is wrapped in an envelope: `{ ok: true, data }` on success and
 * `{ ok: false, error: { code, message } }` on failure.
 */

export interface ApiErrorBody {
  code: string
  message: string
}

export interface ApiEnvelope<T> {
  ok: boolean
  data?: T
  error?: ApiErrorBody
}

/** Storage kind of a configurable value. */
export type ValueType = 'int' | 'float' | 'bool' | 'boolArray' | 'intArray'

/** Control kind rendered by the frontend. */
export type OptionKind = 'slider' | 'drag' | 'radio' | 'switch' | 'multi' | 'single' | 'priority'

/** Layout hint of a multi or single option. */
export type OptionLayout = 'horizontal' | 'role-grid' | 'job-grid' | 'pvp-status' | `grid-${number}`

export interface OptionChoice {
  value: string
  label: string
  tooltip?: string | null
  iconId?: number | null
}

export interface OptionSchema {
  kind: OptionKind
  key: string
  label: string
  tooltip?: string | null
  valueType?: ValueType | null
  min?: number | null
  max?: number | null
  step?: number | null
  digits?: number | null
  speed?: number | null
  value?: unknown
  options?: OptionChoice[] | null
  layout?: OptionLayout | string | null
  group?: string | null
  optionValue?: number | null
  conditional?: boolean
  maxItems?: number | null
  order: number
}

export interface PresetNode {
  id: string
  fancyName: string
  description: string
  jobId: number
  jobName: string
  jobShorthand: string
  order: number
  parentId?: string | null
  childIds: string[]
  conflictIds: string[]
  isVariant: boolean
  isBozja: boolean
  isEureka: boolean
  isPvP: boolean
  enabled: boolean
  hidden: boolean
}

export interface PresetDetail {
  id: string
  parentChain: string[]
  conflicts: string[]
  conflictNames: string[]
  replaces: string[]
  replaceIcons: number[]
  bluNotice?: string | null
  bluStatus: string
  bluMissing?: string | null
  variantParents: string[]
  bozjaParents: string[]
  eurekaParents: string[]
}

export interface JobInfo {
  jobId: number
  name: string
  shorthand: string
  version?: string | null
  disabled: boolean
  iconId: number
  /** Notice shown above the feature list of the job, if any. */
  notice?: string | null
}

export interface MetaDto {
  version: string
  port: number
  jobs: JobInfo[]
  presets: PresetNode[]
  /** Whether the plugin was built with the debug endpoints enabled. */
  debug: boolean
}

export interface PresetOptionsDto {
  presetId: string
  options: OptionSchema[]
}

export interface PresetDetailDto {
  detail: PresetDetail
  options: OptionSchema[]
}

export interface ToggleResult {
  enabled: boolean
  forcedDisabled: string[]
}

export interface ColorDto {
  r: number
  g: number
  b: number
  a: number
}

export interface FoodDto {
  id: number
  name: string
  inInventory: boolean
  inInventoryHq: boolean
}

export interface ConfigDto {
  enabledOutputLog: boolean
  setOutChat: boolean
  hideConflictedCombos: boolean
  hideChildren: boolean
  meleeOffset: number
  targetHighlightColor: ColorDto
  autoRefine: boolean
  onlyRefineAlexander: boolean
  autoFood: boolean
  autoFoodId: number
  requiredFoodHq: boolean
  autoRefinePotion: boolean
  hideMessageOfTheDay: boolean
  specialEvent: boolean
  movementLeeway: number
  performanceMode: boolean
  recommendedSettingsViewed: boolean
  activeBluSpells: number[]
  foods: FoodDto[]
}

export interface TimelineActionDto {
  actionId: number
  actionName?: string | null
  actionIcon?: number | null
  useTimeStart: number
  useTimeEnd: number
  targetType: number
  customActionType: number
  customActionTypeSub: number
  x: number
  y: number
  z: number
}

export interface TimelineDto {
  index: number
  enable: boolean
  jobId: number
  name: string
  actionList: TimelineActionDto[]
}

export interface CustomTypeDto {
  value: number
  label: string
}

export interface TimelineStateDto {
  timelines: TimelineDto[]
  working: TimelineDto[]
  current: TimelineDto | null
  useItem: boolean
  customTypes: CustomTypeDto[]
  customTypeSubs: CustomTypeDto[]
}

export interface HealthDto {
  version: string
  port: number
}
