# Plan: Crear una categoría nueva

> Generado por el agente planificador · 2026-09-16
> Issue GitHub #4 · Historia 2 de `docs/historias-usuario-crud-categorias.md`

## 1. Resumen

Permitir al usuario crear una categoría nueva (nombre + color) desde `POST /api/categorias`, y añadir en `CategoriasPage` un formulario de alta que refresque el listado sin recargar la página.

## 2. Requisitos funcionales

1. El usuario puede enviar `POST /api/categorias` con nombre y color, y recibir `201 Created` con la categoría creada.
2. Si el nombre está vacío o supera 100 caracteres, la API responde `400 Bad Request`.
3. Desde `CategoriasPage` existe un formulario para dar de alta una categoría (nombre + color).
4. Al guardar correctamente, la nueva categoría aparece en el listado sin recargar la página.

## 3. Cambios en el modelo de datos

### Entidades nuevas o modificadas

Ninguna. `Categoria` (`AppTodoList.Models/Models/Categoria.cs`) ya tiene `Id`, `Nombre` y `Color`. No se requieren campos nuevos.

### Migración necesaria

Ninguna. No hay cambios de esquema.

## 4. DTOs

No se crean DTOs nuevos. `GuardarCategoriaDto` ya existe con las validaciones necesarias:

```csharp
public class GuardarCategoriaDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(30, ErrorMessage = "El color no puede superar 30 caracteres.")]
    public string Color { get; set; } = string.Empty;
}
```

`CategoriaDto` (salida) tampoco cambia:

```csharp
public class CategoriaDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}
```

**Cambio a realizar (no es un DTO nuevo, es un ajuste de contrato):** `ICategoriaService.CrearAsync` recibe hoy `CategoriaDto` en vez de `GuardarCategoriaDto`, rompiendo la convención ya usada en `ITodoService.CrearAsync(GuardarTareaDto dto)`. Hay que cambiar la firma a `Task<CategoriaDto> CrearAsync(GuardarCategoriaDto dto)` en `ICategoriaService` y `CategoriaService`.

## 5. Endpoints

| Verbo | Ruta | Cuerpo | Respuesta exitosa | Errores posibles |
|-------|------|--------|-------------------|-----------------|
| POST | `/api/categorias` | `GuardarCategoriaDto` (`nombre`, `color`) | `201 Created` con `CategoriaDto` creada (header `Location: /api/categorias/{id}`) | `400 Bad Request` si `nombre` vacío o > 100 caracteres (validación automática de `[ApiController]` sobre las `DataAnnotations`, reforzada por la validación de dominio en `CategoriaLogica`) |

