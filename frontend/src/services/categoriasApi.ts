import { request } from './api'
import type { Categoria, CategoriaInput } from '../types'

export const categoriasApi = {
  obtenerTodas: () => request<Categoria[]>('/categorias'),

  crear: (categoria: CategoriaInput) =>
    request<Categoria>('/categorias', {
      method: 'POST',
      body: JSON.stringify(categoria),
    }),
}
