import { useState } from 'react'
import { ETIQUETAS_RECURRENCIA, type TodoItem, type TodoItemInput } from '../types'
import { TareaForm } from './TareaForm'

interface TareaItemProps {
  tarea: TodoItem
  onCompletar: (id: number) => void
  onActualizar: (id: number, tarea: TodoItemInput) => void
  onEliminar: (id: number) => void
}

export function TareaItem({ tarea, onCompletar, onActualizar, onEliminar }: TareaItemProps) {
  const [editando, setEditando] = useState(false)

  if (editando) {
    return (
      <li className="tarjeta">
        <TareaForm
          valorInicial={{
            title: tarea.title,
            isCompleted: tarea.isCompleted,
            esRepetitiva: tarea.esRepetitiva,
            recurrencia: tarea.recurrencia,
          }}
          onGuardar={(datos) => {
            onActualizar(tarea.id, datos)
            setEditando(false)
          }}
          onCancelar={() => setEditando(false)}
        />
      </li>
    )
  }

  return (
    <li className={`tarjeta${tarea.isCompleted ? ' tarjeta-completada' : ''}`}>
      <div className="tarjeta-info">
        <strong>{tarea.title}</strong>
        {tarea.esRepetitiva && tarea.recurrencia !== null && (
          <span className="etiqueta">Repetitiva · {ETIQUETAS_RECURRENCIA[tarea.recurrencia]}</span>
        )}
        {tarea.isCompleted && <span className="etiqueta etiqueta-completada">Completada</span>}
      </div>

      <div className="tarjeta-acciones">
        {!tarea.isCompleted && (
          <button className="boton boton-primario" onClick={() => onCompletar(tarea.id)}>
            Completar
          </button>
        )}
        <button className="boton boton-secundario" onClick={() => setEditando(true)}>
          Editar
        </button>
        <button className="boton boton-peligro" onClick={() => onEliminar(tarea.id)}>
          Eliminar
        </button>
      </div>
    </li>
  )
}
