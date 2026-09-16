import { useState } from 'react'
import { TareasPage } from './pages/TareasPage'
import { PlantillasPage } from './pages/PlantillasPage'
import { CategoriasPage } from './pages/CategoriasPage'
import logo from './assets/logo.svg'
import './App.css'

type Pestana = 'tareas' | 'plantillas' | 'categorias'

function App() {
  const [pestana, setPestana] = useState<Pestana>('tareas')

  return (
    <div className="app">
      <header className="app-header">
        <div className="app-brand">
          <img className="app-logo" src={logo} alt="" width={40} height={40} />
          <h1>App Todo List</h1>
        </div>
        <nav className="app-nav">
          <button
            className={pestana === 'tareas' ? 'activo' : ''}
            onClick={() => setPestana('tareas')}
          >
            Tareas
          </button>
          <button
            className={pestana === 'plantillas' ? 'activo' : ''}
            onClick={() => setPestana('plantillas')}
          >
            Plantillas
          </button>
          <button
            className={pestana === 'categorias' ? 'activo' : ''}
            onClick={() => setPestana('categorias')}
          >
            Categorías
          </button>
        </nav>
      </header>

      <main>
        {pestana === 'tareas' && <TareasPage />}
        {pestana === 'plantillas' && <PlantillasPage />}
        {pestana === 'categorias' && <CategoriasPage />}
      </main>
    </div>
  )
}

export default App
