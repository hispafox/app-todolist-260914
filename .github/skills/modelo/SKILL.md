---
name: modelo
description: |
  Genera y actualiza el modelo de dominio de la aplicación.
  Lee la documentación del proyecto, especialmente README.md y
  docs/analisis-diseño.md, y crea o modifica las clases de Models/
  sin duplicar la entidad dentro del propio skill.
license: MIT
metadata:
  version: "1.0.0"
---

# Skill de dominio: modelo de datos

Usa esta habilidad cuando necesites:

- crear el modelo de dominio del proyecto
- actualizar el modelo y sus elementos relacionados solo cuando realmente existan cambios
- mantener `Models/` alineado con la fuente de verdad del negocio
- generar clases de dominio sin duplicar la información en el propio skill
- evitar crear capas o archivos extra si no hay elementos relacionados que actualizar

## Fuente de verdad

Nunca infieras el modelo desde memoria ni lo escribas a mano dentro del skill.
La fuente de verdad es esta documentación del proyecto:

- `README.md`
- `docs/analisis-diseño.md`

En particular, la sección 4 del análisis, "Modelo de datos", define qué entidades hay, sus propiedades y sus reglas.

## Regla principal

Este skill no debe listar entidades fijas ni campos hardcodeados dentro de la propia instrucción.
El modelo no debe duplicarse dentro del skill, porque si cambian las entidades del análisis habrá que cambiar este archivo constantemente.

Debe leer el documento y generar o actualizar el modelo a partir de ese análisis cada vez que se ejecute.

Si el diseño cambia en el documento, el modelo debe actualizarse sin tocar este skill.

Si existen elementos relacionados al modelo (por ejemplo, entidades auxiliares, anotaciones o tipos derivados), se actualizan solo cuando realmente formen parte del dominio y existan en el análisis. Si no hay elementos relacionados, el skill solo crea o actualiza el modelo sin generar capas adicionales ni archivos innecesarios.

Además, el modelo de datos debe vivir en un proyecto separado del proyecto principal de la aplicación, para poder integrarse después en la solución. No se debe mezclar con la API, la lógica de negocio ni el frontend.

## Objetivo

Crear y mantener la capa de modelos de dominio de la aplicación, con una estructura simple, clara y alineada con el análisis del proyecto, y garantizando que ese código resida en un proyecto de dominio independiente listo para incorporarse en la solución.

## Estructura esperada

- un proyecto de modelos independiente, por ejemplo `AppTodoList.Models` o un proyecto equivalente de dominio
- dentro de ese proyecto, una carpeta `Models/` para las entidades del negocio
- un archivo por entidad o enum cuando aplique
- namespace consistente con el proyecto
- las clases de modelo suelen estar en carpetas o proyectos concretos; usar esa ubicación definida por el proyecto, no inventar carpetas nuevas si no existen
- la solución principal incorporará ese proyecto más adelante; el skill no debe crear el modelo dentro del proyecto web ni dentro de la API

Ejemplo esperable:

```csharp
namespace AppTodoList.Models;
```

## Orden de generación

Cuando se creen entidades nuevas, respeta este orden por dependencias:

1. `TipoRecurrencia` (enum)
2. `PlantillaTarea`
3. `TodoItem`

Esto mantiene una jerarquía coherente y evita referencias cruzadas innecesarias.

## Reglas de generación

- Usa la definición del dominio en `docs/analisis-diseño.md` como referencia única.
- No inventes campos, nombres o relaciones que no estén definidos allí.
- No copies las entidades actuales dentro del skill; la fuente de verdad es el análisis.
- Mantén nombres de clases y propiedades alineados con el dominio actual del proyecto.
- Crea o actualiza el modelo dentro de un proyecto separado de la API; nunca lo generes como parte del proyecto web principal.
- Si el proyecto de modelos no existe, créalo como una librería de clases o proyecto de dominio independiente que luego se añadirá a la solución.
- Solo actualiza elementos relacionados cuando existan y estén definidos por el dominio; si no hay elementos relacionados, crea o actualiza únicamente el modelo.
- No fuerces la creación de servicios, controladores, DbContext, DTOs ni archivos adjuntos si no forman parte del modelo y no están requeridos por el análisis.
- Usa `string.Empty` en propiedades `string` requeridas cuando haga falta un valor por defecto.
- `PlantillaId` debe ser nullable.
- `TodoItem` puede tener relación opcional con `PlantillaTarea`.
- La lógica de recurrencia no va en el modelo; va en la capa de servicios.
- No generes controladores, servicios ni DbContext a menos que se te pida expresamente.
- Si falta una entidad nueva en el análisis, créala; si una entidad desaparece del análisis, elimina o actualiza aquella que ya no corresponda.
- Se debe priorizar la claridad sobre la sofisticación.

## Entidades esperadas del proyecto

Consulta siempre el documento de análisis para confirmar la definición exacta actual. En el diseño actual del proyecto, las entidades esperadas son:

- `TipoRecurrencia` (enum con `Diaria`, `Semanal`, `Mensual`)
- `PlantillaTarea`
- `TodoItem`

## Criterio de validación

Tras generar o actualizar el modelo, comprueba que:

- los archivos en `Models/` reflejan el documento de análisis
- no se han añadido entidades no definidas
- hay coherencia entre nombres, tipos, nulos y relaciones
- el proyecto compila sin errores

## Resultado esperado

El output debe ser un proyecto de modelos independiente y consistente con la documentación del proyecto, con el dominio expresado de forma clara y real, listo para incorporarse luego a la solución principal. La entidad no debe quedar mezclada dentro de la API ni del proyecto ejecutable del backend.

## Ejemplo de estilo esperado

```csharp
public enum TipoRecurrencia
{
    Diaria,
    Semanal,
    Mensual
}

public class PlantillaTarea
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public bool EsRepetitiva { get; set; }
    public TipoRecurrencia? Recurrencia { get; set; }
}

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool EsRepetitiva { get; set; }
    public TipoRecurrencia? Recurrencia { get; set; }
    public DateTime? ProximaFecha { get; set; }
    public int? PlantillaId { get; set; }
    public PlantillaTarea? Plantilla { get; set; }
}
```

## Instrucción operativa final

Antes de generar o actualizar el modelo:

1. lee `README.md`
2. lee `docs/analisis-diseño.md`
3. identifica la sección 4 del análisis como fuente de verdad
4. crea o actualiza el modelo en un proyecto separado del proyecto principal cuando corresponda
5. actualiza solo elementos relacionados si realmente existen y forman parte del dominio
6. si no hay elementos relacionados, crea o actualiza solo el modelo y no generes capas extra
7. no listes entidades a mano dentro de este skill
8. mantén la solución simple y alineada con la arquitectura del proyecto