> Nota: `CategoriasController` todavía no tiene `GetById` (llegará con la historia 3, issue #3). Por tanto el `201 Created` debe construirse con `Created($"/api/categorias/{creada.Id}", creada)` en vez de `CreatedAtAction(nameof(GetById), ...)`, ya que esa acción no existe todavía.

## 6. Lógica de negocio

No se añaden reglas nuevas: `CategoriaLogica.CrearAsync` ya valida que el nombre no esté vacío ni supere 100 caracteres (lanzando `ArgumentException`), y persiste con `AppDbContext`. El controlador deberá capturar `ArgumentException` y devolver `BadRequest(ex.Message)`, siguiendo el mismo patrón que `TareasController.Create`.

## 7. Capas afectadas

**Crear:**
- Ninguno.

**Modificar:**
- `AppTodoList.Api/Services/ICategoriaService.cs` — cambiar `CrearAsync(CategoriaDto categoria)` por `CrearAsync(GuardarCategoriaDto dto)`.
- `AppTodoList.Api/Services/CategoriaService.cs` — adaptar `CrearAsync` para recibir `GuardarCategoriaDto` y mapear a `Categoria` antes de delegar en `ICategoriaLogica.CrearAsync`.
- `AppTodoList.Api/Controllers/CategoriasController.cs` — añadir `[HttpPost]` que recibe `GuardarCategoriaDto`, delega en `ICategoriaService.CrearAsync`, captura `ArgumentException` → `400`, y devuelve `201 Created`.
- `frontend/src/services/categoriasApi.ts` — añadir `crear(categoria: { nombre: string; color: string }) => request<Categoria>('/categorias', { method: 'POST', body: JSON.stringify(categoria) })`.
- `frontend/src/pages/CategoriasPage.tsx` — añadir un formulario de alta (nombre + color) integrado en la página; al enviarlo, llamar a `categoriasApi.crear()` y, si tiene éxito, añadir la categoría al estado local (`setCategorias`) sin volver a pedir el listado completo (o recargarlo con `cargarCategorias()`, ambas opciones son válidas; se recomienda añadir al array local para evitar una petición extra).
- `AppTodoList.Api.Tests/Controllers/CategoriasControllerTests.cs` — añadir tests del nuevo endpoint (ver sección 8).

**No se crea** ningún componente nuevo de formulario reutilizable (`CategoriaForm.tsx`) salvo que se prefiera separarlo de `CategoriasPage.tsx` por claridad; dado el tamaño reducido del formulario (2 campos), se plantea como opción B en la sección 10 pero no es obligatorio.

## 8. Tests unitarios a implementar

En `AppTodoList.Api.Tests/Controllers/CategoriasControllerTests.cs`:

- `Create_ConDatosValidos_DeberiaDevolver201ConLaCategoriaCreada`: verifica que `POST` con nombre y color válidos devuelve `CreatedAtActionResult`/`CreatedResult` con `CategoriaDto` cuyo `Id` > 0.
- `Create_ConNombreVacio_DeberiaDevolver400`: nombre vacío o solo espacios → `BadRequestObjectResult`.
- `Create_ConNombreDemasiadoLargo_DeberiaDevolver400`: nombre de 101+ caracteres → `BadRequestObjectResult`.
- `Create_DeberiaPersistirLaCategoriaEnElContexto`: tras crear, `GetAll` incluye la nueva categoría.

Si existen tests de `CategoriaService` o `CategoriaLogica` (revisar `AppTodoList.Api.Tests/Servicios/`), actualizar los que usaban `CrearAsync(CategoriaDto)` para que usen `CrearAsync(GuardarCategoriaDto)`.

## 9. Criterios de aceptación

- [ ] `POST /api/categorias` con nombre y color válidos devuelve `201 Created` con la categoría creada (incluyendo `Id` asignado).
- [ ] `POST /api/categorias` con nombre vacío devuelve `400 Bad Request`.
- [ ] `POST /api/categorias` con nombre de más de 100 caracteres devuelve `400 Bad Request`.
- [ ] `CategoriasPage` muestra un formulario con campos nombre y color.
- [ ] Al enviar el formulario con datos válidos, la nueva categoría aparece en el listado sin recargar la página (sin `window.location.reload()` ni navegación completa).
- [ ] `dotnet build` y `dotnet test` pasan sin errores.
- [ ] El frontend compila (`npm run build` o equivalente, verificado por quien implemente) sin errores de TypeScript.

## 10. Skills a invocar

> Para ejecutar toda la cadena de una vez, usa el skill orquestador: `nueva-feature`.
> Para ejecutar skills individuales, llámalos en el orden indicado a continuación.

| Orden | Skill | Motivo (qué genera para esta feature) |
|-------|-------|---------------------------------------|
| 1 | `diseño-analisis` | **N/A** — no hay cambios en el modelo de datos; el endpoint `POST /api/categorias` ya estaba contemplado en el análisis original de categorías. |
| 2 | `modelo` | **N/A** — `Categoria` no cambia. |
| 3 | `dto` | **N/A** — `GuardarCategoriaDto` y `CategoriaDto` ya existen y cubren el caso; solo cambia la firma del servicio que los consume. |
| 4 | `base-de-datos` | **N/A** — no hay migración ni cambios en `AppDbContext`. |
| 5 | `logica-negocio` | **N/A** — `CategoriaLogica.CrearAsync` ya existe y ya valida nombre vacío/longitud. |
| 6 | `validaciones` | **N/A** — las validaciones de `GuardarCategoriaDto` (DataAnnotations) y la guarda de dominio en `CategoriaLogica` ya existen; no se añaden reglas nuevas. |
| 7 | `servicio` | **Sí** — cambiar la firma de `ICategoriaService.CrearAsync`/`CategoriaService.CrearAsync` de `CategoriaDto` a `GuardarCategoriaDto`, siguiendo la convención de `ITodoService`. |
| 8 | `controlador` | **Sí** — añadir el endpoint `[HttpPost]` en `CategoriasController` con manejo de `ArgumentException` → `400` y respuesta `201 Created`. |
| 9 | `ui-ux-pro-max` | **Sí** — consultar antes de diseñar el formulario de alta en `CategoriasPage` (layout, accesibilidad de campos nombre/color). |
| 10 | `frontend-react` | **Sí** — añadir `crear()` a `categoriasApi.ts` y el formulario de alta en `CategoriasPage.tsx`, con actualización del listado sin recargar. |
| 11 | `tests-unitarios` | **Sí** — añadir los tests del punto 8 (endpoint POST: éxito, nombre vacío, nombre demasiado largo). |
| 12 | `commit-message` | Siempre, al finalizar la implementación. |
