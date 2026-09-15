import { useEffect, useState } from 'react'
import { plantillasApi } from '../services/plantillasApi'
import { ApiError } from '../services/api'
import type { PlantillaTarea, PlantillaTareaInput } from '../types'
import { PlantillaForm } from '../components/PlantillaForm'
import { PlantillaItem } from '../components/PlantillaItem'

export function PlantillasPage() {
  const [plantillas, setPlantillas] = useState<PlantillaTarea[]>([])
  const [error, setError] = useState<string | null>(null)
  const [mensaje, setMensaje] = useState<string | null>(null)
  const [cargando, setCargando] = useState(true)

  useEffect(() => {
    cargarPlantillas()
  }, [])

  async function cargarPlantillas() {
    try {
      setCargando(true)
      const datos = await plantillasApi.obtenerTodas()
      setPlantillas(datos)
      setError(null)
    } catch {
      setError('No se pudieron cargar las plantillas.')
    } finally {
      setCargando(false)
    }
  }

  async function manejarCrear(plantilla: PlantillaTareaInput) {
    try {
      await plantillasApi.crear(plantilla)
      await cargarPlantillas()
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'No se pudo crear la plantilla.')
    }
  }

  async function manejarActualizar(id: number, plantilla: PlantillaTareaInput) {
    try {
      await plantillasApi.actualizar(id, plantilla)
      await cargarPlantillas()
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'No se pudo actualizar la plantilla.')
    }
  }

  async function manejarEliminar(id: number) {
    try {
      await plantillasApi.eliminar(id)
      await cargarPlantillas()
    } catch {
      setError('No se pudo eliminar la plantilla.')
    }
  }

  async function manejarInstanciar(id: number) {
    try {
      await plantillasApi.instanciar(id)
      setMensaje('Tarea creada a partir de la plantilla. Consúltala en la pestaña Tareas.')
      setError(null)
    } catch {
      setError('No se pudo crear la tarea desde la plantilla.')
    }
  }

  return (
    <section>
      <h2>Plantillas</h2>

      <PlantillaForm onGuardar={manejarCrear} />

      {error && <p className="mensaje-error">{error}</p>}
      {mensaje && <p className="mensaje-info">{mensaje}</p>}

      {cargando ? (
        <p>Cargando plantillas…</p>
      ) : plantillas.length === 0 ? (
        <p>No hay plantillas todavía.</p>
      ) : (
        <ul className="lista">
          {plantillas.map((plantilla) => (
            <PlantillaItem
              key={plantilla.id}
              plantilla={plantilla}
              onActualizar={manejarActualizar}
              onEliminar={manejarEliminar}
              onInstanciar={manejarInstanciar}
            />
          ))}
        </ul>
      )}
    </section>
  )
}
