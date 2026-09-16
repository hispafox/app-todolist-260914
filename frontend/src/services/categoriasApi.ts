import { request } from './api'
import type { Categoria } from '../types'

export const categoriasApi = {
  obtenerTodas: () => request<Categoria[]>('/categorias'),
}
