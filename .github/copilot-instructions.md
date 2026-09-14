# Instrucciones para Copilot

## Proyecto
Aplicación web CRUD de gestión de tareas personales como demo didáctica del curso de GitHub Copilot.
Permite crear, consultar, actualizar y eliminar tareas, con soporte de plantillas reutilizables y tareas repetitivas con generación automática de la siguiente ocurrencia al completar.
La prioridad es la claridad del código sobre la sofisticación arquitectónica.

## Stack tecnológico
- ASP.NET Core 10
- Entity Framework Core 10
- SQLite
- xUnit + Moq
- C# moderno con .NET 10

## Arquitectura de capas
- Estructura plana y legible, sin patrones complejos (sin CQRS, sin Mediator, sin arquitectura hexagonal).
- Capa de modelos: `Models/`
- Capa de acceso a datos: `Data/`
- Capa de lógica de negocio: `Services/`
- Capa API / controladores: `Controllers/`
- Capa de tests: `Tests/`

Estructura recomendada:
- `Models/` → entidades del negocio (`TodoItem`, `PlantillaTarea`, `TipoRecurrencia`)
- `Data/` → `DbContext` y configuración de EF Core
- `Services/` → `ITodoService` + `TodoService`, `IPlantillaService` + `PlantillaService`
- `Controllers/` → orquestación HTTP sin lógica de negocio
- `Tests/` → pruebas unitarias separadas del proyecto principal

## Principios de diseño
- Inyección de dependencias por constructor; nunca `new` directo de servicios.
- `async/await` en todos los métodos que acceden a base de datos.
- Los controladores solo orquestan; no deben implementar lógica de negocio.
- La lógica de negocio vive en servicios.
- Mantener una solución simple y explícita; no introducir abstracciones complejas si no son necesarias.
- No mezclar responsabilidades entre capas.
- Priorizar legibilidad sobre sofisticación.

## Modelado de dominio
- `TodoItem` representa una tarea con sus propiedades principales: `Id`, `Title`, `IsCompleted`, `CreatedAt`, `EsRepetitiva`, `Recurrencia`, `ProximaFecha`, `PlantillaId`.
- `PlantillaTarea` es una entidad independiente que permite generar tareas con valores predefinidos.
- `TipoRecurrencia` es un enum con los valores `Diaria`, `Semanal` y `Mensual`.
- La FK `PlantillaId` debe ser nullable para permitir tareas creadas manualmente sin plantilla.
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
- No se incluye autenticación ni paginación en el alcance inicial.
- No se incluye lógica de filtros o búsqueda avanzados sin petición explícita.
- No se asume un frontend concreto; si se añade más adelante, debe hacerse como capa separada y no redefinir el backend.

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
2. Definir el modelo o la entidad necesaria.
3. Añadir o actualizar el `DbContext` y la configuración de EF Core.
4. Implementar la lógica de negocio en servicios.
5. Crear o actualizar el controlador REST asociado.
6. Validar la API y la persistencia con pruebas relevantes.

## Regla final
Cuando haya dudas sobre arquitectura, diseño o persistencia, prioriza una solución simple, explícita y alineada con ASP.NET Core 10 + Controllers + Entity Framework + SQLite, sin mezclar responsabilidades ni introducir complejidad innecesaria. No conviertas el proyecto en un diseño más complejo que el definido en el análisis; el objetivo es una demo clara, mantenible y fácil de seguir.
