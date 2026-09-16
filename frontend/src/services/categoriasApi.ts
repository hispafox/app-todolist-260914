import { request } from './api'
import type { Categoria, CategoriaInput } from '../types'

export const categoriasApi = {
  obtenerTodas: () => request<Categoria[]>('/categorias'),

  obtenerPorId: (id: number) => request<Categoria>(`/categorias/${id}`),

  crear: (categoria: CategoriaInput) =>
    request<Categoria>('/categorias', {
      method: 'POST',
      body: JSON.stringify(categoria),
    }),

  actualizar: (id: number, categoria: CategoriaInput) =>
    request<Categoria>(`/categorias/${id}`, {
      method: 'PUT',
      body: JSON.stringify(categoria),
    }),

  eliminar: (id: number) => request<void>(`/categorias/${id}`, { method: 'DELETE' }),
}
