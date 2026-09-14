---
name: domain-design
description: |
  Guía de diseño del dominio para una aplicación de gestión de tareas.
  Ayuda a identificar entidades, reglas del negocio, validaciones,
  recurrencia y separación entre dominio, servicios y controladores.
license: MIT
metadata:
  version: "1.0.0"
---

# Diseño del dominio para la app de tareas

Usa esta habilidad cuando necesites:

- definir entidades del dominio y sus reglas
- decidir qué va en modelos, servicios o controladores
- revisar la lógica de recurrencia y plantillas
- mantener la arquitectura simple y legible
- evitar sobreingeniería o patrones complejos innecesarios

## Fuente de verdad

Esta app debe seguir primero la documentación funcional del proyecto, especialmente el análisis y diseño del dominio. El objetivo no es añadir abstracciones sofisticadas, sino mantener un modelo claro y explícito.

## Principios clave

- Mantener el dominio simple y expresivo.
- Separar claramente modelos, acceso a datos, servicios y API.
- La lógica de negocio vive en servicios, no en controladores ni en modelos.
- Evitar mezclar responsabilidades entre capas.
- No introducir patrones complejos si el problema es pequeño y claro.
- Priorizar legibilidad y mantenibilidad sobre sofisticación.

## Entidades del dominio

### TodoItem
Representa una tarea personal. Debe tener claridad sobre:

- Id
- Title
- Description (si aplica)
- IsCompleted
- CreatedAt
- EsRepetitiva
- Recurrencia
- ProximaFecha
- PlantillaId

Reglas:

- una tarea puede ser creada manualmente o a partir de una plantilla
- `PlantillaId` debe ser nullable
- el título debe ser obligatorio
- si la tarea es repetitiva, la siguiente ocurrencia debe generarse al completarla

### PlantillaTarea
Es una entidad independiente que encapsula la definición reutilizable de una tarea.

Reglas:

- no debe estar embebida dentro de `TodoItem`
- debe permitir crear nuevos elementos reutilizando valores predefinidos
- la instanciación debe producir una tarea con los datos de la plantilla

### TipoRecurrencia
Es un enum con los valores:

- Diaria
- Semanal
- Mensual

## Reglas de negocio del dominio

### Recurrencia
La recurrencia no es una preocupación del controlador. Debe resolverse en el servicio.

Cuando una tarea repetitiva se completa:

1. se marca como completada
2. se calcula la próxima ocurrencia
3. la nueva fecha se obtiene según el tipo de recurrencia
4. se persiste la nueva programación de forma consistente

### Plantillas
Las plantillas deben servir para generar tareas con valores predefinidos, no para mezclar lógica con los modelos de tarea.

### Validación
Las entidades y servicios deben validar lo que realmente define el negocio:

- título requerido
- recurrencia válida
- fechas coherentes
- no crear estados inconsistentes al completar tareas

## Arquitectura recomendada

- `Models/`: entidades del dominio
- `Data/`: EF Core y persistencia
- `Services/`: lógica de negocio e interacción con el dominio
- `Controllers/`: orquestación HTTP únicamente
- `frontend/`: cliente React separado, sin acceso directo a la base de datos

## Qué evitar

- poner lógica de negocio dentro de `Controllers`
- duplicar reglas de negocio en frontend y backend
- introducir agregados o servicios complejos para una app pequeña
- mezclar entidades de presentación con entidades del dominio
- crear lógica de validación en varios niveles sin un punto claro de responsabilidad

## Preguntas útiles al diseñar cambios

Antes de implementar una nueva funcionalidad, responde estas preguntas:

1. ¿Qué hecho del negocio representa esta entidad?
2. ¿Qué regla no puede romperse?
3. ¿Debe vivir en el dominio o en la capa de aplicación?
4. ¿Hay una operación con efecto secundario, como completar una tarea repetitiva?
5. ¿Esto está creando una responsabilidad extra en el controlador?

## Criterio de decisión

Si existe duda sobre arquitectura o persistencia, prioriza una solución simple, explícita y alineada con ASP.NET Core, controllers, EF Core y SQLite. No conviertas la app en un diseño complejo si no lo necesita.

## Resultado esperado

El dominio debe ser claro para que un desarrollador pueda entender:

- qué es una tarea
- qué es una plantilla
- cómo se repite una tarea
- qué ocurre al completarla
- dónde vive cada regla

Si la intención del código es evidente sin leer miles de líneas, el diseño del dominio está bien orientado.
