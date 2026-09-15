---
name: pruebas
description: |
  Genera y valida pruebas para cambios funcionales de la aplicación.
  Debe cubrir comportamiento real del dominio, servicios y validaciones,
  evitando pruebas artificiales basadas solo en mocks.
license: MIT
metadata:
  version: "1.0.0"
---

# Skill de validación: pruebas funcionales

Usa esta habilidad cuando necesites:

- añadir o actualizar pruebas para una funcionalidad nueva o modificada
- asegurar que un cambio queda cubierto por comportamiento real
- evitar tests que solo validen llamadas a mocks o estructura interna
- mantener la suite de pruebas alineada con la documentación del proyecto
- validar que la lógica de negocio cumple los requisitos del dominio

## Fuente de verdad

La fuente de verdad para las pruebas es la documentación funcional del proyecto:

- `README.md`
- `docs/analisis-diseño.md`
- `.github/copilot-instructions.md`

En particular, la sección de Testing del proyecto define la regla general:

- cuando se agregue una funcionalidad, incluir pruebas adecuadas
- preferir pruebas unitarias para servicios y validación
- probar casos normales y casos límite
- no crear pruebas que solo validen mocks sin verificar el comportamiento real
- usar xUnit + Moq como stack de pruebas del proyecto

## Regla principal

No generes tests por costumbre ni por “completar la tarea”.
Cada prueba debe proteger un comportamiento real y verificable del sistema.

Si una funcionalidad no tiene un comportamiento observable que validar, no hace falta generar tests de relleno.

## Objetivo

Crear pruebas claras, útiles y realistas para la lógica de negocio, validaciones y reglas del dominio del proyecto, sin depender de mocks para afirmar que el comportamiento existe.

## Reglas de generación

- Usa el comportamiento real esperado como criterio de validación.
- Cubre al menos: caso feliz, caso límite o inválido, y regresión relevante si aplica.
- Prioriza servicios y validación sobre pruebas de estructura interna.
- Evita pruebas que solo verifiquen que se invocó a un método o a un mock.
- Si se usa un mock, debe representar una dependencia externa y no sustituir la lógica que se está validando.
- Mantén los nombres de las pruebas descriptivos y centrados en el comportamiento.
- Si una entidad o servicio tiene lógica de negocio, las pruebas deben medir el resultado útil del mismo.
- Especifica los casos esperados y los errores o resultados reales en la aserción.
- No introduzcas pruebas artificiales sin relación con el requisito.

## Qué se considera una prueba adecuada

Una prueba es adecuada si:

- valida un resultado observable
- falla cuando cambia la regla de negocio
- cubre un escenario real del usuario o del dominio
- no depende de que un mock reciba una llamada concreta sin comprobar el resultado final

## Qué se considera una prueba débil

Se consideran pruebas débiles o innecesarias:

- “se llamó al método X” sin comprobar el resultado
- asserts sobre mocks en lugar del comportamiento real
- pruebas que repiten el mismo caso sin aportar valor
- tests de estructura que no tienen un requisito funcional asociado

## Criterio de validación

Antes de aceptar una prueba, comprueba que:

- cubre un comportamiento real del sistema
- tiene un caso de éxito y un caso de error o límite
- no es solo una prueba de implementación
- la suite puede ejecutarse con `dotnet test`
- el proyecto compila sin errores

## Resultado esperado

Debe existir una prueba útil y verificable para cada cambio funcional relevante, usando xUnit como framework y validando comportamiento real, no mocks por sí solos.

## Instrucción operativa final

Cuando se añada o modifique una funcionalidad:

1. identificar el comportamiento real que debe protegerse
2. crear pruebas para caso normal y caso de error o límite
3. priorizar pruebas de servicios y validación de negocio
4. evitar asserts solo sobre mocks
5. ejecutar la validación con `dotnet test`
6. confirmar que la prueba protege el requisito y no la implementación
