import { useState } from 'react'
import { ETIQUETAS_RECURRENCIA, TipoRecurrencia, type TodoItemInput } from '../types'

interface TareaFormProps {
  valorInicial?: TodoItemInput
  onGuardar: (tarea: TodoItemInput) => void
  onCancelar?: () => void
}

const valorPorDefecto: TodoItemInput = {
  title: '',
  isCompleted: false,
  esRepetitiva: false,
  recurrencia: null,
}

export function TareaForm({ valorInicial, onGuardar, onCancelar }: TareaFormProps) {
  const [title, setTitle] = useState(valorInicial?.title ?? valorPorDefecto.title)
  const [esRepetitiva, setEsRepetitiva] = useState(valorInicial?.esRepetitiva ?? false)
  const [recurrencia, setRecurrencia] = useState<TipoRecurrencia | null>(
    valorInicial?.recurrencia ?? null,
  )

  function handleSubmit(evento: React.FormEvent) {
    evento.preventDefault()
    if (!title.trim()) {
      return
    }

    onGuardar({
      title: title.trim(),
      isCompleted: valorInicial?.isCompleted ?? false,
      esRepetitiva,
      recurrencia: esRepetitiva ? recurrencia : null,
    })
  }

  return (
    <form className="formulario" onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Título de la tarea"
        value={title}
        onChange={(evento) => setTitle(evento.target.value)}
      />

      <label className="formulario-check">
        <input
          type="checkbox"
          checked={esRepetitiva}
          onChange={(evento) => setEsRepetitiva(evento.target.checked)}
        />
        Es repetitiva
      </label>

      {esRepetitiva && (
        <select
          value={recurrencia ?? ''}
          onChange={(evento) => setRecurrencia(Number(evento.target.value) as TipoRecurrencia)}
        >
          <option value="" disabled>
            Selecciona recurrencia
          </option>
          {Object.entries(ETIQUETAS_RECURRENCIA).map(([valor, etiqueta]) => (
            <option key={valor} value={valor}>
              {etiqueta}
            </option>
          ))}
        </select>
      )}

      <div className="formulario-acciones">
        <button type="submit" className="boton boton-primario">
          Guardar
        </button>
        {onCancelar && (
          <button type="button" className="boton boton-secundario" onClick={onCancelar}>
            Cancelar
          </button>
        )}
      </div>
    </form>
  )
}
