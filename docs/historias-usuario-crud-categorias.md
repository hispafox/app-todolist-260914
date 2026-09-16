# Historias de usuario — CRUD de categorías

Fecha: 2026-09-16

## Contexto

El backend ya tiene implementadas las capas de `LogicaNegocio` (`CategoriaLogica`) y `Services` (`CategoriaService`) con soporte completo de CRUD (crear, consultar por id, actualizar, eliminar). Sin embargo:

- `CategoriasController` solo expone `GET /api/categorias` (listar todas). Faltan los endpoints de creación, edición, eliminación y consulta por id.
- El frontend (`categoriasApi.ts`) solo tiene `obtenerTodas()`. No existe ninguna página de gestión de categorías (solo se usan como selector de solo lectura en `TareasPage`).

## Historias

### 1. Ver el listado de categorías en una página de gestión

Como usuario de la aplicación, quiero ver una página dedicada con el listado de todas las categorías, para poder acceder desde ahí a crearlas, editarlas o eliminarlas.

**Criterios de aceptación**
- [ ] Existe una nueva página `CategoriasPage` en el frontend, accesible desde la navegación.
- [ ] La página muestra el nombre y el color de cada categoría existente.
- [ ] Si no hay categorías, se muestra un mensaje indicándolo.

**Notas técnicas**
- Reutiliza `categoriasApi.obtenerTodas()`, ya existente.
- Capas afectadas: `frontend/src/pages`, `frontend/src/services/categoriasApi.ts` (sin cambios).

---

### 2. Crear una categoría nueva

Como usuario de la aplicación, quiero crear una nueva categoría indicando su nombre y color, para poder clasificar mis tareas.

**Criterios de aceptación**
- [ ] `POST /api/categorias` crea la categoría y devuelve `201 Created` con la categoría creada.
- [ ] Si el nombre está vacío o supera 100 caracteres, la API responde `400 Bad Request`.
- [ ] Desde `CategoriasPage` existe un formulario para dar de alta una categoría (nombre + color).
- [ ] Al guardar correctamente, la nueva categoría aparece en el listado sin recargar la página.

**Notas técnicas**
- Backend: añadir el endpoint en `CategoriasController` usando `GuardarCategoriaDto` (ya existe con validaciones) y delegando en `ICategoriaService.CrearAsync`.
- Frontend: añadir `crear()` a `categoriasApi.ts`.
- Depende de la historia 1 (página donde ubicar el formulario).

---

### 3. Editar una categoría existente

Como usuario de la aplicación, quiero editar el nombre o el color de una categoría existente, para corregir o actualizar su información.

**Criterios de aceptación**
- [ ] `GET /api/categorias/{id}` devuelve la categoría solicitada o `404 Not Found` si no existe.
- [ ] `PUT /api/categorias/{id}` actualiza la categoría y devuelve `200 OK` con los datos actualizados, o `404 Not Found` si no existe.
- [ ] Si el nombre está vacío o supera 100 caracteres, la API responde `400 Bad Request`.
- [ ] Desde `CategoriasPage` se puede editar una categoría existente y ver el cambio reflejado en el listado.

**Notas técnicas**
- Backend: añadir ambos endpoints en `CategoriasController`, delegando en `ICategoriaService.ObtenerPorIdAsync` y `ActualizarAsync`.
- Frontend: añadir `obtenerPorId()` y `actualizar()` a `categoriasApi.ts`.
- Depende de la historia 1.

---

### 4. Eliminar una categoría

Como usuario de la aplicación, quiero eliminar una categoría que ya no necesito, para mantener el listado limpio y relevante.

**Criterios de aceptación**
- [ ] `DELETE /api/categorias/{id}` elimina la categoría y devuelve `204 No Content`, o `404 Not Found` si no existe.
- [ ] Desde `CategoriasPage` se puede eliminar una categoría existente, previa confirmación del usuario.
- [ ] Tras eliminar, la categoría desaparece del listado sin recargar la página.

**Notas técnicas**
- Backend: añadir el endpoint en `CategoriasController`, delegando en `ICategoriaService.EliminarAsync`.
- Frontend: añadir `eliminar()` a `categoriasApi.ts`.
- Considerar qué ocurre con las tareas que ya tenían asignada la categoría eliminada (fuera de alcance de esta historia salvo que el usuario lo pida).
- Depende de la historia 1.

## Issues creados en GitHub

| # | Historia | URL |
|---|---|---|
| 1 | Ver el listado de categorías en una página de gestión | https://github.com/hispafox/app-todolist-260914/issues/1 |
| 2 | Crear una categoría nueva | https://github.com/hispafox/app-todolist-260914/issues/2 |
| 3 | Editar una categoría existente | https://github.com/hispafox/app-todolist-260914/issues/3 |
| 4 | Eliminar una categoría | https://github.com/hispafox/app-todolist-260914/issues/4 |

**Nota:** no se aplicó la etiqueta `historia-de-usuario` porque no existe en el repositorio.
