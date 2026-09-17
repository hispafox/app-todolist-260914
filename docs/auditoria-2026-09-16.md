# 🔍 INFORME DE AUDITORÍA — AppTodoList

**Fecha:** 2026-09-16
**Alcance:** Aplicación completa — `AppTodoList.Models/Models`, `AppTodoList.Api/{Controllers,Data,Dtos,LogicaNegocio,Services,Program.cs}`, `AppTodoList.Api.csproj`, migraciones, y tests (`AppTodoList.Api.Tests`, `AppTodoList.Models.Tests`).
**Compilación:** ✅ OK (4 proyectos, 0 errores, 0 advertencias)
**Tests:** 26 totales, 26 pasados, 0 fallidos

---

## VEREDICTO GLOBAL

```
╔══════════════════════════════════════════╗
║              RECHAZADO                    ║
║         Puntuación: 23 / 100              ║
╚══════════════════════════════════════════╝
```

La compilación y los tests existentes pasan, pero hay **2 hallazgos críticos** de arquitectura y seguridad de diseño que, según el criterio de veredicto de esta auditoría, obligan a RECHAZAR el estado actual con independencia de la puntuación.

---

## RESUMEN DE HALLAZGOS

| Severidad | Cantidad |
|---|---|
| 🔴 Crítico | 2 |
| 🟠 Alto | 2 |
| 🟡 Medio | 5 |
| 🔵 Bajo | 2 |
| **Total** | **11** |

---

## HALLAZGOS DETALLADOS

### 🔴 [CRÍTICO] — Arquitectura — `TodoService` y `PlantillaService` saltan la capa `LogicaNegocio`

