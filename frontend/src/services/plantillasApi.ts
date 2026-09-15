import { request } from './api'
import type { PlantillaTarea, PlantillaTareaInput, TodoItem } from '../types'

export const plantillasApi = {
  obtenerTodas: () => request<PlantillaTarea[]>('/plantillas'),

  obtenerPorId: (id: number) => request<PlantillaTarea>(`/plantillas/${id}`),

  crear: (plantilla: PlantillaTareaInput) =>
    request<PlantillaTarea>('/plantillas', {
      method: 'POST',
      body: JSON.stringify(plantilla),
    }),

  actualizar: (id: number, plantilla: PlantillaTareaInput) =>
    request<PlantillaTarea>(`/plantillas/${id}`, {
      method: 'PUT',
      body: JSON.stringify(plantilla),
    }),

  eliminar: (id: number) =>
    request<void>(`/plantillas/${id}`, {
      method: 'DELETE',
    }),

  instanciar: (id: number) =>
    request<TodoItem>(`/plantillas/${id}/instanciar`, {
      method: 'POST',
    }),
}
