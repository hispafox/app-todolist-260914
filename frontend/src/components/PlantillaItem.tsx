import { useState } from 'react'
import { ETIQUETAS_RECURRENCIA, type PlantillaTarea, type PlantillaTareaInput } from '../types'
import { PlantillaForm } from './PlantillaForm'

interface PlantillaItemProps {
  plantilla: PlantillaTarea
  onActualizar: (id: number, plantilla: PlantillaTareaInput) => void
  onEliminar: (id: number) => void
  onInstanciar: (id: number) => void
}

export function PlantillaItem({
  plantilla,
  onActualizar,
  onEliminar,
  onInstanciar,
}: PlantillaItemProps) {
  const [editando, setEditando] = useState(false)

  if (editando) {
    return (
      <li className="tarjeta">
        <PlantillaForm
          valorInicial={{
            titulo: plantilla.titulo,
            esRepetitiva: plantilla.esRepetitiva,
            recurrencia: plantilla.recurrencia,
          }}
          onGuardar={(datos) => {
            onActualizar(plantilla.id, datos)
            setEditando(false)
          }}
          onCancelar={() => setEditando(false)}
        />
      </li>
    )
  }

  return (
    <li className="tarjeta">
      <div className="tarjeta-info">
        <strong>{plantilla.titulo}</strong>
        {plantilla.esRepetitiva && plantilla.recurrencia !== null && (
          <span className="etiqueta">
            Repetitiva · {ETIQUETAS_RECURRENCIA[plantilla.recurrencia]}
          </span>
        )}
      </div>

      <div className="tarjeta-acciones">
        <button className="boton boton-primario" onClick={() => onInstanciar(plantilla.id)}>
          Crear tarea
        </button>
        <button className="boton boton-secundario" onClick={() => setEditando(true)}>
          Editar
        </button>
        <button className="boton boton-peligro" onClick={() => onEliminar(plantilla.id)}>
          Eliminar
        </button>
      </div>
    </li>
  )
}
