import { useEffect, useState } from 'react'
import { categoriasApi } from '../services/categoriasApi'
import { ApiError } from '../services/api'
import type { Categoria, CategoriaInput } from '../types'
import { CategoriaForm } from '../components/CategoriaForm'
import { CategoriaItem } from '../components/CategoriaItem'

export function CategoriasPage() {
  const [categorias, setCategorias] = useState<Categoria[]>([])
  const [error, setError] = useState<string | null>(null)
  const [cargando, setCargando] = useState(true)
  const [categoriaEditando, setCategoriaEditando] = useState<Categoria | null>(null)

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

  async function manejarGuardar(categoria: CategoriaInput) {
    try {
      if (categoriaEditando) {
        const actualizada = await categoriasApi.actualizar(categoriaEditando.id, categoria)
        setCategorias((actuales) =>
          actuales.map((c) => (c.id === actualizada.id ? actualizada : c)),
        )
        setCategoriaEditando(null)
      } else {
        const creada = await categoriasApi.crear(categoria)
        setCategorias((actuales) => [...actuales, creada])
      }
      setError(null)
    } catch (err) {
      setError(err instanceof ApiError ? err.message : 'No se pudo guardar la categoría.')
    }
  }

  function manejarEditarClick(categoria: Categoria) {
    setCategoriaEditando(categoria)
  }

  function manejarCancelarEdicion() {
    setCategoriaEditando(null)
  }

  return (
    <section>
      <h2>Categorías</h2>

      <CategoriaForm
        onGuardar={manejarGuardar}
        categoriaEnEdicion={categoriaEditando}
        onCancelar={manejarCancelarEdicion}
      />

      {error && <p className="mensaje-error">{error}</p>}

      {cargando ? (
        <p>Cargando categorías…</p>
      ) : categorias.length === 0 ? (
        <p>No hay categorías todavía.</p>
      ) : (
        <ul className="lista">
          {categorias.map((categoria) => (
            <CategoriaItem key={categoria.id} categoria={categoria} onEditar={manejarEditarClick} />
          ))}
        </ul>
      )}
    </section>
  )
}
