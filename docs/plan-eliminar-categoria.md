# Plan: Eliminar una categoría

> Generado por el agente planificador · 2026-09-16
> Issue GitHub #3 · Historia 4 de `docs/historias-usuario-crud-categorias.md`

## 1. Resumen

Permitir al usuario eliminar una categoría existente (`DELETE /api/categorias/{id}`), con confirmación previa en `CategoriasPage` y actualización del listado sin recargar la página.

## 2. Requisitos funcionales

1. El usuario puede enviar `DELETE /api/categorias/{id}` y recibir `204 No Content` si la categoría existía y se eliminó, o `404 Not Found` si no existe.
2. Desde `CategoriasPage`, el usuario puede pulsar "Eliminar" en una categoría y se le pide confirmación antes de borrarla.
3. Tras confirmar, la categoría desaparece del listado sin recargar la página.
4. Si el usuario cancela la confirmación, no se realiza ninguna petición ni cambio.

## 3. Cambios en el modelo de datos

### Entidades nuevas o modificadas

Ninguna. `Categoria` (`AppTodoList.Models/Models/Categoria.cs`) no requiere cambios.

### Migración necesaria

Ninguna. No hay cambio de esquema.

**Riesgo de integridad revisado (fuera de alcance salvo bloqueo):** en `AppDbContext.OnModelCreating`, la relación `TodoItem.Categoria` está configurada con `.OnDelete(DeleteBehavior.SetNull)` y `CategoriaId` es nullable en `TodoItem`. Esto significa que eliminar una categoría con tareas asociadas **no** lanza una violación de FK: EF Core pone `CategoriaId = null` en esas tareas automáticamente. No hay ninguna restricción que rompa el build o los tests, por lo que no se necesita tratamiento adicional en esta historia.

## 4. DTOs

No se crean ni modifican DTOs. `DELETE` no recibe cuerpo y no devuelve contenido (`204 No Content`).

## 5. Endpoints

| Verbo | Ruta | Cuerpo | Respuesta exitosa | Errores posibles |
|-------|------|--------|-------------------|-----------------|
| DELETE | `/api/categorias/{id}` | — | `204 No Content` | `404 Not Found` si no existe la categoría |

## 6. Lógica de negocio

No se añaden reglas nuevas: `CategoriaLogica.EliminarAsync(id)` ya existe, ya busca la categoría por id (devuelve `false` si no existe) y ya elimina con `_contexto.Categorias.Remove(...)` + `SaveChangesAsync()`. `CategoriaService.EliminarAsync(id)` ya delega directamente en `ICategoriaLogica.EliminarAsync(id)`. El controlador debe:

- Delegar en `ICategoriaService.EliminarAsync(id)`.
- Devolver `NoContent()` si el resultado es `true`.
- Devolver `NotFound()` si el resultado es `false`.

## 7. Capas afectadas

**Crear:**
- Ninguno.

**Modificar:**
- `AppTodoList.Api/Controllers/CategoriasController.cs` — añadir `[HttpDelete("{id}")]` que delega en `ICategoriaService.EliminarAsync(id)` y devuelve `NoContent()` o `NotFound()`.
- `AppTodoList.Api.Tests/Controllers/CategoriasControllerTests.cs` — añadir tests del nuevo endpoint (ver sección 8).
- `frontend/src/services/categoriasApi.ts` — añadir `eliminar: (id: number) => request<void>(`/categorias/${id}`, { method: 'DELETE' })`.
- `frontend/src/components/CategoriaItem.tsx` — añadir prop `onEliminar: (categoria: Categoria) => void` y un botón "Eliminar" que la invoca.
- `frontend/src/pages/CategoriasPage.tsx` — añadir función `manejarEliminarClick(categoria)` que pide confirmación con `window.confirm(...)`, y si se confirma, llama a `categoriasApi.eliminar(categoria.id)` y actualiza el estado local con `setCategorias((actuales) => actuales.filter((c) => c.id !== categoria.id))`; pasar `onEliminar` a cada `CategoriaItem`. Si la categoría eliminada era la que estaba en edición (`categoriaEditando`), limpiarla para no dejar el formulario apuntando a una categoría inexistente.

