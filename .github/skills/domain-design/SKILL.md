---
name: domain-design
description: |
  Alias obsoleto del skill de modelo. La guía de dominio se mantiene en `modelo`.
  Se conserva por compatibilidad, pero no debe duplicar la documentación principal.
license: MIT
metadata:
  version: "1.0.0"
---

# Skill obsoleto: domain-design

Este skill queda deprecado y se mantiene solo por compatibilidad.
La guía principal del dominio del proyecto está en el skill `modelo`.

## Qué usar en su lugar

Usa [`modelo`](../modelo/SKILL.md) para:

- crear o actualizar el modelo de dominio
- revisar entidades y propiedades del negocio
- mantener `Models/` alineado con `README.md` y `docs/analisis-diseño.md`
- decidir si una funcionalidad sigue siendo de dominio o requiere expandirse a otras capas

## Regla de no duplicación

No vuelvas a repetir aquí la misma guía del dominio que ya está en `modelo`.
La duplicación entre skills genera conflicto de alcance y confunde la responsabilidad de cada capa.

Si una funcionalidad necesita expandir el alcance a servicios, Data, controllers o frontend, debe decidirlo el orquestador del proyecto y dejar la validación asociada en el skill de pruebas.
