# Plan: Listado de categorías en una página de gestión

> Generado por el agente planificador · 2026-09-16

## 1. Resumen

Añadir una página `CategoriasPage` en el frontend que muestre el listado de todas las categorías existentes (nombre y color), como punto de partida para las futuras historias de creación, edición y eliminación. Es la historia 1 de `docs/historias-usuario-crud-categorias.md`.

## 2. Requisitos funcionales

1. El usuario puede navegar a una página dedicada de "Categorías" desde la navegación principal de la app.
2. La página muestra, para cada categoría existente, su nombre y su color.
3. Si no existen categorías, la página muestra un mensaje indicándolo (sin lista vacía silenciosa).

## 3. Cambios en el modelo de datos

### Entidades nuevas o modificadas

**N/A.** No hay cambios en `Models/`, `Dtos/`, `Data/AppDbContext.cs`, `LogicaNegocio/` ni `Services/` (backend). El endpoint `GET /api/categorias` ya existe y devuelve `CategoriaDto` con `id`, `nombre` y `color`. El tipo `Categoria` ya existe en `frontend/src/types/index.ts` con esos mismos campos.

### Migración necesaria

**N/A.** No se requiere ninguna migración de EF Core.

## 4. DTOs

**N/A.** No se crean ni modifican DTOs de backend. En frontend, se reutiliza la interfaz `Categoria` ya existente en [frontend/src/types/index.ts](../frontend/src/types/index.ts):

```typescript
export interface Categoria {
  id: number
  nombre: string
  color: string
}
```

## 5. Endpoints

**N/A — no se crean ni modifican endpoints.** Se reutiliza el ya existente:

| Verbo | Ruta | Cuerpo | Respuesta exitosa | Errores posibles |
|-------|------|--------|-------------------|-------------------|
| GET | `/api/categorias` | — | `200 OK` con `CategoriaDto[]` | Error de red / servidor (gestionado en frontend como mensaje de error) |

## 6. Lógica de negocio

**N/A.** No hay reglas de negocio nuevas: la página solo lee y muestra datos ya validados y persistidos. La única "regla" es de presentación: si el array de categorías está vacío, mostrar un mensaje en lugar de una lista vacía.

## 7. Capas afectadas

Esta es una feature **puramente de frontend**. No se toca ninguna capa del backend (`Models/`, `Dtos/`, `Data/`, `LogicaNegocio/`, `Services/`, `Controllers/`).

**Antes de implementar:** consultar los skills `ui-ux-pro-max` y `frontend-react` para las convenciones de estilo y estructura del proyecto.

> **Nota sobre `ui-ux-pro-max`:** en este workspace el skill solo trae la ficha de catálogo (sin plantillas ni datos de patrones). Si al invocarlo se confirma que no aporta contenido adicional, no bloquear la implementación ni fingir que se ha aplicado: seguir con los principios básicos ya usados en `TareasPage`/`PlantillasPage` (claridad, feedback visible en cada acción, consistencia visual con las páginas existentes, mensajes accesibles) y dejar constancia de que el skill no aportó nada nuevo.

**Crear:**
- `frontend/src/pages/CategoriasPage.tsx` — página que carga las categorías con `categoriasApi.obtenerTodas()` en un `useEffect`, gestiona estados de carga/error, y renderiza el listado o el mensaje de "no hay categorías".
- `frontend/src/components/CategoriaItem.tsx` — componente de presentación que recibe una `Categoria` por props y muestra su nombre y una muestra visual de su color (p. ej. un `<span>` con `backgroundColor` inline o una clase con variable CSS).

**Modificar:**
- `frontend/src/App.tsx` — añadir la pestaña `'categorias'` al tipo `Pestana`, el botón de navegación correspondiente (mismo patrón que `tareas`/`plantillas`) y el renderizado condicional de `<CategoriasPage />`.

