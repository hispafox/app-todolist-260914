# Plan: Editar una categoría existente

> Generado por el agente planificador · 2026-09-16
> Issue GitHub #2 · Historia 3 de `docs/historias-usuario-crud-categorias.md`

## 1. Resumen

Permitir al usuario consultar una categoría por su id (`GET /api/categorias/{id}`) y editar su nombre o color (`PUT /api/categorias/{id}`), reutilizando `CategoriaForm` en modo edición desde `CategoriasPage`.

## 2. Requisitos funcionales

1. El usuario puede consultar `GET /api/categorias/{id}` y recibir `200 OK` con la categoría, o `404 Not Found` si no existe.
2. El usuario puede enviar `PUT /api/categorias/{id}` con nombre y color, y recibir `200 OK` con la categoría actualizada, o `404 Not Found` si no existe.
3. Si el nombre está vacío o supera 100 caracteres, la API responde `400 Bad Request`.
4. Desde `CategoriasPage` el usuario puede pulsar "Editar" en una categoría, ver el formulario precargado con sus datos, guardar los cambios y verlos reflejados en el listado sin recargar la página.

## 3. Cambios en el modelo de datos

### Entidades nuevas o modificadas

Ninguna. `Categoria` (`AppTodoList.Models/Models/Categoria.cs`) ya tiene `Id`, `Nombre` y `Color`. No se requieren campos nuevos.

### Migración necesaria

Ninguna. No hay cambios de esquema.

## 4. DTOs

No se crean DTOs nuevos. Se reutilizan los existentes:

```csharp
public class GuardarCategoriaDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(30, ErrorMessage = "El color no puede superar 30 caracteres.")]
    public string Color { get; set; } = string.Empty;
}

public class CategoriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}
```

**Cambio a realizar (no es un DTO nuevo, es un ajuste de contrato):** `ICategoriaService.ActualizarAsync` recibe hoy `CategoriaDto` (`Task<CategoriaDto?> ActualizarAsync(int id, CategoriaDto categoria)`), lo cual expone un campo `Id` en el cuerpo que no se usa (el id viene de la ruta) y rompe la convención ya aplicada a `CrearAsync(GuardarCategoriaDto dto)` en el issue #4. Hay que cambiar la firma a:

```csharp
Task<CategoriaDto?> ActualizarAsync(int id, GuardarCategoriaDto dto);
```

Esto además permite que las `DataAnnotations` de `GuardarCategoriaDto` (`[Required]`, `[MaxLength(100)]`) se apliquen igual que en `POST`, manteniendo el mismo patrón en ambos endpoints.

## 5. Endpoints

| Verbo | Ruta | Cuerpo | Respuesta exitosa | Errores posibles |
|-------|------|--------|-------------------|-----------------|
| GET | `/api/categorias/{id}` | — | `200 OK` con `CategoriaDto` | `404 Not Found` si no existe la categoría |
| PUT | `/api/categorias/{id}` | `GuardarCategoriaDto` (`nombre`, `color`) | `200 OK` con `CategoriaDto` actualizada | `400 Bad Request` si `nombre` vacío o > 100 caracteres; `404 Not Found` si no existe la categoría |

## 6. Lógica de negocio

No se añaden reglas nuevas: `CategoriaLogica.ActualizarAsync` ya existe, ya busca la categoría por id (devuelve `null` si no existe) y ya valida el nombre con `ValidarCategoria` (lanza `ArgumentException` si está vacío o supera 100 caracteres) antes de persistir con `AppDbContext`. El controlador debe:

- Devolver `NotFound()` cuando `ICategoriaService.ObtenerPorIdAsync`/`ActualizarAsync` devuelvan `null`.
- Capturar `ArgumentException` y devolver `BadRequest(ex.Message)`, siguiendo el mismo patrón que `Create`.

## 7. Capas afectadas

**Crear:**
- Ninguno.

