---
name: analisis-peticion
description: |
  Actualiza la documentación de análisis con una petición concreta del usuario
  y la deja preparada para que el orquestador decida el alcance real del cambio.
license: MIT
metadata:
  version: "1.0.0"
---

# Skill de actualización del análisis

Usa esta habilidad cuando necesites:

- documentar una nueva petición funcional o de cambio
- dejar registrada la solicitud en `docs/analisis-diseño.md`
- preparar el contexto antes de que el orquestador decida el alcance
- evitar que el cambio se implemente sin haber quedado reflejado en la documentación

## Fuente de verdad

La fuente de verdad del proyecto sigue siendo:

- `README.md`
- `docs/analisis-diseño.md`
- `.github/copilot-instructions.md`

Este skill no debe inventar requisitos ni sustituir la documentación existente. Debe ampliar la documentación con la petición recibida y dejarla alineada con el proyecto.

## Regla principal

Este skill no genera implementación ni lógica de negocio.
Su objetivo es preparar la documentación y dejar claro el contexto antes de que el orquestador tome la decisión.

La secuencia correcta es:

1. documentar la petición
2. decidir el alcance con el orquestador
3. delegar la implementación a la capa adecuada

El orquestador es quien dirige el flujo; este skill solo prepara la entrada del cambio.

## Flujo de trabajo

1. Lee la documentación funcional del proyecto.
2. Identifica la sección más adecuada donde debe registrarse el cambio:
   - requisitos funcionales
   - funcionalidades añadidas
   - cambios solicitados
   - historial de evolución del análisis
3. Añade la petición con un formato claro y útil:
   - objetivo del cambio
   - descripción de la funcionalidad pedida
   - contexto del negocio
   - resultado esperado
   - impacto estimado por capa
4. Si el análisis ya tiene un bloque relacionado, actualiza ese bloque en lugar de duplicarlo.
5. Mantén el estilo existente del documento y no destruyas contenido previo.
6. Después de documentar la petición, invoca al skill `orquestador` para decidir el alcance real del cambio.
7. El orquestador debe determinar si el cambio es solo dominio, o si requiere expandirse a servicios, Data, controllers o frontend.

## Qué debe quedar documentado

Cada petición registrada debe responder a estas preguntas:

- ¿Qué se quiere conseguir?
- ¿Qué problema o necesidad cubre?
- ¿Qué comportamiento debe quedar definido?
- ¿Qué capas podrían verse afectadas?
- ¿Hay impacto en reglas del negocio, persistencia o experiencia de usuario?

## Qué evitar

- escribir requisitos vagos o sin contexto
- duplicar secciones en varios sitios
- alterar el análisis existente sin necesidad
- generar código sin pasar por la validación del orquestador
- mezclar la documentación de la petición con la implementación técnica aún no aprobada
- tratar al orquestador como si fuese la capa que implementa en lugar de la que coordina

## Criterio de finalización

La documentación estará correcta cuando:

- la petición queda reflejada con claridad en el análisis
- el contexto del cambio es comprensible para el proyecto
- la siguiente decisión ya no es “qué se quiere”, sino “qué capa corresponde”
- el orquestador puede decidir el alcance sin perder el contexto original

## Instrucción operativa final

Antes de implementar cualquier cambio:

1. actualiza `docs/analisis-diseño.md` con la petición recibida
2. conserva la documentación previa y añade el nuevo contexto de forma ordenada
3. llama al skill `orquestador`
4. deja que el orquestador decida el alcance y el orden de ejecución
5. no generes implementación ni código adicional antes de esa validación
