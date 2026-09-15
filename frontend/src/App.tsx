import { useState } from 'react'
import { TareasPage } from './pages/TareasPage'
import { PlantillasPage } from './pages/PlantillasPage'
import logo from './assets/logo.svg'
import './App.css'

type Pestana = 'tareas' | 'plantillas'

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
        </nav>
      </header>

      <main>{pestana === 'tareas' ? <TareasPage /> : <PlantillasPage />}</main>
    </div>
  )
}

export default App
