import { test, type Page } from '@playwright/test'
import { mkdirSync } from 'node:fs'
import path from 'node:path'

// Recorre la SPA (pestañas por estado, sin rutas) y genera las 12 capturas usadas en docs/manual-usuario.md.
const CARPETA_CAPTURAS = path.join(__dirname, 'img')

async function capturar(page: Page, nombreFichero: string) {
  await page.waitForLoadState('networkidle')
  await page.screenshot({
    path: path.join(CARPETA_CAPTURAS, nombreFichero),
    animations: 'disabled',
  })
}

async function irAPestana(page: Page, nombre: 'Tareas' | 'Plantillas' | 'Categorías') {
  await page.locator('.app-nav button', { hasText: nombre }).click()
}

test.beforeAll(() => {
  mkdirSync(CARPETA_CAPTURAS, { recursive: true })
})

test('genera las capturas del manual de usuario', async ({ page }) => {
  await page.goto('/')
  await capturar(page, '01-pantalla-tareas.png')

  // --- Categorías: se crea "Trabajo" para poder asignarla luego a una tarea ---
  await irAPestana(page, 'Categorías')
  await capturar(page, '10-pantalla-categorias.png')

  await page.locator('#categoria-nombre').fill('Trabajo')
  await page.locator('#categoria-color').evaluate((el: HTMLInputElement) => {
    el.value = '#2563eb'
    el.dispatchEvent(new Event('input', { bubbles: true }))
  })
  await capturar(page, '11-formulario-categoria.png')
  await page.getByRole('button', { name: 'Guardar' }).click()
  await page.waitForLoadState('networkidle')

  await page.getByRole('button', { name: 'Editar' }).first().click()
  await capturar(page, '12-editar-categoria.png')
  await page.getByRole('button', { name: 'Cancelar' }).click()

  // --- Tareas: se crea una tarea repetitiva con categoría, se completa y se edita ---
  await irAPestana(page, 'Tareas')
  await page.waitForLoadState('networkidle')

  await page.locator('input[placeholder="Título de la tarea"]').fill('Preparar informe semanal')
  await page.getByLabel('Es repetitiva').check()
  await page.locator('form.formulario select').first().selectOption({ label: 'Semanal' })
  await page.locator('form.formulario select').last().selectOption({ label: 'Trabajo' })
  await capturar(page, '02-formulario-nueva-tarea.png')

  await page.getByRole('button', { name: 'Guardar' }).click()
  await page.waitForLoadState('networkidle')
  await capturar(page, '03-lista-tareas-con-etiquetas.png')

  await page.getByRole('button', { name: 'Completar' }).first().click()
  await page.waitForLoadState('networkidle')
  await capturar(page, '04-tarea-completada.png')

  await page.getByRole('button', { name: 'Editar' }).first().click()
  await capturar(page, '05-editar-tarea.png')
  await page.getByRole('button', { name: 'Cancelar' }).click()

  // --- Plantillas: se crea una plantilla repetitiva y se instancia una tarea desde ella ---
  await irAPestana(page, 'Plantillas')
  await page.waitForLoadState('networkidle')
  await capturar(page, '06-pantalla-plantillas.png')

  await page.locator('input[placeholder="Título de la plantilla"]').fill('Reunión de equipo')
  await page.getByLabel('Genera tareas repetitivas').check()
  await page.locator('form.formulario select').selectOption({ label: 'Semanal' })
  await capturar(page, '07-formulario-nueva-plantilla.png')

  await page.getByRole('button', { name: 'Guardar' }).click()
  await page.waitForLoadState('networkidle')
  await capturar(page, '08-lista-plantillas.png')

  await page.getByRole('button', { name: 'Crear tarea' }).first().click()
  await capturar(page, '09-plantilla-instanciar.png')
})
