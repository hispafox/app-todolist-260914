# Plan: Categorizar tareas

> Generado por el agente planificador · 2026-09-16

## 1. Resumen

La aplicación ya persiste tareas y dispone de las entidades `Categoria` y la relación opcional `TodoItem.CategoriaId`, pero no expone las categorías a la interfaz ni permite seleccionarlas. Esta feature habilita la asignación, retirada y visualización de categorías predefinidas en las tareas.

## 2. Requisitos funcionales

1. El usuario puede consultar las categorías disponibles para elegir una al crear o editar una tarea.
2. El usuario puede asignar una categoría opcional a una tarea nueva o existente.
3. El usuario puede dejar una tarea sin categoría.
4. La lista de tareas muestra el nombre y color de la categoría asignada.
5. La API rechaza la creación o actualización de una tarea que referencia una categoría inexistente.

## 3. Cambios en el modelo de datos

### Entidades nuevas o modificadas

| Entidad | Campo | Tipo | Restricciones | Descripción |
|---------|-------|------|---------------|-------------|
| `TodoItem` | `CategoriaId` | `int?` | FK opcional existente a `Categoria` | Identificador de la categoría asignada. No requiere modificación. |
| `TodoItem` | `Categoria` | `Categoria?` | Navegación opcional existente | Permite devolver el nombre y color de la categoría al consultar tareas. No requiere modificación. |
| `Categoria` | `Id`, `Nombre`, `Color` | `int`, `string`, `string` | Entidad existente; `Nombre` requerido, máximo 100; `Color` máximo 30 | Catálogo reutilizado por el selector de tareas. No requiere modificación. |

### Migración necesaria

No. `AppDbContext` ya declara `DbSet<Categoria>`, configura la FK opcional `TodoItem.CategoriaId` con borrado `SetNull` y siembra las categorías `Hogar` y `Trabajo`. La aplicación usa `Database.EnsureCreated()`, no migraciones de EF Core; no se debe recrear ni eliminar la base de datos existente.

## 4. DTOs

### DTOs de entrada

Se amplía el contrato de entrada de tareas para incluir la categoría opcional. El backend actual enlaza directamente con `TodoItem`; al implementar la feature se deben introducir DTOs para no exponer la entidad en la API.

```csharp
public class GuardarTareaDto
{
    public string Titulo { get; set; } = string.Empty;
    public bool Completada { get; set; }
    public bool EsRepetitiva { get; set; }
    public TipoRecurrencia? Recurrencia { get; set; }
    public int? CategoriaId { get; set; }
}
```

### DTOs de salida

```csharp
public class CategoriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}

public class TareaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool Completada { get; set; }
    public int? CategoriaId { get; set; }
    public CategoriaDto? Categoria { get; set; }
}
```

Los DTOs definitivos deben conservar también los campos ya públicos de tarea (`CreatedAt`, recurrencia, plantilla y persona) para no romper el cliente actual.

## 5. Endpoints

| Verbo | Ruta | Cuerpo | Respuesta exitosa | Errores posibles |
|-------|------|--------|-------------------|-----------------|
| GET | `/api/categorias` | — | `200 OK` con `CategoriaDto[]`, ordenado por nombre | — |
| POST | `/api/tareas` | `GuardarTareaDto` con `categoriaId` opcional | `201 Created` con `TareaDto` y categoría expandida | `400` si la categoría no existe o los datos son inválidos |
| PUT | `/api/tareas/{id}` | `GuardarTareaDto` con `categoriaId` opcional | `200 OK` con `TareaDto` y categoría expandida | `400` si la categoría no existe; `404` si no existe la tarea |
| GET | `/api/tareas` | — | `200 OK` con `TareaDto[]`, incluyendo `categoria` cuando esté asignada | — |
| GET | `/api/tareas/{id}` | — | `200 OK` con `TareaDto`, incluyendo `categoria` cuando esté asignada | `404` si no existe |

## 6. Lógica de negocio

- Consultar las categorías con lectura sin seguimiento y ordenarlas por `Nombre`.
- Antes de crear o actualizar una tarea con `CategoriaId`, comprobar que la categoría existe. Si el valor es `null`, eliminar la asignación sin error.
- Mantener las cargas de tareas con `Include(todo => todo.Categoria)` para que las respuestas devuelvan los datos de presentación de la categoría.
- Al completar una tarea repetitiva, conservar `CategoriaId` en la siguiente ocurrencia, comportamiento que ya existe y debe cubrirse con prueba de regresión.
- Los controladores solo traducen HTTP y DTOs; la comprobación de existencia pertenece a la capa de lógica de negocio.

## 7. Capas afectadas

**Crear:**
- `AppTodoList.Api/Dtos/GuardarTareaDto.cs`
- `AppTodoList.Api/Dtos/TareaDto.cs`
- `AppTodoList.Api/Dtos/CategoriaDto.cs`
- `AppTodoList.Api/LogicaNegocio/ICategoriaLogica.cs`
- `AppTodoList.Api/LogicaNegocio/CategoriaLogica.cs`
- `AppTodoList.Api/Services/ICategoriaService.cs`
- `AppTodoList.Api/Services/CategoriaService.cs`
- `AppTodoList.Api/Controllers/CategoriasController.cs`
- `AppTodoList.Api.Tests/Servicios/TodoServiceTests.cs`
- `AppTodoList.Api.Tests/Servicios/CategoriaServiceTests.cs`
- `frontend/src/services/categoriasApi.ts`

