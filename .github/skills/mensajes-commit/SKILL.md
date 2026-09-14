---
name: mensajes-commit
description: 'Redacta mensajes de commit precisos para este repositorio. Usa esta skill cuando necesites resumir cambios concretos con formato tipo(ambito): descripcion corta en castellano y en minúsculas, y añadir un cuerpo opcional solo si aporta claridad.'
---

# Mensajes de commit para este proyecto

## Regla principal

Escribir un resumen específico y útil, no genérico.

Un buen mensaje de commit debe decir exactamente qué se ha tocado, no solo que "algo se actualizó".

## Formato obligatorio del resumen

Usa este patrón:

`tipo(ambito): descripcion corta`

- El texto va en español.
- En minúsculas.
- Sin punto final.
- Debe ser concreto: mencionar la parte del sistema o el cambio real.

### Ejemplos correctos

- `docs: corregir endpoints y ejemplos de la api de tareas`
- `feat(tareas): añadir plantilla para crear tareas repetitivas`
- `fix(services): evitar duplicar la siguiente ocurrencia al completar una tarea`
- `test(plantillas): cubrir instanciación desde plantilla en casos límite`
- `refactor(data): separar configuración del contexto de sqlite`

### Ejemplos incorrectos

- `actualizar fichero`
- `corregir errores`
- `mejorar frontend`
- `arreglar proyecto`
- `actualizar instrucciones`

## Cuerpo del commit

El cuerpo es opcional, pero cuando aporta valor hay que usarlo.

- La primera línea sigue siendo el resumen.
- Después va una línea en blanco.
- Luego se explica el detalle del cambio en una o varias líneas.
- Si el cambio es trivial, una sola línea basta.

### Ejemplo con cuerpo

```text
feat(tareas): añadir plantilla para crear tareas repetitivas

- se añade la entidad PlantillaTarea y su servicio para crear instancias
- la tarea repetitiva calcula la siguiente ocurrencia al completarla
- se actualiza el controlador de tareas para exponer el flujo de completado
```

### Ejemplo de un cambio trivial

```text
docs: corregir nombres de endpoints en la documentación
```

## Qué priorizar

1. Exactitud: decir qué se cambió, no solo que hubo un cambio.
2. Especificidad: nombrar el componente o área afectada.
3. Brevedad: corto, pero no vago.
4. Claridad: el mensaje debe entenderse sin contexto extra.

## Estrategia para redactarlo bien

Antes de escribir el commit, responde estas preguntas:

- ¿Qué módulo o área se tocó exactamente?
- ¿Qué comportamiento cambió?
- ¿Qué problema resuelve o qué funcionalidad añade?
- ¿Es necesario un cuerpo para dejar claro el detalle?

Si la respuesta es "actualicé un archivo" o "arreglé cosas", el mensaje no es suficiente.

## Reglas de no

- No uses verbos generales sin especificar el objeto.
- No escribas mensajes demasiado amplios como "actualizar documentación".
- No mezcles varios cambios sin relación en un mismo commit.
- No conviertas el cuerpo en un ensayo largo si el cambio es pequeño.
- No cambies el formato recomendado por el repositorio.

## Formato preferido para este repositorio

Mantén el estilo:

`tipo(ambito): descripcion corta`

Con `tipo` normalmente entre:

- `feat` para nuevas funcionalidades
- `fix` para correcciones
- `docs` para documentación
- `test` para pruebas
- `refactor` para limpieza o reorganización
- `chore` para tareas de mantenimiento menores

La clave es que la descripción sea exacta y útil, no genérica.
