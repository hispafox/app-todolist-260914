import { useEffect, useState } from 'react'
import { tareasApi } from '../services/tareasApi'
import { categoriasApi } from '../services/categoriasApi'
import { ApiError } from '../services/api'
import type { Categoria, TodoItem, TodoItemInput } from '../types'
import { TareaForm } from '../components/TareaForm'
import { TareaItem } from '../components/TareaItem'

export function TareasPage() {
  const [tareas, setTareas] = useState<TodoItem[]>([])
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [error, setError] = useState<string | null>(null)
  const [cargando, setCargando] = useState(true)

  useEffect(() => {
    cargarDatos()
  }, [])

  async function cargarDatos() {
    try {
      setCargando(true)
      const [datosTareas, datosCategorias] = await Promise.all([
        tareasApi.obtenerTodas(),
        categoriasApi.obtenerTodas(),
      ])
      setTareas(datosTareas)
      setCategorias(datosCategorias)
      setError(null)
    } catch {
      setError('No se pudieron cargar las tareas y categorías.')
    } finally {
      setCargando(false)
    }
  }

  async function manejarCrear(tarea: TodoItemInput) {
    try {
      await tareasApi.crear(tarea)
      await cargarDatos()
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'No se pudo crear la tarea.')
    }
  }

  async function manejarActualizar(id: number, tarea: TodoItemInput) {
    try {
      await tareasApi.actualizar(id, tarea)
      await cargarDatos()
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'No se pudo actualizar la tarea.')
    }
  }

  async function manejarCompletar(id: number) {
    try {
      await tareasApi.completar(id)
      await cargarDatos()
    } catch {
      setError('No se pudo completar la tarea.')
    }
  }

  async function manejarEliminar(id: number) {
    try {
      await tareasApi.eliminar(id)
      await cargarDatos()
    } catch {
      setError('No se pudo eliminar la tarea.')
    }
  }

  return (
    <section>
      <h2>Tareas</h2>

      <TareaForm categorias={categorias} onGuardar={manejarCrear} />

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
              categorias={categorias}
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