**Modificar:**
- `AppTodoList.Api/Services/ICategoriaService.cs` — cambiar `ActualizarAsync(int id, CategoriaDto categoria)` por `ActualizarAsync(int id, GuardarCategoriaDto dto)`.
- `AppTodoList.Api/Services/CategoriaService.cs` — adaptar `ActualizarAsync` para recibir `GuardarCategoriaDto`, mapear a `Categoria` y delegar en `ICategoriaLogica.ActualizarAsync`.
- `AppTodoList.Api/Controllers/CategoriasController.cs` — añadir:
  - `[HttpGet("{id}")]` que delega en `ICategoriaService.ObtenerPorIdAsync(id)` y devuelve `Ok(categoria)` o `NotFound()`.
  - `[HttpPut("{id}")]` que recibe `GuardarCategoriaDto`, delega en `ICategoriaService.ActualizarAsync(id, dto)`, captura `ArgumentException` → `400`, y devuelve `Ok(actualizada)` o `NotFound()`.
- `AppTodoList.Api.Tests/Controllers/CategoriasControllerTests.cs` — añadir tests de los nuevos endpoints (ver sección 8).
- `frontend/src/services/categoriasApi.ts` — añadir:
  - `obtenerPorId: (id: number) => request<Categoria>(`/categorias/${id}`)`.
  - `actualizar: (id: number, categoria: CategoriaInput) => request<Categoria>(`/categorias/${id}`, { method: 'PUT', body: JSON.stringify(categoria) })`.
- `frontend/src/components/CategoriaForm.tsx` — añadir soporte de modo edición: prop opcional `categoriaEnEdicion?: Categoria | null` que precarga `nombre`/`color` en el estado del formulario (vía `useEffect` sobre esa prop), cambiar el texto del botón a "Actualizar" cuando se está editando, y añadir un botón "Cancelar" (llama a una prop `onCancelar?: () => void`) que solo se muestra en modo edición.
- `frontend/src/components/CategoriaItem.tsx` — añadir prop `onEditar: (categoria: Categoria) => void` y un botón "Editar" que la invoca.
- `frontend/src/pages/CategoriasPage.tsx` — añadir estado `categoriaEditando: Categoria | null`; función `manejarEditarClick(categoria)` que lo establece; adaptar `manejarGuardar` para que, si `categoriaEditando` tiene valor, llame a `categoriasApi.actualizar(categoriaEditando.id, categoria)` y reemplace la categoría en el array local (`setCategorias` con `map`), y si no, mantenga el flujo actual de `crear()`; añadir `manejarCancelarEdicion()` que limpia `categoriaEditando`; pasar `categoriaEnEdicion` y `onCancelar` a `CategoriaForm`, y `onEditar` a cada `CategoriaItem`.

**Decisión de diseño:** la edición reutiliza los datos de la categoría ya presentes en el estado local de `CategoriasPage` (`categorias`) para precargar el formulario, sin llamar a `categoriasApi.obtenerPorId()` — evita una petición de red innecesaria cuando el dato ya está en memoria. `obtenerPorId()` se añade al cliente API porque lo pide la historia de usuario y porque cubre el endpoint `GET /api/categorias/{id}` (útil para tests, deep-linking futuro o si `CategoriasPage` pasa a paginar/no mantener todo el listado en memoria).

## 8. Tests unitarios a implementar

En `AppTodoList.Api.Tests/Controllers/CategoriasControllerTests.cs`:

- `GetById_ConIdExistente_DeberiaDevolver200ConLaCategoria`: crea una categoría, pide `GetById` con su id, espera `OkObjectResult` con `CategoriaDto` cuyo `Nombre` coincide.
- `GetById_ConIdInexistente_DeberiaDevolver404`: `GetById` con un id que no existe (p. ej. `9999`) devuelve `NotFoundResult`.
- `Update_ConDatosValidos_DeberiaDevolver200ConLaCategoriaActualizada`: crea una categoría, la actualiza con nombre/color nuevos, espera `OkObjectResult` con `CategoriaDto` reflejando los cambios.
- `Update_ConIdInexistente_DeberiaDevolver404`: `Update` con un id que no existe devuelve `NotFoundResult`.
- `Update_ConNombreVacio_DeberiaDevolver400`: `Update` sobre una categoría existente con nombre vacío/solo espacios devuelve `BadRequestObjectResult`.
- `Update_ConNombreDemasiadoLargo_DeberiaDevolver400`: `Update` sobre una categoría existente con nombre de 101+ caracteres devuelve `BadRequestObjectResult`.