**Sin cambios (reutilizados tal cual):**
- `frontend/src/services/categoriasApi.ts` — ya expone `obtenerTodas()`, no requiere ninguna modificación.
- `frontend/src/types/index.ts` — el tipo `Categoria` ya existe con los campos necesarios.

## 8. Tests unitarios a implementar

El proyecto de tests actual (`AppTodoList.Api.Tests`, xUnit + Moq) cubre backend (Controllers/Services/LogicaNegocio). Esta feature no toca backend, por lo que no genera tests en ese proyecto.

Si el proyecto dispusiera de un framework de tests de frontend (Vitest/React Testing Library) ya configurado, los casos a cubrir serían:
- `CategoriasPage_MuestraListado`: dado que `categoriasApi.obtenerTodas()` devuelve categorías, la página renderiza el nombre y el color de cada una.
- `CategoriasPage_MuestraMensajeSinCategorias`: dado que `categoriasApi.obtenerTodas()` devuelve un array vacío, la página muestra el mensaje indicándolo y no renderiza ninguna lista.
- `CategoriasPage_MuestraErrorSiFallaLaCarga`: dado que `categoriasApi.obtenerTodas()` rechaza la promesa, la página muestra un mensaje de error.

**Nota:** el workspace actual no tiene configurado un runner de tests de frontend (no hay `vitest`/`@testing-library/react` en `frontend/package.json` según la estructura observada). Si no existe dicha infraestructura, verificar manualmente en el navegador es el criterio de aceptación aplicable; no se debe introducir un framework de testing de frontend nuevo como efecto colateral de esta historia salvo que se pida explícitamente.

## 9. Criterios de aceptación

- [ ] Existe una nueva página `CategoriasPage` en el frontend, accesible desde la navegación.
- [ ] La página muestra el nombre y el color de cada categoría existente.
- [ ] Si no hay categorías, se muestra un mensaje indicándolo.
- [ ] No se ha modificado ningún fichero de backend (`Models/`, `Dtos/`, `Data/`, `LogicaNegocio/`, `Services/`, `Controllers/`).
- [ ] `categoriasApi.ts` permanece sin cambios.
- [ ] El proyecto frontend compila (`npm run build` o equivalente) sin errores de TypeScript.

## 10. Skills a invocar

> Para ejecutar toda la cadena de una vez, usa el skill orquestador: `nueva-feature`.
> Para ejecutar skills individuales, llámalos en el orden indicado a continuación.

| Orden | Skill | Motivo (qué genera para esta feature) |
|-------|-------|-----------------------------------------|
| 1 | `diseño-analisis` | **N/A.** No hay cambios en el modelo de datos ni en los endpoints; el análisis ya documenta `GET /api/categorias`. |
| 2 | `modelo` | **N/A.** No hay entidades ni campos nuevos. |
| 3 | `dto` | **N/A.** No hay DTOs nuevos ni modificados. |
| 4 | `base-de-datos` | **N/A.** No hay cambios en `AppDbContext` ni migraciones. |
| 5 | `logica-negocio` | **N/A.** No hay reglas de negocio ni acceso a datos nuevos. |
| 6 | `validaciones` | **N/A.** No hay entrada de usuario que validar (página de solo lectura). |
| 7 | `servicio` | **N/A.** No hay métodos de servicio de backend nuevos ni modificados. |
| 8 | `controlador` | **N/A.** No hay endpoints nuevos ni modificados. |
| 9 | `ui-ux-pro-max` | Consultar antes de maquetar la página, para aplicar heurísticas de usabilidad y accesibilidad al listado y al mensaje de "sin categorías". En este workspace solo aporta la ficha de catálogo: si no da contenido adicional, seguir con los principios básicos ya aplicados en `TareasPage`/`PlantillasPage` y dejarlo anotado. |
| 10 | `frontend-react` | Genera `CategoriasPage.tsx`, `CategoriaItem.tsx` y el enlace de navegación en `App.tsx`, siguiendo el mismo patrón que `TareasPage`/`PlantillasPage`. |
| 11 | `commit-message` | Siempre, al finalizar la implementación. |
