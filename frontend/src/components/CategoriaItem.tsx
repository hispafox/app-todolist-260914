import type { Categoria } from '../types'

interface CategoriaItemProps {
  categoria: Categoria
}

export function CategoriaItem({ categoria }: CategoriaItemProps) {
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
    </li>
  )
}