**Modificar:**
- `AppTodoList.Api/Services/ITodoService.cs` — aceptar o devolver contratos de tarea con categoría sin exponer entidades de EF Core.
- `AppTodoList.Api/Services/TodoService.cs` — validar `CategoriaId` y mapear la relación en las respuestas.
- `AppTodoList.Api/Controllers/TareasController.cs` — recibir y devolver DTOs de tarea.
- `AppTodoList.Api/Program.cs` — registrar las dependencias de categoría y las capas añadidas.
- `AppTodoList.Api/Data/AppDbContext.cs` — solo si hace falta adaptar consultas de categoría; sin cambio de esquema.
- `AppTodoList.Api/AppTodoList.Api.csproj` — añadir referencia de DTOs si la estructura del proyecto lo requiere.
- `AppTodoList.Models.Tests/AppTodoList.Models.Tests.csproj` o la solución — incorporar el proyecto de pruebas de API si aún no existe.
- `frontend/src/types/index.ts` — añadir `Categoria`, reflejar `categoria` en `TodoItem` y `categoriaId` en `TodoItemInput`.
- `frontend/src/pages/TareasPage.tsx` — cargar categorías junto a las tareas y pasarlas al formulario.
- `frontend/src/components/TareaForm.tsx` — selector con opción «Sin categoría» para creación y edición.
- `frontend/src/components/TareaItem.tsx` — etiqueta de categoría con el color recibido.
- `docs/analisis-diseño.md` — documentar el endpoint de categorías, DTOs y contrato actualizado de tareas.

## 8. Tests unitarios a implementar

- `ObtenerCategoriasAsync_DeberiaDevolverCategoriasOrdenadasPorNombre`: verifica la consulta del catálogo.
- `CrearAsync_ConCategoriaExistente_DeberiaAsignarla`: verifica que la tarea persiste con la FK y devuelve la categoría.
- `CrearAsync_ConCategoriaInexistente_DeberiaLanzarArgumentException`: evita referencias inválidas antes de guardar.
- `ActualizarAsync_ConCategoriaInexistente_DeberiaLanzarArgumentException`: protege la edición frente a IDs inválidos.
- `ActualizarAsync_ConCategoriaNula_DeberiaQuitarLaAsignacion`: comprueba que se puede retirar una categoría.
- `CompletarAsync_TareaRepetitivaConCategoria_DeberiaConservarCategoriaEnLaSiguienteOcurrencia`: prueba de regresión de la regla existente.
- `GetAll_DeberiaDevolverCategoriaDto`: prueba del controlador para el catálogo y su código `200`.
- `Create_ConCategoriaInexistente_DeberiaDevolverBadRequest`: prueba del contrato HTTP de tareas.

## 9. Criterios de aceptación

- La tabla `TodoItems` y la tabla `Categorias` se conservan; no se elimina ni recrea la base de datos.
- `GET /api/categorias` devuelve las categorías existentes ordenadas por nombre.
- Crear y editar una tarea permite enviar una categoría existente o `null`.
- Enviar un `categoriaId` inexistente produce `400 Bad Request` y no guarda cambios.
- Las respuestas de listado y detalle de tareas incluyen los datos de su categoría cuando exista.
- El formulario de React permite seleccionar «Sin categoría» o una categoría cargada desde la API.
- Cada tarea categorizada muestra una etiqueta legible con su color; las tareas sin categoría no muestran etiqueta.
- Las tareas repetitivas creadas tras completar otra conservan su categoría.
- Los tests nuevos y los existentes pasan.

## 10. Skills a invocar

No existe `docs/skills-orquestacion.md` en el estado actual del repositorio. El orden siguiente se deriva de la cadena de dependencias definida en las instrucciones del proyecto y se mantiene la consulta de diseño previa al frontend. `ui-ux-pro-max` solo aporta una ficha de catálogo local, por lo que la implementación debe aplicar principios básicos de claridad, consistencia y accesibilidad sin afirmar que se usó su biblioteca completa.

> Para ejecutar toda la cadena de una vez, usa el skill orquestador: `nueva-feature`.
> Para ejecutar skills individuales, llámalos en el orden indicado a continuación.

| Orden | Skill | Motivo (qué genera para esta feature) |
|-------|-------|---------------------------------------|
| 1 | `diseño-analisis` | Actualizar el análisis con el catálogo de categorías y los contratos de tarea. |
| 2 | `modelo` | **N/A**: `Categoria`, la FK y la navegación de `TodoItem` ya existen. |
| 3 | `dto` | Crear los DTOs de categoría y sustituir los contratos HTTP de entidad por DTOs de tarea. |
| 4 | `base-de-datos` | **N/A**: no hay cambio de esquema ni migración; verificar que se preserva la configuración existente. |
| 5 | `logica-negocio` | Crear la consulta de categorías y la validación de existencia de `CategoriaId`. |
| 6 | `validaciones` | Validar el identificador de categoría opcional y los contratos de entrada. |
| 7 | `servicio` | Exponer operaciones de categoría y orquestar el mapeo DTO-entidad de tareas. |
| 8 | `controlador` | Añadir `CategoriasController` y adaptar `TareasController` a DTOs. |
| 9 | `tests-unitarios` | Crear pruebas unitarias de categoría, asignación, retirada y regresión de recurrencia. |
| 10 | `ui-ux-pro-max` | Consultar el catálogo de UX antes de cambiar el flujo de selección en React. |
| 11 | `frontend-react` | Añadir el servicio, tipos, selector y etiqueta de categoría en la interfaz de tareas. |
| 12 | `commit-message` | Generar el mensaje de commit al finalizar la implementación. |