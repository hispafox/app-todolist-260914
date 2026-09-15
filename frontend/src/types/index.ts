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

export interface TodoItem {
  id: number
  title: string
  isCompleted: boolean
  createdAt: string
  esRepetitiva: boolean
  recurrencia: TipoRecurrencia | null
  proximaFecha: string | null
  plantillaId: number | null
  categoriaId: number | null
  personaId: number | null
}

export interface TodoItemInput {
  title: string
  isCompleted: boolean
  esRepetitiva: boolean
  recurrencia: TipoRecurrencia | null
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
