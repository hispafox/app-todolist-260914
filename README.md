# App Todo List

Aplicación web de gestión de tareas personales desarrollada como demo didáctica para el curso de GitHub Copilot. El objetivo principal es mantener una solución clara y fácil de seguir, con separación de responsabilidades entre dominio, servicios, acceso a datos y capa API.

La aplicación permite crear, consultar, actualizar y eliminar tareas, además de trabajar con plantillas reutilizables y tareas repetitivas que generan automáticamente la siguiente ocurrencia al completarse.

## Funcionalidades principales

- CRUD completo de tareas.
- Gestión de plantillas reutilizables para crear tareas con valores predefinidos.
- Tareas repetitivas con recurrencia diaria, semanal o mensual.
- Generación automática de la siguiente ocurrencia al completar una tarea repetitiva.
- API REST con controladores explícitos.
- Separación clara entre backend y frontend.
- Tests de servicios y validación con xUnit + Moq.

## Stack tecnológico

| Tecnología | Uso |
| --- | --- |
| ASP.NET Core 10 | Backend y API REST |
| Entity Framework Core 10 | Persistencia y acceso a datos |
| SQLite | Base de datos embebida |
| React + Vite + TypeScript | Frontend SPA |
| xUnit + Moq | Pruebas unitarias |

## Arquitectura propuesta

El proyecto sigue una estructura simple y legible, organizada por capas:

```text
AppTodoList/
├── Models/
│   ├── TodoItem.cs
│   ├── PlantillaTarea.cs
│   └── TipoRecurrencia.cs
├── Data/
│   └── AppDbContext.cs
├── Services/
│   ├── ITodoService.cs
│   ├── TodoService.cs
│   ├── IPlantillaService.cs
│   └── PlantillaService.cs
├── Controllers/
│   ├── TareasController.cs
│   └── PlantillasController.cs
├── frontend/
│   ├── src/
│   ├── components/
│   ├── pages/
│   ├── services/
│   └── types/
├── Tests/
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── README.md
```

### Responsabilidades por capa

- Models: entidades del dominio.
- Data: configuración de EF Core y acceso a SQLite.
- Services: lógica de negocio, validaciones y recurrencia.
- Controllers: orquestación HTTP y delegación al servicio.
- frontend: cliente React que consume la API REST.
- Tests: validación del comportamiento real de la aplicación.

## Modelo de dominio

### TodoItem

Representa una tarea personal con sus atributos principales:

- Id
- Title
- IsCompleted
- CreatedAt
- EsRepetitiva
- Recurrencia
- ProximaFecha
- PlantillaId
- PersonaId

### PlantillaTarea

Entidad reutilizable para generar tareas con valores predefinidos.

### Persona

Entidad auxiliar que representa a la persona responsable de una tarea. El dominio permite asignar una tarea concreta a una persona, y esa relación queda representada con `PersonaId` en `TodoItem`.

### TipoRecurrencia

Enum con los valores:

- Diaria
- Semanal
- Mensual

## Regla de coordinación del flujo

La documentación del proyecto es la fuente de verdad para decidir el alcance real del cambio. Cuando llega una petición, primero se registra en el análisis y luego el orquestador determina si el cambio corresponde solo al dominio o requiere ampliar a servicios, acceso a datos, controladores o frontend.

Este proyecto evita generar capas adicionales por costumbre. El modelo se mantiene estable y solo se amplía cuando el análisis lo exige, con un criterio explícito y justificado antes de implementar.

## API REST

La API se estructura en dos recursos principales:

### Tareas

- GET /api/tareas
- GET /api/tareas/{id}
- POST /api/tareas
- PUT /api/tareas/{id}
- DELETE /api/tareas/{id}
- POST /api/tareas/{id}/completar

### Plantillas

- GET /api/plantillas
- GET /api/plantillas/{id}
- POST /api/plantillas
- PUT /api/plantillas/{id}
- DELETE /api/plantillas/{id}
- POST /api/plantillas/{id}/instanciar

La lógica de recurrencia se mantiene en la capa de servicios, no en el controlador ni en el modelo, tal y como se define en el análisis del proyecto.

## Requisitos previos

- .NET 10 SDK
- Node.js 20+ y npm
- SQLite
- Git

## Puesta en marcha

1. Clona el repositorio.
2. Restaura las dependencias del backend con .NET.
3. Configura la conexión a SQLite en la configuración de la aplicación.
4. Ejecuta la API y valida los endpoints.
5. En el frontend, instala las dependencias con npm y arranca la aplicación Vite.

Ejemplo orientativo de comandos:

```bash
dotnet restore
dotnet build
dotnet run
```

Y para el cliente:

```bash
cd frontend
npm install
npm run dev
```

> La estructura exacta de arranque puede variar según la organización final del proyecto y el nombre del proyecto backend dentro del repositorio.

## Documentación de referencia

La fuente de verdad del dominio y de los requisitos funcionales del proyecto está en:

- [docs/analisis-diseño.md](docs/analisis-diseño.md)

## Objetivo del proyecto

Este repositorio funciona como una demo didáctica para practicar y demostrar cómo usar GitHub Copilot en un flujo de desarrollo real con:

- diseño de dominio,
- arquitectura por capas,
- API REST,
- pruebas automatizadas,
- y frontend separado.

La prioridad es mantener un código claro, legible y alineado con el análisis del negocio, más que introducir complejidad innecesaria.

## Estado del repositorio

Este README describe el propósito general del proyecto y la arquitectura esperada según la documentación de diseño del repositorio. Si se desarrollan nuevos componentes, conviene mantener esta guía sincronizada con la implementación real.
