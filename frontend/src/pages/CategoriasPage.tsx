import { useEffect, useState } from 'react'
import { categoriasApi } from '../services/categoriasApi'
import type { Categoria } from '../types'
import { CategoriaItem } from '../components/CategoriaItem'

export function CategoriasPage() {
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [error, setError] = useState<string | null>(null)
  const [cargando, setCargando] = useState(true)

  useEffect(() => {
    cargarCategorias()
  }, [])

  async function cargarCategorias() {
    try {
      setCargando(true)
      const datos = await categoriasApi.obtenerTodas()
      setCategorias(datos)
      setError(null)
    } catch {
      setError('No se pudieron cargar las categorías.')
    } finally {
      setCargando(false)
    }
  }

  return (
    <section>
      <h2>Categorías</h2>

      {error && <p className="mensaje-error">{error}</p>}

      {cargando ? (
        <p>Cargando categorías…</p>
      ) : categorias.length === 0 ? (
        <p>No hay categorías todavía.</p>
      ) : (
        <ul className="lista">
          {categorias.map((categoria) => (
            <CategoriaItem key={categoria.id} categoria={categoria} />
          ))}
        </ul>
      )}
    </section>
  )
}
