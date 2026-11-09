import { beforeEach, describe, expect, it, vi } from 'vitest'

function jsonResponse(body: unknown, status = 200) {
  return {
    ok: status >= 200 && status < 300,
    status,
    json: async () => body,
    text: async () => JSON.stringify(body),
  }
}

describe('api client', () => {
  beforeEach(() => {
    vi.resetModules()
    vi.unstubAllGlobals()
    window.history.replaceState({}, '', '/')
    window.sessionStorage.clear()
  })

  it('sends the token from the query string', async () => {
    window.history.replaceState({}, '', '/?t=secret-token')

    const fetchMock = vi.fn().mockResolvedValue(jsonResponse({ ok: true, data: { value: 7 } }))
    vi.stubGlobal('fetch', fetchMock)

    const { api } = await import('./client')
    const result = await api.getValue('some_key', 'int')

    expect(result.value).toBe(7)

    const [, init] = fetchMock.mock.calls[0]
    const headers = init.headers as Record<string, string>
    expect(headers['X-Sloth-Token']).toBe('secret-token')
    expect(init.method).toBe('GET')
  })

  it('remembers the token for later requests', async () => {
    window.history.replaceState({}, '', '/?t=secret-token')

    const fetchMock = vi.fn().mockResolvedValue(jsonResponse({ ok: true, data: {} }))
    vi.stubGlobal('fetch', fetchMock)

    const { api } = await import('./client')
    await api.health()

    window.history.replaceState({}, '', '/')
    await api.health()

    expect(window.sessionStorage.getItem('sloth-webui-token')).toBe('secret-token')
    const headers = fetchMock.mock.calls[1][1].headers as Record<string, string>
    expect(headers['X-Sloth-Token']).toBe('secret-token')
  })

  it('unwraps the data of a successful answer', async () => {
    const fetchMock = vi.fn().mockResolvedValue(jsonResponse({ ok: true, data: { port: 1234 } }))
    vi.stubGlobal('fetch', fetchMock)

    const { api } = await import('./client')
    const health = await api.health()

    expect(health.port).toBe(1234)
  })

  it('turns an error envelope into an ApiError', async () => {
    const fetchMock = vi
      .fn()
      .mockResolvedValue(
        jsonResponse({ ok: false, error: { code: 'UNAUTHORIZED', message: '会话已失效' } }, 401),
      )
    vi.stubGlobal('fetch', fetchMock)

    const { ApiError, api } = await import('./client')

    await expect(api.meta()).rejects.toBeInstanceOf(ApiError)
    await expect(api.meta()).rejects.toThrow('会话已失效')
  })

  it('reports a network failure', async () => {
    const fetchMock = vi.fn().mockRejectedValue(new Error('boom'))
    vi.stubGlobal('fetch', fetchMock)

    const { ApiError, api } = await import('./client')

    await expect(api.meta()).rejects.toBeInstanceOf(ApiError)
  })

  it('sends the value type along with a written value', async () => {
    window.history.replaceState({}, '', '/?t=secret-token')

    const fetchMock = vi.fn().mockResolvedValue(jsonResponse({ ok: true, data: {} }))
    vi.stubGlobal('fetch', fetchMock)

    const { api } = await import('./client')
    await api.setValue('key', 'boolArray', [true, false])

    const [, init] = fetchMock.mock.calls.at(-1)!
    expect(init.method).toBe('POST')
    expect(JSON.parse(init.body as string)).toEqual({
      key: 'key',
      type: 'boolArray',
      value: [true, false],
    })
  })

  it('asks the dev server for the token when the page has none', async () => {
    const fetchMock = vi
      .fn()
      .mockResolvedValueOnce(jsonResponse({ port: 4321, token: 'found-token' }))
      .mockResolvedValue(jsonResponse({ ok: true, data: {} }))
    vi.stubGlobal('fetch', fetchMock)

    const { api } = await import('./client')
    await api.health()

    expect(fetchMock.mock.calls[0][0]).toBe('/__sloth_info')
    const headers = fetchMock.mock.calls[1][1].headers as Record<string, string>
    expect(headers['X-Sloth-Token']).toBe('found-token')
  })

  it('builds the address of an icon with the token', async () => {
    window.history.replaceState({}, '', '/?t=icon-token')

    const { iconUrl } = await import('./client')
    const url = iconUrl(62101, true)

    expect(url).toContain('/api/icon/62101')
    expect(url).toContain('hd=1')
    expect(url).toContain('t=icon-token')
  })
})
