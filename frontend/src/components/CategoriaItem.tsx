import type { Categoria } from '../types'

interface CategoriaItemProps {
  categoria: Categoria
  onEditar: (categoria: Categoria) => void
}

export function CategoriaItem({ categoria, onEditar }: CategoriaItemProps) {
  return (
    <li className="tarjeta">
      <div className="tarjeta-info">
        <strong>{categoria.nombre}</strong>
      </div>
      <span
        className="categoria-color"
        style={{ backgroundColor: categoria.color }}
        aria-label={`Color: ${categoria.color}`}
        title={categoria.color}
      />
      <div className="tarjeta-acciones">
        <button className="boton boton-secundario" onClick={() => onEditar(categoria)}>
          Editar
        </button>
      </div>
    </li>
  )
}
