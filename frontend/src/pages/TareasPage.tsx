import { useEffect, useState } from 'react'
import { tareasApi } from '../services/tareasApi'
import { ApiError } from '../services/api'
import type { TodoItem, TodoItemInput } from '../types'
import { TareaForm } from '../components/TareaForm'
import { TareaItem } from '../components/TareaItem'

export function TareasPage() {
  const [tareas, setTareas] = useState<TodoItem[]>([])
  const [error, setError] = useState<string | null>(null)
  const [cargando, setCargando] = useState(true)

  useEffect(() => {
    cargarTareas()
  }, [])

  async function cargarTareas() {
    try {
      setCargando(true)
      const datos = await tareasApi.obtenerTodas()
      setTareas(datos)
      setError(null)
    } catch {
      setError('No se pudieron cargar las tareas.')
    } finally {
      setCargando(false)
    }
  }

  async function manejarCrear(tarea: TodoItemInput) {
    try {
      await tareasApi.crear(tarea)
      await cargarTareas()
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'No se pudo crear la tarea.')
    }
  }

  async function manejarActualizar(id: number, tarea: TodoItemInput) {
    try {
      await tareasApi.actualizar(id, tarea)
      await cargarTareas()
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'No se pudo actualizar la tarea.')
    }
  }

  async function manejarCompletar(id: number) {
    try {
      await tareasApi.completar(id)
      await cargarTareas()
    } catch {
      setError('No se pudo completar la tarea.')
    }
  }

  async function manejarEliminar(id: number) {
    try {
      await tareasApi.eliminar(id)
      await cargarTareas()
    } catch {
      setError('No se pudo eliminar la tarea.')
    }
  }

  return (
    <section>
      <h2>Tareas</h2>

      <TareaForm onGuardar={manejarCrear} />

      {error && <p className="mensaje-error">{error}</p>}

      {cargando ? (
        <p>Cargando tareas…</p>
      ) : tareas.length === 0 ? (
        <p>No hay tareas todavía.</p>
      ) : (
        <ul className="lista">
          {tareas.map((tarea) => (
            <TareaItem
              key={tarea.id}
              tarea={tarea}
              onCompletar={manejarCompletar}
              onActualizar={manejarActualizar}
              onEliminar={manejarEliminar}
            />
          ))}
        </ul>
      )}
    </section>
  )
}
