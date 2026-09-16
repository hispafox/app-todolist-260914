// Los valores numéricos deben coincidir con el enum TipoRecurrencia del backend (AppTodoList.Models).
export const TipoRecurrencia = {
  Diaria: 0,
  Semanal: 1,
  Mensual: 2,
} as const

export type TipoRecurrencia = (typeof TipoRecurrencia)[keyof typeof TipoRecurrencia]

export const ETIQUETAS_RECURRENCIA: Record<TipoRecurrencia, string> = {
  [TipoRecurrencia.Diaria]: 'Diaria',
  [TipoRecurrencia.Semanal]: 'Semanal',
  [TipoRecurrencia.Mensual]: 'Mensual',
}

export interface Categoria {
  id: number
  nombre: string
  color: string
}

export interface TodoItem {
  id: number
  titulo: string
  completada: boolean
  createdAt: string
  esRepetitiva: boolean
  recurrencia: TipoRecurrencia | null
  proximaFecha: string | null
  plantillaId: number | null
  categoriaId: number | null
  categoria?: Categoria | null
  personaId: number | null
}

export interface TodoItemInput {
  titulo: string
  completada: boolean
  esRepetitiva: boolean
  recurrencia: TipoRecurrencia | null
  categoriaId: number | null
}

export interface PlantillaTarea {
  id: number
  titulo: string
  esRepetitiva: boolean
  recurrencia: TipoRecurrencia | null
}

export interface PlantillaTareaInput {
  titulo: string
  esRepetitiva: boolean
  recurrencia: TipoRecurrencia | null
}
