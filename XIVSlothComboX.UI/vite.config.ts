import { readFileSync } from 'node:fs'
import { resolve } from 'node:path'
import { fileURLToPath, URL } from 'node:url'
import { defineConfig, type PluginOption } from 'vitest/config'
import vue from '@vitejs/plugin-vue'

const projectRoot = fileURLToPath(new URL('.', import.meta.url))

interface WebUiInfo {
  port: number
  token: string
  url: string
}

/**
 * The program writes the port and the access token of its local web server into
 * its data folder. The dev server uses it to proxy the API and to hand the
 * token to the client.
 */
function infoFileCandidates(): string[] {
  const candidates: string[] = []

  const override = process.env.SLOTH_WEBUI_INFO
  if (override) candidates.push(override)

  const appData = process.env.APPDATA
  if (appData) {
    // The standalone program keeps its description in its own folder.
    candidates.push(resolve(appData, 'XIVSlothComboX', 'sloth-webui.json'))
  }

  return candidates
}

function readWebUiInfo(): WebUiInfo | null {
  for (const candidate of infoFileCandidates()) {
    try {
      const raw = readFileSync(candidate, 'utf8')
      const parsed = JSON.parse(raw) as Partial<WebUiInfo>
      if (!parsed.port || !parsed.token) continue
      return { port: parsed.port, token: parsed.token, url: parsed.url ?? '' }
    } catch {
      // Try the next candidate.
    }
  }

  return null
}

/** Serves the info file so the client can discover the running plugin. */
function webUiInfoPlugin(): PluginOption {
  return {
    name: 'sloth-webui-info',
    configureServer(server) {
      server.middlewares.use('/__sloth_info', (_request, response) => {
        const info = readWebUiInfo()
        if (!info) {
          response.statusCode = 404
          response.end('{}')
          return
        }
        response.setHeader('Content-Type', 'application/json; charset=utf-8')
        response.end(JSON.stringify(info))
      })
    },
  }
}

export default defineConfig(() => {
  const info = readWebUiInfo()

  return {
    plugins: [vue(), webUiInfoPlugin()],
    base: './',
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
      },
    },
    define: {
      __SLOTH_TOKEN__: JSON.stringify(info?.token ?? ''),
    },
    server: {
      port: 5273,
      strictPort: false,
      proxy: info
        ? {
            '/api': {
              target: `http://127.0.0.1:${info.port}`,
              changeOrigin: false,
              headers: {
                'X-Sloth-Token': info.token,
                // The plugin only accepts requests that originate from its own page,
                // so the dev server has to claim the plugin origin.
                Origin: `http://127.0.0.1:${info.port}`,
              },
            },
          }
        : undefined,
    },
    build: {
      outDir: resolve(projectRoot, 'wwwroot'),
      emptyOutDir: true,
      assetsDir: '.',
      modulePreload: { polyfill: false },
      chunkSizeWarningLimit: 2048,
      rollupOptions: {
        output: {
          entryFileNames: 'assets/[name]-[hash].js',
          chunkFileNames: 'assets/[name]-[hash].js',
          assetFileNames: 'assets/[name]-[hash][extname]',
        },
      },
    },
    test: {
      environment: 'jsdom',
      globals: true,
      include: ['src/**/*.spec.ts'],
    },
  }
})
