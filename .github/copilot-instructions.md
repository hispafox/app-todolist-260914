# Instrucciones para Copilot

## Proyecto
Aplicación web CRUD de gestión de tareas personales como demo didáctica del curso de GitHub Copilot.
Permite crear, consultar, actualizar y eliminar tareas, con soporte de plantillas reutilizables y tareas repetitivas con generación automática de la siguiente ocurrencia al completar.
La prioridad es la claridad del código sobre la sofisticación arquitectónica.

## Entorno de ejecución (terminal)
- Ruta completa del workspace: `F:\w\repos\app-todolist-260914`. Usar siempre esta ruta absoluta al ejecutar comandos o abrir archivos; no asumir `C:\` ni la carpeta del usuario.
- El workspace está en la unidad `F:`, fuera del sandbox por defecto del terminal. Cualquier comando de terminal en este repo (git, dotnet, npm, etc.) necesita ejecutarse con `requestUnsandboxedExecution=true`, o fallará con "Access to the path is denied" o "not a git repository" aunque el repositorio exista.
- El terminal sandboxed no conserva el directorio de trabajo entre llamadas: hay que anteponer `cd "F:\w\repos\app-todolist-260914";` (o el subdirectorio correspondiente, p. ej. `AppTodoList.Api` o `frontend`) en cada comando nuevo en lugar de asumir que el `cd` anterior persiste.
- Usar `git --no-pager` para evitar que `git diff` / `git log` invoquen `less.exe`, que falla en este entorno (Git for Windows + MSYS).
- Backend: ejecutar desde `AppTodoList.Api/` (`dotnet run --launch-profile https`, disponible en `https://localhost:5001`).
- Frontend: ejecutar desde `frontend/` (`npm run dev`, disponible en `http://localhost:5173`).
- Python del sistema: `C:\Users\hispa\AppData\Local\Python\bin\python.exe`. No usar `python` ni `python3` a secas: resuelven al stub de Microsoft Store y no funcionan.

## Fuente de verdad y soporte
- El documento de análisis y diseño en `docs/analisis-diseño.md` es la fuente de verdad del dominio, requisitos funcionales, endpoints y modelo de datos.
- El archivo `.github/copilot-instructions.md` no sustituye al análisis; actúa como guía operativa para que Copilot mantenga la estructura, el estilo y la separación de responsabilidades del proyecto.
- Cuando haya duda sobre requisitos, prioridad o diseño, se debe seguir primero el análisis y la documentación del proyecto, y las instrucciones de Copilot deben reforzar esa decisión, no contradecirla.
- No duplicar reglas funcionales en ambos sitios; el análisis define “qué” debe hacer el sistema y las instrucciones definen “cómo” debe construirse sin romper la arquitectura.

## Stack tecnológico
- ASP.NET Core 10
- Entity Framework Core 10
- SQLite
- React + Vite + TypeScript para la capa cliente
- xUnit + Moq
- C# moderno con .NET 10

## Arquitectura de capas
- Estructura plana y legible, sin patrones complejos (sin CQRS, sin Mediator, sin arquitectura hexagonal).
- Capa de modelos: `Models/`
- Capa de acceso a datos: `Data/`
- Capa de lógica de negocio: `Services/`
- Capa API / controladores: `Controllers/`
- Capa cliente: `frontend/` con React + Vite + TypeScript
- Capa de tests: `Tests/`

Estructura recomendada:
- `Models/` → entidades del negocio (`TodoItem`, `PlantillaTarea`, `Persona`, `TipoRecurrencia`)
- `Data/` → `DbContext` y configuración de EF Core
- `Services/` → `ITodoService` + `TodoService`, `IPlantillaService` + `PlantillaService`
- `Controllers/` → orquestación HTTP sin lógica de negocio
- `frontend/` → aplicación cliente en React + Vite, con `src/`, `components/`, `pages/`, `services/` y `types/`
- `Tests/` → pruebas unitarias separadas del proyecto principal

## Mapa de carpetas y responsabilidades
La separación por capas debe mantenerse siempre. La siguiente estructura explica dónde va cada cosa y por qué:

```text
AppTodoList/
├── Models/
│   ├── TodoItem.cs
│   ├── PlantillaTarea.cs
│   ├── Persona.cs
│   └── TipoRecurrencia.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Services/
│   ├── ITodoService.cs
│   ├── TodoService.cs
│   ├── IPlantillaService.cs
│   └── PlantillaService.cs
│
├── Controllers/
│   ├── TareasController.cs
│   └── PlantillasController.cs
│
├── frontend/
│   ├── src/
│   ├── components/
│   ├── pages/
│   ├── services/
│   └── types/
│
├── Tests/
│   └── ... pruebas unitarias ...
│
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

Reglas clave:
- `Models/` define el dominio: qué es una tarea y una plantilla.
- `Data/` define cómo se guardan y consultan esas entidades en SQLite.
- `Services/` contiene la lógica de negocio: creación, actualización, eliminación, recurrencia y generación de la siguiente ocurrencia.
- `Controllers/` solo orquesta la petición HTTP y delega en servicios; no debe resolver reglas de negocio.
- `frontend/` consume la API REST y solo aporta UX; no toca la base de datos ni duplica la lógica del backend.
- `Tests/` valida el comportamiento real de la aplicación con pruebas unitarias de servicios y validación.

## Principios de diseño
- Inyección de dependencias por constructor; nunca `new` directo de servicios.
- `async/await` en todos los métodos que acceden a base de datos.
- Los controladores solo orquestan; no deben implementar lógica de negocio.
- La lógica de negocio vive en servicios.
- Mantener una solución simple y explícita; no introducir abstracciones complejas si no son necesarias.
- No mezclar responsabilidades entre capas.
- Priorizar legibilidad sobre sofisticación.

## Modelado de dominio
- `TodoItem` representa una tarea con sus propiedades principales: `Id`, `Title`, `IsCompleted`, `CreatedAt`, `EsRepetitiva`, `Recurrencia`, `ProximaFecha`, `PlantillaId`, `PersonaId`.
- `PlantillaTarea` es una entidad independiente que permite generar tareas con valores predefinidos.
- `Persona` representa a la persona responsable de una tarea y puede asociarse a varias tareas.
- `TipoRecurrencia` es un enum con los valores `Diaria`, `Semanal` y `Mensual`.
- La FK `PlantillaId` debe ser nullable para permitir tareas creadas manualmente sin plantilla.
- La FK `PersonaId` debe ser nullable para permitir tareas sin persona asignada.
- Los nombres y reglas de negocio deben ser claros y estar alineados con el dominio de la aplicación.

## Base de datos y EF Core
- Usar `DbContext` como punto central de acceso a SQLite.
- Configurar la conexión en `appsettings.json` o mediante configuración de entorno.
- Usar migraciones de EF Core cuando cambie el esquema.
- Evitar manipular la base de datos manualmente si existe una migración disponible.
- No borrar archivos `.db` ni la base de datos sin confirmación explícita.
- Preferir `CREATE TABLE IF NOT EXISTS`, `ALTER TABLE` o migraciones a recrear la base de datos a mano.
- Mantener la persistencia simple, sin infraestructura externa ni complejidad innecesaria.

## API REST
El proyecto debe basarse en Controllers para exponer una API REST clara y explícita.

### Tareas — `/api/tareas`
- `GET /api/tareas` → listar todas las tareas
- `GET /api/tareas/{id}` → obtener tarea por ID
- `POST /api/tareas` → crear una tarea
- `PUT /api/tareas/{id}` → actualizar título o estado
- `DELETE /api/tareas/{id}` → eliminar tarea
- `POST /api/tareas/{id}/completar` → marcar como completada; si es repetitiva, genera la siguiente ocurrencia

### Plantillas — `/api/plantillas`
- `GET /api/plantillas` → listar plantillas
- `GET /api/plantillas/{id}` → obtener plantilla por ID
- `POST /api/plantillas` → crear una plantilla
- `PUT /api/plantillas/{id}` → actualizar una plantilla
- `DELETE /api/plantillas/{id}` → eliminar una plantilla
- `POST /api/plantillas/{id}/instanciar` → crear una tarea desde la plantilla

## Decisiones de diseño que deben mantenerse
- `SQLite` sobre SQL Server por ser una base de datos embebida y adecuada para una demo local.
- `Controllers` sobre Minimal API porque son más explícitos y fáciles de leer en pantalla.
- `ITodoService` e `IPlantillaService` como contratos para facilitar pruebas y sustituciones.
- La lógica de recurrencia debe ir en el servicio, no en el controlador ni en el modelo.
- `POST /completar` debe usarse para completar una tarea repetitiva porque tiene efecto secundario: generar la siguiente ocurrencia.
- `PlantillaTarea` debe ser una entidad independiente, no un campo embebido dentro de `TodoItem`.
- La capa cliente será una SPA en `React + Vite + TypeScript`, separada del backend y consumiendo la API REST.
- El frontend no debe tocar la base de datos ni duplicar la lógica de negocio; debe orquestar llamadas a `Controllers` del backend.
- No se incluye autenticación ni paginación en el alcance inicial.
- No se incluye lógica de filtros o búsqueda avanzados sin petición explícita.
- La separación entre backend y frontend debe mantenerse clara: el backend resuelve dominio y persistencia; el cliente solo aporta UX y consumo de API.

## Reglas funcionales
- Leer y seguir los requisitos funcionales del proyecto antes de implementar nuevas características.
- Revisar la documentación en `docs/` antes de escribir código nuevo.
- Mantener cambios pequeños y dirigidos a la necesidad exacta.
- No añadir funcionalidades extra sin que se soliciten explícitamente.
- Si una tarea es repetitiva, la siguiente ocurrencia debe calcularse en el servicio al completarla.
- Mantener la lógica de plantillas y recurrencia consistente con el análisis de diseño.

## Validación y UX
- Validar campos requeridos como título y descripción cuando aplique.
- Usar validación del modelo con `DataAnnotations` o validación de negocio consistente.
- Dar mensajes de error claros y amigables.
- Mantener la experiencia de usuario consistente para crear, editar, completar y eliminar tareas.

## Manejo de errores
- No ocultar errores críticos con `try/catch` vacíos.
- Manejar errores de acceso a datos, validación y configuración con mensajes apropiados.
- Registrar la información relevante en caso de excepción.

## Testing
- Cuando se agregue una funcionalidad, incluir pruebas adecuadas.
- Preferir pruebas unitarias para servicios y validación, y pruebas de integración cuando la funcionalidad dependa de EF Core o controllers.
- Probar casos normales y casos límite.
- No crear pruebas que solo validen mocks sin verificar el comportamiento real del sistema.
- Usar xUnit + Moq como stack de pruebas del proyecto.

## Estilo de contribución
- Mantener las modificaciones enfocadas en la tarea solicitada.
- No refactorizar código ajeno a la necesidad del cambio.
- Mantener el nombre de archivos, namespaces y estructura del proyecto consistentes.
- Asegurar que el código compile sin errores y siga la convención del repositorio.

## Resumen de implementación ideal
Un cambio típico en este proyecto debe seguir este flujo:
1. Revisar requisitos y estructura existente.
2. Registrar la petición en `docs/analisis-diseño.md` mediante el skill de análisis de petición.
3. Que el orquestador determine el alcance real y la capa adecuada.
4. Definir el modelo o la entidad necesaria solo si corresponde al cambio.
5. Añadir o actualizar el `DbContext` y la configuración de EF Core solo cuando la persistencia esté justificada.
6. Implementar la lógica de negocio en servicios cuando la regla de negocio lo requiera.
7. Crear o actualizar el controlador REST asociado solo si la funcionalidad necesita exposición HTTP.
8. Validar la API y la persistencia con pruebas relevantes.

## Regla de coordinación del flujo
- El orquestador es el director del cambio: decide el alcance, la secuencia y la delegación.
- El orquestador no sustituye a la capa que implementa la solución; coordina la ejecución y evita scope creep.
- La documentación debe preceder a la implementación.
- El cambio solo debe expandirse a las capas necesarias: dominio, servicios, Data, controllers o frontend.
- Cuando no exista un requisito claro, el alcance debe mantenerse mínimo y explícito.
- La tarea no debe pasar a una capa superior si la necesidad real está en otra; la coordinación evita generar código extra de forma automática.

## Regla final
Cuando haya dudas sobre arquitectura, diseño o persistencia, prioriza una solución simple, explícita y alineada con ASP.NET Core 10 + Controllers + Entity Framework + SQLite, y con un cliente frontend separado en React + Vite + TypeScript. No mezcles responsabilidades ni conviertas el proyecto en un diseño más complejo que el definido en el análisis; el objetivo es una demo clara, mantenible y fácil de seguir.

## Humanizer local del proyecto
- Cuando redactes, revises o pulires texto de documentación, comentarios, descripciones, README, mensajes o cambios de código en este repositorio, usa el skill local `humanizer` ubicado en `.github/skills/humanizer/SKILL.md`.
- El objetivo es quitar señales de escritura artificial y dejar el texto con un tono humano, claro y natural sin cambiar el contenido ni los hechos.
- Este comportamiento debe quedar limitado a este proyecto y no debe depender de instalaciones globales del perfil del usuario.
