import { useState } from 'react'
import { ETIQUETAS_RECURRENCIA, TipoRecurrencia, type Categoria, type TodoItemInput } from '../types'

interface TareaFormProps {
  valorInicial?: TodoItemInput
  categorias?: Categoria[]
  onGuardar: (tarea: TodoItemInput) => void
  onCancelar?: () => void
}

const valorPorDefecto: TodoItemInput = {
  titulo: '',
  completada: false,
  esRepetitiva: false,
  recurrencia: null,
  categoriaId: null,
}

export function TareaForm({ valorInicial, categorias = [], onGuardar, onCancelar }: TareaFormProps) {
  const [titulo, setTitulo] = useState(valorInicial?.titulo ?? valorPorDefecto.titulo)
  const [esRepetitiva, setEsRepetitiva] = useState(valorInicial?.esRepetitiva ?? false)
  const [recurrencia, setRecurrencia] = useState<TipoRecurrencia | null>(
    valorInicial?.recurrencia ?? null,
  )
  const [categoriaId, setCategoriaId] = useState<number | null>(valorInicial?.categoriaId ?? null)

  function handleSubmit(evento: React.FormEvent) {
    evento.preventDefault()
    if (!titulo.trim()) {
      return
    }

    onGuardar({
      titulo: titulo.trim(),
      completada: valorInicial?.completada ?? false,
      esRepetitiva,
      recurrencia: esRepetitiva ? recurrencia : null,
      categoriaId,
    })
  }

  return (
    <form className="formulario" onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Título de la tarea"
        value={titulo}
        onChange={(evento) => setTitulo(evento.target.value)}
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

      <select
        value={categoriaId ?? ''}
        onChange={(evento) => setCategoriaId(evento.target.value === '' ? null : Number(evento.target.value))}
      >
        <option value="">Sin categoría</option>
        {categorias.map((categoria) => (
          <option key={categoria.id} value={categoria.id}>
            {categoria.nombre}
          </option>
        ))}
      </select>

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
