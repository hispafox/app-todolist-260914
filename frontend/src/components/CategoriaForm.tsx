import { useEffect, useState } from 'react'
import type { Categoria, CategoriaInput } from '../types'

interface CategoriaFormProps {
  onGuardar: (categoria: CategoriaInput) => void
  categoriaEnEdicion?: Categoria | null
  onCancelar?: () => void
}

export function CategoriaForm({ onGuardar, categoriaEnEdicion, onCancelar }: CategoriaFormProps) {
  const [nombre, setNombre] = useState('')
  const [color, setColor] = useState('#2563eb')

  useEffect(() => {
    if (categoriaEnEdicion) {
      setNombre(categoriaEnEdicion.nombre)
      setColor(categoriaEnEdicion.color)
    } else {
      setNombre('')
      setColor('#2563eb')
    }
  }, [categoriaEnEdicion])

  function handleSubmit(evento: React.FormEvent) {
    evento.preventDefault()
    if (!nombre.trim()) {
      return
    }

    onGuardar({ nombre: nombre.trim(), color })
    if (!categoriaEnEdicion) {
      setNombre('')
      setColor('#2563eb')
    }
  }

  return (
    <form className="formulario" onSubmit={handleSubmit}>
      <label htmlFor="categoria-nombre">Nombre</label>
      <input
        id="categoria-nombre"
        type="text"
        placeholder="Nombre de la categoría"
        value={nombre}
        onChange={(evento) => setNombre(evento.target.value)}
        maxLength={100}
      />

      <label htmlFor="categoria-color">Color</label>
      <input
        id="categoria-color"
        type="color"
        value={color}
        onChange={(evento) => setColor(evento.target.value)}
      />

      <div className="formulario-acciones">
        <button type="submit" className="boton boton-primario">
          {categoriaEnEdicion ? 'Actualizar' : 'Guardar'}
        </button>
        {categoriaEnEdicion && onCancelar && (
          <button type="button" className="boton boton-secundario" onClick={onCancelar}>
            Cancelar
          </button>
        )}
      </div>
    </form>
  )
}