**Fichero:** [AppTodoList.Api/Services/TodoService.cs](../AppTodoList.Api/Services/TodoService.cs#L11) (línea 11, 14) y [AppTodoList.Api/Services/PlantillaService.cs](../AppTodoList.Api/Services/PlantillaService.cs#L9) (línea 9, 11)
**Descripción:** Ambos servicios inyectan `AppDbContext` directamente y ejecutan queries EF Core (`_context.TodoItems...`, `_context.Plantillas...`) dentro de la capa `Services/`. No existe `ITodoLogica`/`TodoLogica` ni `IPlantillaLogica`/`PlantillaLogica` en `LogicaNegocio/` — solo `CategoriaLogica` respeta el flujo documentado. Esto contradice explícitamente tanto `.github/copilot-instructions.md` (`Controller → [DTO] → Service → [Entidad] → LogicaNegocio → DbContext`) como el propio skill `servicio` ("El servicio no accede a `AppDbContext` directamente — eso es responsabilidad de la lógica de negocio").
**Riesgo:** Rompe la separación de responsabilidades del proyecto; mezcla orquestación, reglas de negocio (`ValidarTodo`, `CalcularProximaFecha`) y acceso a datos en la misma clase, dificultando el testeo aislado y la evolución del modelo de datos. Es un defecto estructural, no puntual: dos de los tres recursos principales (Tareas y Plantillas) no siguen el patrón de capas del proyecto.
**Acción requerida:** Extraer `ITodoLogica`/`TodoLogica` e `IPlantillaLogica`/`PlantillaLogica` (invocando el skill `logica-negocio`), moviendo el acceso a `AppDbContext` y las reglas de dominio ahí, dejando `TodoService`/`PlantillaService` como pura orquestación DTO ↔ entidad.

---

### 🔴 [CRÍTICO] — DTO / Seguridad de diseño (OWASP A04) — `PlantillasController` expone entidades de dominio directamente

**Fichero:** [AppTodoList.Api/Controllers/PlantillasController.cs](../AppTodoList.Api/Controllers/PlantillasController.cs#L38) (líneas 19, 26, 38, 52, 83)
**Descripción:** Todos los endpoints de `PlantillasController` reciben y devuelven `PlantillaTarea` y `TodoItem` (entidades de `AppTodoList.Models`) directamente en el cuerpo HTTP, en vez de DTOs. No existe ningún `PlantillaDto`, `GuardarPlantillaDto` en `Dtos/`. Esto viola el patrón DTO que sí se aplica correctamente en `TareasController` y `CategoriasController`, y contradice el skill `dto` ("los DTOs no incluyen propiedades de tipo entidad; el cliente nunca envía ni recibe entidades de dominio").
**Riesgo:** Mass assignment / over-posting: un cliente puede fijar `Id` u otras propiedades internas del dominio en el `POST`/`PUT`. Además, cualquier campo que se añada a `PlantillaTarea` en el futuro (p. ej. una relación sensible) se expondría automáticamente en la API sin control explícito. Es la misma clase de problema que el resto del proyecto ya evita con DTOs — aquí no se aplicó.
**Acción requerida:** Crear `PlantillaDto` / `GuardarPlantillaDto` (skill `dto`) y refactorizar `PlantillasController` y `IPlantillaService`/`PlantillaService` para trabajar con DTOs en la firma pública, igual que `TareasController`.

---

### 🟠 [ALTO] — Tests — Cobertura gravemente desigual entre recursos

**Fichero:** [AppTodoList.Api.Tests/Controllers/TareasControllerTests.cs](../AppTodoList.Api.Tests/Controllers/TareasControllerTests.cs#L1); no existe fichero equivalente para Plantillas.
**Descripción:** `TareasControllerTests` contiene **un único test** (`Create_ConCategoriaInexistente_DeberiaDevolverBadRequest`) — no hay tests de `GetAll`, `GetById`, `Create` exitoso, `Update`, `Delete` ni `Completar` a nivel de controlador. `PlantillasController` y `PlantillaService` **no tienen ningún test**, pese a ser el recurso con más riesgo (sin DTOs, sin capa de lógica separada, endpoint `Instanciar` con efectos secundarios de creación de `TodoItem`). En cambio, `CategoriasController` tiene 12 tests bien estructurados.
**Riesgo:** El recurso con más deuda arquitectónica (Plantillas) es también el que menos garantías de regresión tiene. Un cambio futuro en `PlantillaService.InstanciarAsync` o en `PlantillasController` no sería detectado por la suite actual.
**Acción requerida:** Añadir tests de controlador y de servicio para Plantillas (creación, actualización, eliminación, instanciación, casos 404/400) y completar la cobertura de `TareasControllerTests` para todos los verbos.

---

### 🟠 [ALTO] — Controladores — `CategoriasController.Create` no usa `CreatedAtAction`

**Fichero:** [AppTodoList.Api/Controllers/CategoriasController.cs](../AppTodoList.Api/Controllers/CategoriasController.cs#L43) (línea 43)
**Descripción:** `return Created($"/api/categorias/{creada.Id}", creada);` construye la URL de localización manualmente con interpolación de string, mientras que `TareasController.Create` y `PlantillasController.Create` usan `CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada)`. El patrón `CreatedAtAction` es además el que exige explícitamente el skill `controlador`.
**Riesgo:** La URL hardcodeada `/api/categorias/{id}` se desincroniza silenciosamente si cambia el prefijo de rutas, se versiona la API, o se añade un área/subdominio — no hay ningún error de compilación que lo detecte, a diferencia de `nameof(GetById)`. Es además una inconsistencia de estilo entre controladores generados por el mismo skill.
**Acción requerida:** Sustituir por `CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada)`.

---

### 🟡 [MEDIO] — Convenciones — No existen migraciones EF Core pese a estar documentadas

**Fichero:** `AppTodoList.Api/Program.cs` (línea 20, `dbContext.Database.EnsureCreated()`); no existe carpeta `Migrations/`.
**Descripción:** `.github/copilot-instructions.md`, sección "Build y Tests", documenta `dotnet ef migrations add <Nombre>` y `dotnet ef database update` como parte del flujo del proyecto, y el skill `base-de-datos` está diseñado para generar migraciones. En la práctica, `Program.cs` usa `EnsureCreated()` y no hay ninguna migración en el repositorio. `docs/plan-categorizar-tareas.md` confirma que es una decisión consciente ("la aplicación usa `Database.EnsureCreated()`, no migraciones de EF Core"), pero esa decisión **no está reflejada** en `copilot-instructions.md` ni en `docs/analisis-diseño.md`.
**Riesgo:** Documentación desincronizada con la implementación real: cualquier persona (o skill) que siga literalmente `copilot-instructions.md` ejecutará comandos de migración que no tienen efecto real sobre el esquema en producción, y `EnsureCreated()` no permite evolucionar el esquema de una base de datos ya creada sin borrarla.
**Acción requerida:** Actualizar `copilot-instructions.md` para reflejar el uso real de `EnsureCreated()`, o migrar a `dotnet ef migrations` si se quiere evolución de esquema sin pérdida de datos.

---

### 🟡 [MEDIO] — Convenciones — Stack de tests documentado ("xUnit + Moq") no se cumple

**Fichero:** [AppTodoList.Api.Tests/AppTodoList.Api.Tests.csproj](../AppTodoList.Api.Tests/AppTodoList.Api.Tests.csproj#L8)
**Descripción:** `.github/copilot-instructions.md` indica "Tests: xUnit + Moq", pero el `.csproj` de tests no referencia el paquete `Moq` en absoluto, y ningún test usa mocks: todos instancian un `AppDbContext` real contra SQLite en memoria (`Data Source=:memory:`).
**Riesgo:** No es necesariamente un defecto — los tests actuales son de integración y funcionan correctamente — pero la documentación del proyecto promete algo que el código no entrega, lo que puede inducir a error a futuras contribuciones o skills que planifiquen usando `Moq`.
**Acción requerida:** Actualizar la documentación para reflejar la estrategia real de tests de integración con SQLite en memoria, o introducir `Moq` donde tenga sentido (p. ej. tests de `TodoService`/`CategoriaService` que no necesiten BD real).

---

### 🟡 [MEDIO] — Diseño — Lógica de negocio mezclada en la capa `Services`

**Fichero:** [AppTodoList.Api/Services/TodoService.cs](../AppTodoList.Api/Services/TodoService.cs#L150) (métodos `ValidarTodo`, `ValidarCategoriaAsync`, `CalcularProximaFecha`) y [AppTodoList.Api/Services/PlantillaService.cs](../AppTodoList.Api/Services/PlantillaService.cs#L95) (métodos `ValidarPlantilla`, `CalcularProximaFecha`)
**Descripción:** Las reglas de negocio (validación de título obligatorio, validación de recurrencia, cálculo de próxima fecha según `TipoRecurrencia`) están implementadas como métodos privados en `Services/` en lugar de en `LogicaNegocio/`, contrario a la separación documentada. Es consecuencia directa del hallazgo crítico de arquitectura (ausencia de `TodoLogica`/`PlantillaLogica`).
**Riesgo:** Acopla reglas de dominio a la capa de orquestación; si mañana se necesita reutilizar `CalcularProximaFecha` desde otro flujo (p. ej. un job en background), habría que depender de la capa `Services` en vez de una capa de dominio pura.
**Acción requerida:** Mover al crear `TodoLogica`/`PlantillaLogica` (ver hallazgo crítico #1).

---

### 🟡 [MEDIO] — Duplicación — `CalcularProximaFecha` duplicado literalmente

**Fichero:** [AppTodoList.Api/Services/TodoService.cs](../AppTodoList.Api/Services/TodoService.cs#L191) y [AppTodoList.Api/Services/PlantillaService.cs](../AppTodoList.Api/Services/PlantillaService.cs#L104)
**Descripción:** El método privado `CalcularProximaFecha(TipoRecurrencia, DateTime)` es idéntico carácter por carácter en ambos ficheros.
**Riesgo:** Si se añade un nuevo valor a `TipoRecurrencia` o cambia la regla de cálculo (p. ej. recurrencia "Quincenal"), hay que recordar actualizar dos sitios; es fácil que queden desincronizados.
**Acción requerida:** Extraer a un método compartido (p. ej. en una futura clase de dominio o en `LogicaNegocio` común) una vez se resuelva el hallazgo crítico de arquitectura.

---

### 🟡 [MEDIO] — Tamaño de clase — `TodoService.cs` supera las 100 líneas recomendadas

**Fichero:** [AppTodoList.Api/Services/TodoService.cs](../AppTodoList.Api/Services/TodoService.cs#L1)
**Descripción:** La clase tiene 202 líneas, muy por encima del límite de "clases pequeñas y fáciles de leer en pantalla" que fija `copilot-instructions.md` y del checklist de esta auditoría (≤100 líneas). Es la clase más grande del backend.
**Riesgo:** Dificulta la lectura y el mantenimiento; síntoma directo de que la clase absorbe responsabilidades de tres capas (orquestación, reglas de negocio, acceso a datos) que deberían estar repartidas.
**Acción requerida:** Se resuelve automáticamente al extraer `TodoLogica` (hallazgo crítico #1): `TodoService` quedaría reducido a mapeo DTO ↔ entidad.

---

### 🔵 [BAJO] — Seguridad (OWASP A01) — Ausencia total de autenticación/autorización

**Fichero:** todos los controladores en `AppTodoList.Api/Controllers/`
**Descripción:** Ningún endpoint tiene `[Authorize]` ni existe configuración de autenticación en `Program.cs`. Todos los endpoints son públicos.
**Riesgo:** Bajo en el contexto actual (aplicación didáctica de un curso, sin datos sensibles ni multiusuario real), pero se marca para que quede documentado si la aplicación evoluciona hacia un escenario con datos de usuarios reales.
**Acción requerida:** Ninguna en el estado actual del curso; revisar si se introduce el concepto de usuario/autenticación en una fase posterior.

---

### 🔵 [BAJO] — Datos de siembra con fecha fija que quedará obsoleta

**Fichero:** [AppTodoList.Api/Data/AppDbContext.cs](../AppTodoList.Api/Data/AppDbContext.cs#L79) (líneas 79-96)
**Descripción:** El seeder (`HasData`) fija `CreatedAt`/`ProximaFecha` a fechas absolutas (`2026-09-15`, `2026-09-16`) en lugar de fechas relativas calculadas en tiempo de ejecución.
**Riesgo:** Es dato de demostración, sin impacto funcional real; solo se menciona porque con el paso del tiempo esas tareas de ejemplo aparecerán con fechas "pasadas" de forma permanente al recrear la base de datos.
**Acción requerida:** Ninguna obligatoria; cosmético para un seeder de demo.

---

## DEUDA TÉCNICA ACUMULADA

| Área | Descripción | Esfuerzo estimado |
|---|---|---|
| Arquitectura | Crear `ITodoLogica`/`TodoLogica` e `IPlantillaLogica`/`PlantillaLogica`, mover reglas de negocio y acceso a datos | 4-8h |
| DTOs | Crear `PlantillaDto`/`GuardarPlantillaDto` y refactorizar `PlantillasController`/`PlantillaService` | 1-4h |
| Tests | Completar `TareasControllerTests` y crear `PlantillasControllerTests`/`PlantillaServiceTests` | 4-8h |
| Documentación | Sincronizar `copilot-instructions.md` con el uso real de `EnsureCreated()` y la ausencia de `Moq` | 1-4h |
| Refactor menor | Unificar `CalcularProximaFecha`, corregir `CreatedAtAction` en `CategoriasController` | 1-4h |

---

## CUMPLIMIENTO DE CONVENCIONES

| Convención | Estado | Observación |
|---|---|---|
| Código en castellano | ✅ | Nombres de clases, métodos y variables consistentemente en español. |
| Inyección por constructor | ✅ | No hay `new` directo de dependencias en ningún controlador/servicio/lógica. |
| async/await en DB | ✅ | Todos los métodos de acceso a datos son `async Task<T>`. |
| Prefijo `I` en interfaces | ✅ | `ITodoService`, `IPlantillaService`, `ICategoriaService`, `ICategoriaLogica`, etc. |
| Controladores sin lógica | ✅ | Los tres controladores solo orquestan; ninguno contiene validaciones ni cálculos. |
| Clases ≤100 líneas | ⚠️ | `TodoService.cs` tiene 202 líneas (ver hallazgo medio). El resto de clases cumple. |
| Flujo Controller→DTO→Service→Entidad→LogicaNegocio→DbContext | ❌ | Solo se cumple para Categorías. Tareas y Plantillas saltan la capa `LogicaNegocio` (crítico); Plantillas además salta la capa DTO (crítico). |

---

## ANÁLISIS DE SKILLS

Se encontraron planes de implementación en `docs/plan-*.md` (`plan-categorizar-tareas.md`, `plan-crear-categoria-nueva.md`, `plan-editar-categoria-existente.md`, `plan-eliminar-categoria.md`, `plan-listado-categorias-pagina-gestion.md`), todos con sección "Skills a invocar". Sin embargo, **todos ellos son planes del recurso Categorías** (más `plan-categorizar-tareas.md`, que documenta la incorporación de `CategoriaId` a `TodoItem` pero no la creación original de `TodoItem`/`PlantillaTarea`).

> ⚠️ No existe ningún plan de implementación (`docs/plan-*.md`) que documente cómo se generaron originalmente los recursos **Tareas** y **Plantillas** (controlador, servicio, modelo). Por tanto, los hallazgos críticos #1 y #2 (ausencia de `TodoLogica`/`PlantillaLogica`, ausencia de DTOs en Plantillas) **no se pueden correlacionar con un skill concreto**: no hay evidencia de que se haya invocado `logica-negocio` o `dto` para esos dos recursos, ni de que un skill los haya generado incorrectamente. Es más probable que ese código sea anterior a la introducción de los skills `logica-negocio` y `dto` en el proyecto, o que se haya escrito/generado sin invocarlos.

### Skills auditados con trazabilidad (Categorías)

| Skill | Ficheros generados/tocados | Hallazgos asociados |
|---|---|---|
| `dto` | `Dtos/GuardarCategoriaDto.cs`, `Dtos/CategoriaDto.cs` | Ninguno — cumple validaciones `[Required]`/`[MaxLength]` según lo exigido por el propio skill. |
| `logica-negocio` | `LogicaNegocio/ICategoriaLogica.cs`, `LogicaNegocio/CategoriaLogica.cs` | Ninguno — es la única implementación del proyecto que sigue el patrón completo (`AsNoTracking`, validación de dominio, sin lógica HTTP). |
| `servicio` | `Services/ICategoriaService.cs`, `Services/CategoriaService.cs` | Ninguno — no accede a `AppDbContext`, delega correctamente en `ICategoriaLogica`. |
| `controlador` | `Controllers/CategoriasController.cs` | 🟠 Alto — `Create` no usa `CreatedAtAction` (línea 43), pese a que el propio skill `controlador` documenta ese patrón como obligatorio (`return CreatedAtAction(nameof(ObtenerPorId), ...)`). Es una desviación puntual de la implementación respecto al skill, no un defecto del skill. |

No se detectan **defectos sistemáticos en la definición** de los skills `dto`, `logica-negocio`, `servicio` o `controlador`: sus instrucciones son correctas y, de hecho, describen exactamente el patrón que falta en Tareas y Plantillas. El problema es de **aplicación inconsistente** (esos dos recursos no pasaron por los skills, o se implementaron antes de que existieran), no de contenido de los skills.

**Recomendación:** Antes de seguir añadiendo features a Tareas o Plantillas, ejecutar explícitamente `logica-negocio` sobre esos dos recursos y `dto` sobre Plantillas, usando `CategoriaLogica`/`CategoriaDto` como referencia de patrón correcto ya validado en este mismo repositorio.

---

## PRÓXIMOS PASOS RECOMENDADOS

1. **Crear `ITodoLogica`/`TodoLogica` e `IPlantillaLogica`/`PlantillaLogica`** invocando el skill `logica-negocio`, moviendo el acceso a `AppDbContext` fuera de `Services/` — resuelve 3 de los 5 hallazgos medios de un solo golpe.
2. **Crear DTOs para Plantillas** (`PlantillaDto`, `GuardarPlantillaDto`) invocando el skill `dto` y refactorizar `PlantillasController`/`PlantillaService` — cierra el riesgo de mass assignment.
3. **Completar la suite de tests** de Tareas y Plantillas antes de seguir añadiendo funcionalidad sobre esas capas, para no acumular más deuda sin red de seguridad.
4. Corregir `CategoriasController.Create` para usar `CreatedAtAction`.
5. Sincronizar `copilot-instructions.md` con la realidad (`EnsureCreated()` en vez de migraciones, ausencia de `Moq`).

---

*Auditoría generada por `@auditor-calidad` — Modo abogado del diablo activado.*
