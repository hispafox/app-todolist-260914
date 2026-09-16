import { useState } from 'react'
import { ETIQUETAS_RECURRENCIA, type Categoria, type TodoItem, type TodoItemInput } from '../types'
import { TareaForm } from './TareaForm'

interface TareaItemProps {
  tarea: TodoItem
  categorias: Categoria[]
  onCompletar: (id: number) => void
  onActualizar: (id: number, tarea: TodoItemInput) => void
  onEliminar: (id: number) => void
}

export function TareaItem({ tarea, categorias, onCompletar, onActualizar, onEliminar }: TareaItemProps) {
  const [editando, setEditando] = useState(false)

  if (editando) {
    return (
      <li className="tarjeta">
        <TareaForm
          valorInicial={{
            titulo: tarea.titulo,
            completada: tarea.completada,
            esRepetitiva: tarea.esRepetitiva,
            recurrencia: tarea.recurrencia,
            categoriaId: tarea.categoriaId,
          }}
          categorias={categorias}
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
    <li className={`tarjeta${tarea.completada ? ' tarjeta-completada' : ''}`}>
      <div className="tarjeta-info">
        <strong>{tarea.titulo}</strong>
        {tarea.categoria && (
          <span
            className="etiqueta"
            style={{ backgroundColor: tarea.categoria.color, color: '#fff', borderColor: tarea.categoria.color }}
          >
            {tarea.categoria.nombre}
          </span>
        )}
        {tarea.esRepetitiva && tarea.recurrencia !== null && (
          <span className="etiqueta">Repetitiva · {ETIQUETAS_RECURRENCIA[tarea.recurrencia]}</span>
        )}
        {tarea.completada && <span className="etiqueta etiqueta-completada">Completada</span>}
      </div>

      <div className="tarjeta-acciones">
        {!tarea.completada && (
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