Si existen tests de `CategoriaService`/`CategoriaLogica` que usan `ActualizarAsync(CategoriaDto)`, actualizarlos para que usen `ActualizarAsync(GuardarCategoriaDto)`.

## 9. Criterios de aceptación

- [ ] `GET /api/categorias/{id}` devuelve `200 OK` con la categoría si existe, o `404 Not Found` si no.
- [ ] `PUT /api/categorias/{id}` con nombre y color válidos devuelve `200 OK` con la categoría actualizada.
- [ ] `PUT /api/categorias/{id}` sobre un id inexistente devuelve `404 Not Found`.
- [ ] `PUT /api/categorias/{id}` con nombre vacío o de más de 100 caracteres devuelve `400 Bad Request`.
- [ ] Desde `CategoriasPage`, pulsar "Editar" en una categoría precarga `CategoriaForm` con sus datos actuales.
- [ ] Guardar la edición actualiza la categoría en el listado sin recargar la página (sin `window.location.reload()` ni navegación completa).
- [ ] Existe una forma de cancelar la edición sin guardar cambios.
- [ ] `dotnet build` y `dotnet test` pasan sin errores.
- [ ] El frontend compila (`npm run build` o equivalente, verificado por quien implemente) sin errores de TypeScript.

## 10. Skills a invocar

> Para ejecutar toda la cadena de una vez, usa el skill orquestador: `nueva-feature`.
> Para ejecutar skills individuales, llámalos en el orden indicado a continuación.

| Orden | Skill | Motivo (qué genera para esta feature) |
|-------|-------|---------------------------------------|
| 1 | `diseño-analisis` | **N/A** — no hay cambios en el modelo de datos; los endpoints `GET/{id}` y `PUT/{id}` ya estaban contemplados en el análisis original de categorías. |
| 2 | `modelo` | **N/A** — `Categoria` no cambia. |
| 3 | `dto` | **N/A** — `GuardarCategoriaDto` y `CategoriaDto` ya existen y cubren el caso; solo cambia la firma del servicio que los consume. |
| 4 | `base-de-datos` | **N/A** — no hay migración ni cambios en `AppDbContext`. |
| 5 | `logica-negocio` | **N/A** — `CategoriaLogica.ObtenerPorIdAsync` y `ActualizarAsync` ya existen y ya validan nombre vacío/longitud y existencia por id. |
| 6 | `validaciones` | **N/A** — las validaciones de `GuardarCategoriaDto` (DataAnnotations) y la guarda de dominio en `CategoriaLogica` ya existen; no se añaden reglas nuevas. |
| 7 | `servicio` | **Sí** — cambiar la firma de `ICategoriaService.ActualizarAsync`/`CategoriaService.ActualizarAsync` de `CategoriaDto` a `GuardarCategoriaDto`, siguiendo la convención ya aplicada a `CrearAsync`. |
| 8 | `controlador` | **Sí** — añadir `[HttpGet("{id}")]` y `[HttpPut("{id}")]` en `CategoriasController`, con `NotFound()`/`BadRequest()` según corresponda. |
| 9 | `ui-ux-pro-max` | **Sí** — consultar antes de diseñar el modo edición de `CategoriaForm` (botón "Editar", precarga de datos, botón "Cancelar", feedback visual de qué categoría se está editando). |
| 10 | `frontend-react` | **Sí** — añadir `obtenerPorId()`/`actualizar()` a `categoriasApi.ts`, el modo edición en `CategoriaForm.tsx`, el botón "Editar" en `CategoriaItem.tsx`, y la orquestación del estado de edición en `CategoriasPage.tsx`. |
| 11 | `tests-unitarios` | **Sí** — añadir los tests del punto 8 (`GetById` éxito/404, `Update` éxito/404/nombre vacío/nombre largo). |
| 12 | `commit-message` | Siempre, al finalizar la implementación. |