No se requieren cambios en `Models/`, `Dtos/`, `Data/AppDbContext.cs`, `LogicaNegocio/` ni `Services/`: `EliminarAsync` ya está implementado en las tres capas de backend.

## 8. Tests unitarios a implementar

En `AppTodoList.Api.Tests/Controllers/CategoriasControllerTests.cs`:

- `Delete_ConIdExistente_DeberiaDevolver204YEliminarLaCategoria`: crea una categoría, la elimina, espera `NoContentResult`, y comprueba con `GetAll` que ya no aparece en el listado.
- `Delete_ConIdInexistente_DeberiaDevolver404`: `Delete` con un id que no existe (p. ej. `9999`) devuelve `NotFoundResult`.

## 9. Criterios de aceptación

- [ ] `DELETE /api/categorias/{id}` devuelve `204 No Content` si la categoría existía y se elimina.
- [ ] `DELETE /api/categorias/{id}` devuelve `404 Not Found` si la categoría no existe.
- [ ] Desde `CategoriasPage`, pulsar "Eliminar" en una categoría muestra una confirmación antes de borrarla.
- [ ] Si el usuario cancela la confirmación, la categoría no se elimina ni se realiza ninguna petición.
- [ ] Tras confirmar, la categoría desaparece del listado sin recargar la página (sin `window.location.reload()` ni navegación completa).
- [ ] `dotnet build` y `dotnet test` pasan sin errores.
- [ ] El frontend compila (`npm run build` o equivalente, verificado por quien implemente) sin errores de TypeScript.

## 10. Skills a invocar

> Para ejecutar toda la cadena de una vez, usa el skill orquestador: `nueva-feature`.
> Para ejecutar skills individuales, llámalos en el orden indicado a continuación.

| Orden | Skill | Motivo (qué genera para esta feature) |
|-------|-------|---------------------------------------|
| 1 | `diseño-analisis` | N/A — no hay cambios en el modelo de datos ni en el contrato de endpoints ya descrito en el análisis (el endpoint `DELETE` es una adición menor, no un rediseño). |
| 2 | `modelo` | N/A — no hay entidades ni campos nuevos. |
| 3 | `dto` | N/A — no hay DTOs nuevos ni modificados. |
| 4 | `base-de-datos` | N/A — no hay cambios en `AppDbContext` ni migración necesaria. |
| 5 | `logica-negocio` | N/A — `CategoriaLogica.EliminarAsync` ya existe e implementa la regla completa. |
| 6 | `validaciones` | N/A — no hay validaciones de entrada nuevas (no hay cuerpo en `DELETE`). |
| 7 | `servicio` | N/A — `CategoriaService.EliminarAsync` ya existe y delega correctamente. |
| 8 | `controlador` | Sí — añadir el endpoint `[HttpDelete("{id}")]` en `CategoriasController` delegando en `ICategoriaService.EliminarAsync`. |
| — | `ui-ux-pro-max` | Sí, antes de `frontend-react` — consultar patrones de confirmación de borrado (diálogo de confirmación, feedback visual) para el botón "Eliminar" en `CategoriaItem`. |
| — | `frontend-react` | Sí — añadir `eliminar()` a `categoriasApi.ts`, el botón "Eliminar" con `window.confirm` en `CategoriaItem`, y la lógica de eliminación en `CategoriasPage`. |
| 9 | `commit-message` | Siempre, al finalizar la implementación. |

**Nota:** también hay que añadir tests unitarios (`Delete_ConIdExistente_DeberiaDevolver204YEliminarLaCategoria`, `Delete_ConIdInexistente_DeberiaDevolver404`) al proyecto de tests existente, como parte del mismo cambio del controlador (no requiere un skill propio en el catálogo).
