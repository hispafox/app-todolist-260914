import { request } from './api'
import type { TodoItem, TodoItemInput } from '../types'

export const tareasApi = {
  obtenerTodas: () => request<TodoItem[]>('/tareas'),

  obtenerPorId: (id: number) => request<TodoItem>(`/tareas/${id}`),

  crear: (tarea: TodoItemInput) =>
    request<TodoItem>('/tareas', {
      method: 'POST',
      body: JSON.stringify(tarea),
    }),

  actualizar: (id: number, tarea: TodoItemInput) =>
    request<TodoItem>(`/tareas/${id}`, {
      method: 'PUT',
      body: JSON.stringify(tarea),
    }),

  eliminar: (id: number) =>
    request<void>(`/tareas/${id}`, {
      method: 'DELETE',
    }),

  completar: (id: number) =>
    request<TodoItem>(`/tareas/${id}/completar`, {
      method: 'POST',
    }),
}
