import { ref } from 'vue'

export type ToastKind = 'success' | 'error' | 'info'

export interface ToastItem {
  id: number
  kind: ToastKind
  content: string
}

const items = ref<ToastItem[]>([])
let nextId = 1

/** The queue of the messages shown in the corner of the page. */
export function useToasts(): typeof items {
  return items
}

export function toast(kind: ToastKind, content: string, timeout = 3200): void {
  const id = nextId++
  items.value = [...items.value, { id, kind, content }]
  window.setTimeout(() => {
    items.value = items.value.filter((item) => item.id !== id)
  }, timeout)
}

export function toastSuccess(content: string): void {
  toast('success', content)
}

export function toastError(content: string): void {
  toast('error', content, 5200)
}

export function toastInfo(content: string): void {
  toast('info', content)
}
