# Inventario y propuesta de skills para App Todo List

## 1. Objetivo

Este documento recoge una propuesta de skills para automatizar tareas repetitivas del desarrollo del proyecto App Todo List, manteniendo la arquitectura y la separación de capas recomendadas por la documentación del repositorio.

La idea no es añadir complejidad artificial, sino identificar automatizaciones útiles para:

- coordinar cambios funcionales,
- validar reglas de negocio,
- proteger la capa API,
- mantener la integridad del esquema SQLite,
- sincronizar backend y frontend,
- reducir errores de documentación y regresión.

---

## 2. Contexto del proyecto

El proyecto se compone de:

- backend en ASP.NET Core 10 con Controllers,
- acceso a datos con Entity Framework Core 10 y SQLite,
- lógica de negocio en servicios,
- capa de dominio en modelos,
- frontend en React + Vite + TypeScript,
- pruebas unitarias con xUnit + Moq.

La documentación de referencia del proyecto es:

- README.md
- docs/analisis-diseño.md
- .github/copilot-instructions.md

La separación por capas es una regla clave del proyecto. La lógica de negocio no debe vivir ni en el controller ni en el frontend. La recurrencia y la generación de la siguiente ocurrencia deben quedar en la capa de servicios.

---

## 3. Inventario de skills ya presentes en el repositorio

Se ha identificado una base de skills útil ya integrada en el proyecto:

| Skill existente | Uso actual | Encaja con este proyecto |
|---|---|---|
| orquestador | Coordina alcance, capas y flujo de trabajo | Sí |
| pruebas | Genera y valida pruebas funcionales | Sí |
| modelo | Mantiene entidades del dominio | Sí |
| analisis-peticion | Registra la petición en la documentación | Sí |
| humanizer | Mejora texto, documentación y mensajes | Sí |
| mensajes-commit | Estilo de commits | Sí |
| security-audit | Revisión de scripts y skills locales | Sí |
| find-skills | Búsqueda de skills | Sí |

Estas skills ya cubren bien la dirección del flujo de trabajo y la calidad del código. El punto de mejora está en automatizaciones más especializadas para este dominio concreto.

---

## 4. Diagnóstico: dónde el flujo actual necesita apoyo

Tras revisar el repositorio, los puntos donde mejor encaja una skill nueva son:

### 4.1. API y contratos

El proyecto define endpoints REST muy específicos bajo `/api/tareas` y `/api/plantillas`. Hay un riesgo de que el controller, los nombres de ruta o la estructura esperada se desalineen con la documentación funcional.

### 4.2. Lógica de negocio central

La lógica de tareas repetitivas, validación y generación de la siguiente ocurrencia está en la capa de servicios. Esa parte es perfecta para una skill que valide el comportamiento real y las reglas del dominio.

### 4.3. Persistencia SQLite / EF Core

El proyecto tiene una advertencia explícita de no borrar la base de datos ni recrearla manualmente si hay un esquema desactualizado. Esto hace muy útil una skill de enfoque de seguridad y migración.

### 4.4. Frontend y backend sincronizados

El frontend es una SPA separada y consume la API. La separación es clara, pero también es un punto fácil de romper si cambia el contrato de la API o la estructura de datos.

### 4.5. Calidad del cambio antes de merge

El proyecto necesita validar que los cambios no rompan capas, tests o documentación. Lo ideal es una skill de gate de calidad local antes de aceptar un cambio.

---

## 5. Propuesta de skills nuevas

### 5.1. api-contract-validator

#### Propósito
Validar que los endpoints, rutas, verbos, códigos de respuesta y estructura de salida del backend están alineados con la documentación funcional del proyecto.

#### Cuándo activarse
- se crea o modifica un controller,
- se añade un endpoint nuevo,
- se cambian rutas o nombres del recurso,
- se altera la API de tareas o plantillas.

