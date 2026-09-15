import { useState } from 'react'
import { ETIQUETAS_RECURRENCIA, TipoRecurrencia, type PlantillaTareaInput } from '../types'

interface PlantillaFormProps {
  valorInicial?: PlantillaTareaInput
  onGuardar: (plantilla: PlantillaTareaInput) => void
  onCancelar?: () => void
}

export function PlantillaForm({ valorInicial, onGuardar, onCancelar }: PlantillaFormProps) {
  const [titulo, setTitulo] = useState(valorInicial?.titulo ?? '')
  const [esRepetitiva, setEsRepetitiva] = useState(valorInicial?.esRepetitiva ?? false)
  const [recurrencia, setRecurrencia] = useState<TipoRecurrencia | null>(
    valorInicial?.recurrencia ?? null,
  )

  function handleSubmit(evento: React.FormEvent) {
    evento.preventDefault()
    if (!titulo.trim()) {
      return
    }

    onGuardar({
      titulo: titulo.trim(),
      esRepetitiva,
      recurrencia: esRepetitiva ? recurrencia : null,
    })
  }

  return (
    <form className="formulario" onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Título de la plantilla"
        value={titulo}
        onChange={(evento) => setTitulo(evento.target.value)}
      />

      <label className="formulario-check">
        <input
          type="checkbox"
          checked={esRepetitiva}
          onChange={(evento) => setEsRepetitiva(evento.target.checked)}
        />
        Genera tareas repetitivas
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
