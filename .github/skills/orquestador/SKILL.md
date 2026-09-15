---
name: orquestador
description: |
  Coordina cambios del proyecto revisando la documentación funcional,
  determinando el alcance real del cambio y delegando a las capas correctas
  sin crear sobreingeniería ni duplicar responsabilidades.
license: MIT
metadata:
  version: "1.0.0"
---

# Skill orquestador

Usa esta habilidad cuando necesites:

- decidir si un cambio pertenece solo al dominio o requiere ampliar a otras capas
- revisar el alcance antes de generar código
- coordinar la interacción entre documentación, modelo, pruebas y arquitectura
- evitar scope creep y sobreingeniería
- mantener la separación de responsabilidades entre capas

## Fuente de verdad

La fuente de verdad del proyecto es la documentación funcional y de arquitectura:

- `README.md`
- `docs/analisis-diseño.md`
- `.github/copilot-instructions.md`

Si hay duda, la documentación del proyecto tiene prioridad sobre la intuición del agente o sobre la comodidad de generar código de trabajo.

## Regla principal

No crear más código del que se ha definido en el alcance real.

El dominio ya existe. El orquestador debe validar primero si la petición corresponde:

- solo al dominio
- al dominio y a servicios
- a Data / EF Core
- a controllers / API
- a frontend / UX

Si el cambio no está claramente definido, debe mantenerse el alcance actual y evitar expandir sin necesidad.

## Principios de coordinación

- el orquestador dirige el flujo y la secuencia de trabajo
- el orquestador decide el alcance y delega a la capa correcta
- el orquestador no sustituye al modelo, a servicios, a Data, a controllers ni al frontend
- el modelo es la base del negocio y ya está establecido
- no se debe redefinir el dominio por costumbre ni por conveniencia
- la lógica de negocio no vive en controladores ni en frontend
- la separación por capas debe mantenerse siempre
- cada capa nueva debe ir acompañada de pruebas relevantes
- una funcionalidad solo debe generar código cuando exista un requisito real y verificable

## Rol del orquestador

El orquestador es el coordinador del cambio, no el implementador universal.
Su responsabilidad es:

- revisar la documentación funcional y de arquitectura
- decidir qué capa corresponde al cambio
- ordenar la ejecución del trabajo
- delegar en el skill o capa adecuada
- impedir scope creep, duplicación y sobreingeniería

La intención no es que el orquestador haga todo, sino que dirija todo.
El trabajo real debe ejecutarlo la capa especializada que corresponda en cada caso.

## Flujo de trabajo recomendado

1. iniciar con `analisis-peticion` para registrar la petición en la documentación del análisis
2. leer `README.md`, `docs/analisis-diseño.md` y la guía operativa del repositorio
3. identificar si la necesidad es funcional, técnica o de arquitectura
4. decidir si afecta solo dominio o si requiere ampliar a otra capa
5. si la petición necesita documentarse antes de implementar, dejarla registrada antes de cualquier código
6. si es solo dominio, delegar al skill `modelo`
7. si requiere más capas, definir el alcance exacto y el orden correcto:
   - dominio
   - servicios
   - persistencia / Data
   - controllers
   - frontend
8. preparar los cambios mínimos necesarios para cumplir el requisito
9. preparar pruebas asociadas al comportamiento real que se protege, delegando al skill `pruebas`
10. si el cambio toca documentación, mensajes o texto del proyecto, revisar el estilo con `humanizer`
11. si el cambio afecta skills locales, scripts o ejecución no trivial, ejecutar una auditoría con `security-audit`
12. validar con `dotnet test` cuando corresponda
13. confirmar que no se ha generado código extra no solicitado

## Decisión por capa

### Caso 1: cambio de dominio

Cuando el cambio define o ajusta el negocio y no introduce infraestructura ni flujo de aplicación, la decisión debe ir al skill `modelo`.

Ejemplos:

- revisión de entidades del negocio
- ajustes de propiedades del dominio
- nuevas reglas del negocio que siguen siendo del modelo

### Caso 2: cambio de negocio con lógica de aplicación

Cuando existe lógica de comportamiento que no pertenece al dominio puro, se debe expandir a servicios.

Ejemplos:

- recurrencia
- generación de la siguiente ocurrencia
- validación de negocio que no es solo de propiedad

### Caso 3: cambio con persistencia

Cuando hay efectos de base de datos, EF Core o SQLite, se amplía a Data.

Ejemplos:

- cambios de esquema
- entidades persistidas
- migraciones
- consultas y relaciones

### Caso 4: cambio con API

Cuando el usuario o el requisito necesita exponer una operación a través de HTTP, el cambio va a controllers.

Ejemplos:

- endpoints REST
- contratos de entrada/salida
- orquestación HTTP

### Caso 5: cambio con UX

Cuando la funcionalidad final afecta a la experiencia del cliente, se expande al frontend, pero solo después de que la capa de negocio tenga un contrato claro.

## Qué evitar

- crear servicios sin necesidad
- añadir Data sin un requisito real
- crear endpoints antes de definir la regla de negocio
- duplicar la lógica en frontend y backend
- añadir pruebas de estructura en vez de pruebas de comportamiento
- generar código “por si acaso”

## Reglas de pruebas y validación

La prueba no es opcional cuando hay un cambio funcional verificable.

Cuando el orquestador detecte un alcance real, debe delegar a `pruebas` para:

- validar caso feliz
- validar caso límite o inválido
- validar comportamiento real del sistema
- evitar pruebas basadas solo en mocks

Además, el orquestador debe distinguir entre:

- flujo core del proyecto: `analisis-peticion` → `orquestador` → `modelo` → `pruebas`
- flujo opcional de seguridad: `security-audit` cuando se tocan habilidades locales, scripts o ejecución externa
- flujo opcional de calidad textual: `humanizer` cuando se editan documentos, mensajes o descripciones del proyecto

Esto mantiene el flujo correcto sin mezclar responsabilidades ni crear trabajo duplicado.

## Criterio de finalización

Un cambio está bien orquestado cuando:

- la documentación ha sido revisada
- el alcance se ha definido antes de generar código
- el orquestador ha dirigido la secuencia y la delegación correcta
- las capas implicadas son las mínimas necesarias
- el dominio no se ha duplicado ni mezclado con la API
- las pruebas cubren el comportamiento real y no la implementación
- la solución sigue siendo simple, explícita y fácil de mantener

En resumen: el orquestador es el director del flujo, no el reemplazo de cada especialidad.

## Instrucción operativa final

Antes de implementar una nueva funcionalidad:

1. revisa la documentación del proyecto
2. identifica si el cambio es de dominio o de arquitectura
3. decide la capa exacta y el orden correcto
4. delega en el skill correspondiente
5. no generes código extra ni capas no solicitadas
6. asegura que la validación se realice con pruebas de comportamiento real