#### Qué automatiza
- compara el controller contra la documentación del proyecto,
- valida nombres de rutas y verbos HTTP,
- revisa que los errores HTTP no contradicen la especificación,
- comprueba que la operación no mezcle lógica de negocio con la orquestación HTTP,
- señala inconsistencias de contrato antes de que lleguen al frontend.

#### Resultado esperado
Una revisión rápida de si el endpoint cumple la intención del análisis y no rompe la arquitectura del proyecto.

#### Ejemplo de activación
- “He añadido un endpoint para completar tarea, revisa la API”
- “Valida si el controller de tareas está alineado con el análisis”

---

### 5.2. todo-business-rules

#### Propósito
Auditar y reforzar las reglas del dominio de tareas, especialmente las relacionadas con la recurrencia y la creación de tareas desde plantillas.

#### Cuándo activarse
- se toca la lógica de negocio en servicios,
- se modifica la completitud de tareas,
- se cambia la recurrencia,
- se introduce o ajusta una plantilla.

#### Qué automatiza
- revisa que el título sea obligatorio,
- valida que una tarea repetitiva tenga recurrencia,
- comprueba la generación de la siguiente ocurrencia al completar,
- asegura que la lógica de recurrencia sigue dentro del servicio,
- valida que `TodoService` no se convierta en un controlador o una capa de presentación.

#### Resultado esperado
Un chequeo de las reglas clave del negocio antes de cerrar la implementación.

#### Ejemplo de activación
- “Revisa la regla de recurrencia para tareas repetitivas”
- “Haz una auditoría de validaciones y completado de tareas”

---

### 5.3. ef-core-sqlite-safety

#### Propósito
Proteger la base de datos SQLite del proyecto y evitar cambios destructivos o incompatibles en el esquema.

#### Cuándo activarse
- se modifica `AppDbContext`,
- se añaden entidades, propiedades o relaciones,
- se toca la persistencia o el esquema de SQLite,
- se sospecha que hay un problema de migración o desalineación del modelo.

#### Qué automatiza
- identifica qué entidades y propiedades cambiaron,
- recomienda `ALTER TABLE`, `CREATE TABLE IF NOT EXISTS` o migración antes que borrar la base de datos,
- revisa nombres de propiedades, anulabilidad, claves foráneas y relaciones,
- valida que el cambio no rompa la persistencia de la app,
- previene soluciones destructivas como recrear la BD a mano.

#### Resultado esperado
Cambios seguros y alineados con la política operativa del repositorio: no borrar BD ni provocar regresiones de persistencia.

#### Ejemplo de activación
- “Revisa si este cambio de modelo requiere una migración”
- “Protege la base de datos SQLite antes de modificar el DbContext”

---

### 5.4. frontend-backend-sync

#### Propósito
Mantener el frontend y la API sincronizados, evitando que el cliente de React invoque rutas o propiedades desalineadas con el backend.

#### Cuándo activarse
- se modifican controllers o endpoints,
- se cambia la estructura del modelo de tareas,
- se trabaja en servicios del frontend,
- se revisa consumo de la API desde React.

#### Qué automatiza
- compara la API pública con los servicios del cliente,
- revisa que los nombres de campos coincidan con el backend,
- valida la estrategia de llamadas a endpoints,
- detecta si el cliente está duplicando lógica de negocio del backend,
- ayuda a mantener un contrato claro entre frontend y API.

#### Resultado esperado
Un cliente más estable y menos propenso a errores de integración.

#### Ejemplo de activación
- “Revisa si el frontend consume bien la API de tareas”
- “Validar sincronía entre React y backend para plantillas y tareas”

---

### 5.5. quality-gate-local

#### Propósito
Crear una validación mínima y reproducible antes de aceptar cambios funcionales en el proyecto.

#### Cuándo activarse
- antes de una rama o PR,
- cuando se ha hecho un cambio funcional relevante,
- tras tocar backend, servicios o frontend.

