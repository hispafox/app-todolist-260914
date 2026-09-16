import { defineConfig } from '@playwright/test'

// Config aislada para las capturas del manual de usuario: no toca el proyecto frontend.
export default defineConfig({
  testDir: '.',
  timeout: 30_000,
  use: {
    baseURL: 'http://localhost:5173',
    viewport: { width: 1440, height: 900 },
    deviceScaleFactor: 2, // capturas nítidas en pantallas de alta densidad
    colorScheme: 'light',
    locale: 'es-ES',
    timezoneId: 'Europe/Madrid',
  },
})