#### Qué automatiza
- ejecuta la verificación mínima del proyecto,
- revisa que el backend compile,
- valida que el frontend compile,
- comprueba tests relacionados con el cambio,
- identifica si hay riesgo de romper el flujo por capas.

#### Resultado esperado
Un “gate” útil para no cerrar cambios sin verificación mínima real.

#### Ejemplo de activación
- “Haz una validación antes de cerrar esta funcionalidad”
- “Revisa si este cambio pasa el gate del proyecto”

---

### 5.6. docs-sync-check

#### Propósito
Mantener la documentación del proyecto sincronizada con el código real y con la intención funcional del dominio.

#### Cuándo activarse
- se introducen nuevas entidades, rutas o reglas,
- se cambian endpoints o comportamiento,
- se quiere mantener README y análisis actualizados.

#### Qué automatiza
- compara cambios del código con los documentos,
- detecta divergencias entre implementación y análisis,
- sostiene que la documentación funcional sigue siendo la fuente de verdad,
- ayuda a evitar que código y diseño se separen.

#### Resultado esperado
Documentación más fiable y menos propensa a volverse obsoleta.

#### Ejemplo de activación
- “Comprueba si el cambio está documentado”
- “Sincroniza el análisis con la implementación”

---

### 5.7. ui-form-validation-review

#### Propósito
Revisar la validación, UX y mensajes de error de formularios de frontend y flujo de creación/edición de tareas.

#### Cuándo activarse
- se trabaja la capa cliente,
- se añaden formularios o validaciones,
- se mejora la experiencia de usuario de tareas y plantillas.

#### Qué automatiza
- revisa campos obligatorios,
- valida mensajes de error claros y coherentes,
- evita que el frontend haga validaciones que no existan en backend,
- revisa consistentencia visual y de flujo entre crear, editar y completar tareas.

#### Resultado esperado
Una experiencia de usuario más consistente y menos propensa a errores de validación.

#### Ejemplo de activación
- “Revisa la validación de formulario de creación de tareas”
- “Asegura mensajes deerror claros para plantillas y tareas”

---

## 6. Priorización recomendada

### Fase 1: esenciales

1. api-contract-validator
2. todo-business-rules
3. ef-core-sqlite-safety
4. frontend-backend-sync

Estas cuatro responden directamente al diseño real del proyecto y reducen el riesgo funcional más alto.

### Fase 2: de calidad y mantenimiento

5. quality-gate-local
6. docs-sync-check
7. ui-form-validation-review

Estas son muy útiles para automatizar revisión continua y mantener la salud del proyecto a largo plazo.

---

## 7. Criterio de desarrollo de cada skill

Cada skill debe cumplir este patrón mínimo:

- tener un nombre claro y específico,
- describir un único tipo de automatización,
- activarse solo cuando haga falta,
- basarse en la documentación y arquitectura del repositorio,
- no duplicar lógica ni mezclar capas,
- reforzar la separación dominio / servicios / Data / API / frontend,
- estar pensada para trabajo local y repositorio.

---

## 8. Recomendación final

La mejor estrategia es no crear una skill “genérica para todo”, sino un conjunto de skills pequeñas y especializadas que encajen con la arquitectura del proyecto.

En este repositorio, la mayor rentabilidad la aportan las skills que:

- validan la API,
- protegen la lógica de negocio del dominio,
- controlan el esquema de persistencia SQLite,
- y mantienen sincronia entre frontend y backend.

Este enfoque preserva la claridad del proyecto y mantiene el flujo de trabajo alineado con el análisis y el diseño original.

---

## 9. Siguiente paso sugerido

A partir de esta propuesta, el siguiente desarrollo recomendado sería:

1. crear la skill `api-contract-validator`,
2. crear la skill `todo-business-rules`,
3. crear la skill `ef-core-sqlite-safety`,
4. dejar la restar como evolución posterior o backlog técnico.

Con esto se cubren la mayor parte de los riesgos funcionales del proyecto sin inflar la base de habilidades ni desordenar la arquitectura.
