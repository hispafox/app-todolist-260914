---
titulo: "Estudio: Agent Skills para gestión de proyectos y competencias relacionadas"
fecha: 2026-09-15
tipo: informe-investigacion
estado: borrador-v1
alcance: "Agent Skills (SKILL.md) de ágil/Scrum/Kanban, issues y seguimiento, product management y comunicación/reporting, cruzadas con los marcos de competencias del jefe de proyecto"
fuentes: "API de GitHub (gh api / gh search code), skills.sh, agentskills.io, scrumguides.org, kanbanguides.org, ipma.world, PDF de PMI"
---

# Agent Skills para gestión de proyectos

> Datos consultados el **15-09-2026**. Estrellas y fechas de último push cambian cada día: sirven para comparar tamaños, no como cifra fija.

## Qué se ha mirado y cómo

Cinco búsquedas en paralelo, una por área (ágil, issues, producto, comunicación) y una quinta sobre los marcos de competencias humanas. Las cuatro de skills buscaron con `gh search code --filename SKILL.md` y recorrieron los árboles de las colecciones grandes. Ninguna clonó ni instaló nada.

Después comprobé yo, contra la API de GitHub, **30 repositorios** (estrellas, último push, licencia) y **24 rutas de `SKILL.md`** elegidas entre las más citadas. Existen las 24. Salió una discrepancia: `oakoss/agent-skills` venía como MIT y en su raíz no hay fichero de licencia.

Al leer las tablas, ten en cuenta dos límites:

- La búsqueda de código de GitHub devuelve como mucho unos 30 resultados por término y cortó varias veces por límite de peticiones. Algunos términos (`RICE`, `gantt`, `meeting notes`, `weekly update`) no llegaron a ejecutarse como búsqueda y se cubrieron recorriendo colecciones. **Puede haber skills buenas que no salen aquí.**
- Los scripts que traen algunas skills se han listado, no auditado. Donde pone «trae scripts», hay que leerlos antes de instalar.

## Lo que sale, en corto

La oferta se concentra en pocas colecciones: dos muy grandes de la comunidad (`phuryn/pm-skills` y `mohitagw15856/pm-claude-skills`), la de GitHub y `ccpm`. Alrededor, OpenAI, Anthropic y Microsoft aportan una o dos skills del tema cada una. Cubre muy bien la parte documental de la gestión: el PRD (el documento de requisitos de producto), las historias de usuario, el registro de riesgos, las actas y el informe interno. También la conexión con las herramientas de seguimiento: hay skills oficiales o muy usadas para GitHub Issues, Linear, Azure DevOps, Notion y YouTrack.

Lo que apenas tiene skill es lo que más pesa en los marcos de competencias: el presupuesto, la gobernanza, la negociación, el liderazgo. Tampoco hay nada serio para métricas de flujo Kanban ni para el informe de estado a un cliente externo.

Cuadra con la encuesta del Project Management Institute (PMI) de 2024, en la sección 7: planificación y seguimiento son lo que más se automatiza con IA generativa (54-55 %). Lo que menos, la gestión de talento y recursos (33 %).

## 1. Las colecciones que concentran el tema

| Colección | ⭐ | Último push | Licencia | Qué aporta |
|---|---|---|---|---|
| [phuryn/pm-skills](https://github.com/phuryn/pm-skills) | 26.325 | 14-09-2026 | MIT | 69 skills de producto en nueve plugins. El bloque `pm-execution` es el de gestión: PRD, roadmap por resultados, sprint, retro, mapa de stakeholders, actas, pre-mortem |
| [mohitagw15856/pm-claude-skills](https://github.com/mohitagw15856/pm-claude-skills) | 1.364 | 14-09-2026 | MIT | Colección generalista de 1.098 skills (su descripción incluye hasta cómo recurrir una prestación). Dentro, un bloque de PM muy completo: PRD, RICE, registro de riesgos, Gantt en Mermaid, sprint, retro. Tiene copia en español en `i18n/es/` |
| [github/awesome-copilot](https://github.com/github/awesome-copilot) | 39.016 | 15-09-2026 | MIT | La de GitHub: issues, publicación de versiones, Azure DevOps, actas, post-mortem, informes de estado (`roundup`) |
| [automazeio/ccpm](https://github.com/automazeio/ccpm) | 8.372 | 18-03-2026 | MIT | Una sola skill, la más seguida del área: de PRD a épica, de épica a issues de GitHub y de ahí a agentes en paralelo |
| [openai/skills](https://github.com/openai/skills) | 27.225 | 08-09-2026 | cada skill trae su `LICENSE.txt` | Curadas por OpenAI: Linear, preparación de reuniones con Notion |
| [anthropics/skills](https://github.com/anthropics/skills) | 176.372 | 10-09-2026 | cada skill trae su `LICENSE.txt` | `internal-comms`: informes de estado, actualizaciones a dirección, FAQs |
| [microsoft/skills](https://github.com/microsoft/skills) | 3.018 | 14-09-2026 | MIT | Poco de gestión: `github-issue-creator` y un generador de changelog |

## 2. Skills por área

### 2.1 Ágil: Scrum, Kanban, historias, estimación

| Skill | Repositorio | Qué hace | ⭐ | Licencia | Ojo con |
|---|---|---|---|---|---|
| `sprint-plan`, `retro`, `user-stories`, `job-stories` | phuryn/pm-skills | Planificación de sprint, retrospectiva, historias y job stories | 26.325 | MIT | — |
| `sprint-planning` | mohitagw15856/pm-claude-skills | Objetivo de sprint, backlog con story points, plan de capacidad | 1.364 | MIT | trae `scripts/` y plantillas |
| `sprint-retro-facilitator` | mohitagw15856/pm-claude-skills | Retro a partir de lo hecho, bloqueado y en curso, revisando las acciones de la anterior | 1.364 | MIT | — |
| `user-story-writer`, `sprint-velocity-analysis` | mohitagw15856/pm-claude-skills | Historias con Given/When/Then y análisis de velocidad | 1.364 | MIT | la de velocidad no se ha leído entera |
| `scrum-sage` | michalparkola/tapestry-skills | Coach de agilidad inspirado en Sutherland y Ohno: análisis de sprint, escalado, impedimentos | 543 | MIT | — |
| `estimate-calibrator` | Mathews-Tom/armory | Estimación PERT de tres puntos con intervalo de confianza | 318 | MIT | no es planning poker |
| `kanban` y familia (`-init`, `-run`, `-refine`…) | cyanluna-git/cyanluna.skills | Tablero Kanban en SQLite local | 178 | **sin licencia** | trae scripts que usan `sqlite3` |
| `retro` | neurofoo/agent-skills | Retro Start/Stop/Continue con tabla de acciones | 114 | MIT | — |
| `kanban` | sig-id/chief-wiggum | Tareas Kanban con cadenas de dependencias | 45 | MIT | — |
| `kanban-ai` | mattjoyce/kanban-skill | Tablero Kanban en ficheros Markdown | 24 | Apache-2.0 | trae `scripts/` |
| `scrum`, `kanban` | TheLobbi/claude | Cobertura genérica de Scrum y de Kanban | 21 | MIT | — |
| `scrum-conductor` | oakoss/agent-skills | Ceremonias Scrum enganchadas a GitHub Issues, Jira y Linear | 15 | **sin licencia** | — |
| `p-daily-standup` | jackchuka/skills | Resume la actividad del día anterior y la publica en Slack | 15 | MIT | llama a servicios externos (gh, Slack) |
| `scrum-master` | CrashBytes/claude-role-skills | Las cuatro ceremonias, con fórmulas de capacidad y velocidad | 7 | Apache-2.0 | — |
| `story-creation`, `story-split`, `epic-creation`, `project-story-mapping` | dariopalminio/agile-sddf | Historias con Gherkin, INVEST, épicas y story mapping. **En español** | 1 | MIT | un solo autor |
| `user-story-builder` | juan-estrada-itti/way-of-work-tools | Historias con INVEST, en español e inglés | 0 | **sin licencia** | un solo autor |

Descartadas por usar la palabra y no el método: `glebis/claude-skills/retrospective` es la retro de una sesión del propio agente, y `alon21034/claude-scrum-skills/sprint` y `richardmbailey/rb-skills/rb-sprint` llaman «sprint» a un ciclo de código.

### 2.2 Issues y seguimiento del trabajo

| Skill | Repositorio | Herramienta | Qué hace | ⭐ | Licencia | Credenciales |
|---|---|---|---|---|---|---|
| `github-issues` | github/awesome-copilot | GitHub Issues y Projects | Crear y editar issues, etiquetas, hitos, sub-issues y Projects | 39.016 | MIT | MCP de GitHub |
| `github-release` | github/awesome-copilot | GitHub Releases | Publicación de versión de principio a fin: número SemVer, changelog, PR | 39.016 | MIT | `gh` |
| `azure-devops-cli` | github/awesome-copilot | Azure DevOps Boards | Work items, boards, sprints y pipelines por `az` | 39.016 | MIT | `az` + extensión devops |
| `issue-fields-migration` | github/awesome-copilot | GitHub | Migra etiquetas y campos de Projects a los Issue Fields de organización | 39.016 | MIT | `gh` |
| `create-github-issue-feature-from-specification` | github/awesome-copilot | GitHub Issues | Convierte una especificación en un issue de funcionalidad nueva | 39.016 | MIT | herramientas del entorno |
| `linear` | openai/skills | Linear | Issues, proyectos y flujo del equipo | 27.225 | `LICENSE.txt` propio | MCP de Linear por OAuth |
| `triage`, `to-tickets` | mattpocock/skills | agnóstica | Triaje de issues con máquina de estados; plan → tickets con dependencias de bloqueo | 262.325 | MIT | la del tracker configurado |
| `github-issue-creator` | microsoft/skills | GitHub Issues | Notas, logs o capturas → issue bien redactado | 3.018 | MIT | ninguna, solo redacta |
| `wiki-changelog` | microsoft/skills | changelog | Changelog agrupado por tipo a partir de `git log` | 3.018 | MIT | ninguna |
| `spec-to-implementation` | makenotion/notion-cookbook | Notion | Especificación → tareas en Notion con criterios de aceptación | 210 | MIT | herramientas de Notion |
| `managing-youtrack` | JetBrains/skills | YouTrack | Issues, comentarios, etiquetas y partes de horas | 348 | **sin licencia** | token en variable de entorno |
| `gh-issue-sync` | mitsuhiko/gh-issue-sync | GitHub Issues | Issues como ficheros Markdown locales | 165 | Apache-2.0 | binario propio |
| `Linear` | wrsmith108/linear-claude-skill | Linear | Issues y proyectos, con GraphQL de respaldo | 125 | MIT | MCP o CLI |
| `Jira` | open-cli-collective/atlassian-cli | Jira | Issues, sprints, boards y jerarquía épica-historia | 30 | MIT | CLI `jtk` + token |
| `planecli` | cpatrickalves/plane-cli | Plane.so | Work items, ciclos, módulos, etiquetas y documentos | 23 | MIT | CLI propia |
| `jira` | pchuri/jira-cli | Jira | Issues y sprints desde terminal | 14 | ISC | CLI npm + token |

Dos matices sobre lo «oficial». La de YouTrack está en el repositorio de JetBrains, pero su propio frontmatter dice que viene de un repositorio de la comunidad (`forketyfork/agentic-skills`, 2 ⭐). Y `makenotion/claude-code-notion-plugin` trae la misma skill de Notion **sin licencia**; la copia del cookbook sí es MIT.

### 2.3 Product management y planificación

| Skill | Repositorio | Qué hace | ⭐ | Licencia | Ojo con |
|---|---|---|---|---|---|
| `ccpm` | automazeio/ccpm | Gestión dirigida por especificación: PRD → épica → issues → agentes, con trazabilidad | 8.372 | MIT | último push en marzo |
| `create-prd`, `outcome-roadmap`, `prioritization-frameworks`, `pre-mortem`, `brainstorm-okrs`, `strategy-red-team` | phuryn/pm-skills | PRD, roadmap por resultados, priorización, pre-mortem, OKR y crítica de la estrategia | 26.325 | MIT | — |
| `prd-template` | mohitagw15856/pm-claude-skills | PRD completo: problema, historias, requisitos | 1.364 | MIT | — |
| `rice-prioritisation`, `feature-prioritisation` | mohitagw15856/pm-claude-skills | RICE, y también MoSCoW, Kano, ICE | 1.364 | MIT | la de RICE trae `scripts/` |
| `risk-register` | mohitagw15856/pm-claude-skills | Registro de riesgos con probabilidad, impacto y semáforo | 1.364 | MIT | — |
| `gantt-roadmap`, `roadmap-narrative`, `okr-builder` | mohitagw15856/pm-claude-skills | Gantt en Mermaid con exportación a `.ics`, roadmap contado para dirección, OKR | 1.364 | MIT | — |
| `pdm` | simota/agent-skills | Compara lo planificado (PRD, roadmap) con lo que ya está en el código; vista WBS | 78 | MIT | — |
| `rice`, `moscow` | neurofoo/agent-skills | Priorización mínima, sin scripts | 114 | MIT | — |
| `pm-prd`, `pm-roadmap`, `pm-breakdown` | nmrtn/nanopm | PRD o pitch Shape Up; roadmap según metodología; PRD → tareas exportadas a Linear o GitHub | 48 | MIT | — |
| `product-manager-skills` | Digidai/product-manager-skills | Conjunto de producto en una sola skill: crítica de PRD, roadmap, discovery | 162 | **sin licencia** | — |
| `pm-workbench` | BobbieLee/pm-workbench | Enmarcar peticiones vagas, priorizar, especificación ligera | 42 | MIT | trae `scripts/` |

Un dato que sorprende: **GitHub Spec Kit** (136.877 ⭐) no usa Agent Skills para su flujo principal. Trabaja con comandos y plantillas propias y solo tiene dos `SKILL.md` que no son de especificación.

### 2.4 Comunicación y reporting

| Skill | Repositorio | Qué hace | ⭐ | Licencia | Ojo con |
|---|---|---|---|---|---|
| `internal-comms` | anthropics/skills | Informes de estado, actualizaciones a dirección, newsletter, FAQs, informes de incidente | 176.372 | `LICENSE.txt` propio | — |
| `meeting-minutes` | github/awesome-copilot | Actas cortas con decisiones y acciones, listas para pasar a issues | 39.016 | MIT | — |
| `roundup`, `roundup-setup` | github/awesome-copilot | Informe de estado bajo demanda cruzando GitHub, correo, Teams y Slack, con tu estilo | 39.016 | MIT | lee de varias fuentes |
| `incident-postmortem` | github/awesome-copilot | Post-mortem sin culpables tras una caída | 39.016 | MIT | — |
| `gtm-board-and-investor-communication` | github/awesome-copilot | Consejo de administración e inversores | 39.016 | MIT | — |
| `summarize-meeting`, `stakeholder-map`, `metrics-dashboard`, `release-notes` | phuryn/pm-skills | Transcripción → acta; mapa poder/interés con plan de comunicación; diseño de cuadro de mando; notas de versión | 26.325 | MIT | el cuadro de mando se define, no se genera |
| `notion-meeting-intelligence` | openai/skills | Agenda y documentación previa de una reunión con contexto de Notion | 27.225 | `LICENSE.txt` propio | — |
| `exec-comms`, `stakeholder-craft` | menkesu/awesome-pm-skills | Memo ejecutivo (6-pager, SCQA) y relación con stakeholders | 404 | **sin licencia** | — |
| `meeting-prep` | glebis/claude-skills | Prepara reuniones con calendario, investigación de asistentes e histórico | 378 | MIT | trae un script JS sin auditar |
| `postmortem` | flonat/flonat-research | Post-mortem genérico, también de errores que no son técnicos | 135 | MIT | — |
| `postmortem` | neurofoo/agent-skills | Post-mortem sin culpables con resumen ejecutivo | 114 | MIT | — |
| `monday-prep` | bwelker/Edwin | Informe semanal, puntos a tratar y riesgos para la reunión de dirección | 17 | Apache-2.0 | depende de rutas propias del autor |

## 3. Los marcos de competencias del jefe de proyecto

| Marco | Versión vigente | Estructura | Confianza |
|---|---|---|---|
| [PMI Talent Triangle](https://www.pmi.org/certifications/certification-resources/maintain/talent-triangle) | 2022 | Ways of Working · Power Skills · Business Acumen | media-alta: pmi.org bloqueó la lectura y se contrastó con cuatro fuentes secundarias |
| [PMBOK Guide](https://www.pmi.org/standards/pmbok) | **8.ª edición**: miembros PMI desde noviembre de 2025, venta pública desde enero de 2026 | 6 principios · 7 dominios (Governance, Scope, Schedule, Finance, Stakeholders, Resources, Risk) · 5 áreas de foco | media: **fuente secundaria**. Los nombres de las áreas de foco están por verificar |
| [IPMA ICB4](https://ipma.world/ipma-standards-development-programme/icb4/) | 2015, sin ICB5 anunciada | People (10 elementos) · Practice (14) · Perspective (5) | alta en las tres áreas; media en el recuento de 29 |
| [Scrum Guide](https://scrumguides.org/scrum-guide.html) | noviembre de 2020 | 3 responsabilidades · 5 eventos · 3 artefactos con su compromiso | alta. No define competencias, solo responsabilidades |
| [Kanban Guide](https://kanbanguides.org/english/) | v2025.5, 1-05-2025 | 3 prácticas · 4 métricas de flujo (WIP, throughput, edad del ítem, cycle time) | alta |

Los marcos de product management (ProdBOK, Pragmatic Framework) se quedan fuera. La lectura de sus webs no dio un contenido fiable, y uno de los resúmenes no coincidía con la estructura conocida del marco.

## 4. El cruce: qué competencia tiene skill que la apoye

La columna de cobertura es **criterio mío** sobre lo que hacen las skills de las tablas anteriores. No sale de ninguna fuente.

| Competencia | Aparece en | Skills que la apoyan | Cobertura |
|---|---|---|---|
| Alcance y requisitos | PMBOK Scope · ICB4 Practice | `create-prd`, `prd-template`, `pm-prd`, `ccpm` | alta |
| Descomposición del trabajo | PMBOK Scope · ICB4 Practice | `to-tickets`, `pm-breakdown`, `ccpm`, `epic-creation` | alta en PRD → tareas; la WBS jerárquica clásica solo aparece como vista dentro de `pdm` |
| Cronograma | PMBOK Schedule · ICB4 Practice | `gantt-roadmap`, `outcome-roadmap`, `pm-roadmap` | parcial: dibujan el plan, no gestionan ruta crítica ni holguras |
| Presupuesto y finanzas | PMBOK Finance · ICB4 Practice | ninguna verificada (`estimate-calibrator` estima esfuerzo, no coste) | **hueco** |
| Gobernanza | PMBOK Governance · ICB4 Perspective | ninguna | **hueco** |
| Recursos y capacidad | PMBOK Resources · ICB4 Practice | `sprint-planning` (plan de capacidad) | parcial |
| Seguimiento del trabajo | PMBOK (seguimiento y control) · Kanban Guide | `github-issues`, `azure-devops-cli`, `linear`, `Jira`, `scrum-conductor`, `pdm` | alta |
| Métricas de flujo y desempeño | Kanban Guide · PMBOK | `sprint-velocity-analysis`, `metrics-dashboard` | parcial: velocidad sí; cycle time y diagrama de flujo acumulado, no |
| Visualizar y limitar el trabajo en curso | Kanban Guide | `kanban-ai`, `kanban` (cyanluna) | parcial: llevan el tablero, no calculan métricas |
| Facilitación y coaching ágil | Scrum Guide (Scrum Master) | `scrum-sage`, `scrum-master`, `sprint-retro-facilitator` | parcial: preparan y aconsejan; la sala la lleva una persona |
| Backlog y valor del producto | Scrum Guide (Product Owner) · PMBOK Focus on Value | `rice`, `moscow`, `prioritization-frameworks`, `user-story-writer` | alta |
| Visión y estrategia | Talent Triangle Business Acumen · ICB4 Perspective | `product-strategy`, `product-vision`, `strategy-red-team` | parcial |
| Stakeholders | PMBOK Stakeholders · ICB4 Practice | `stakeholder-map`, `stakeholder-craft`, `internal-comms`, `exec-comms` | media |
| Comunicación de avance | Talent Triangle Power Skills · ICB4 People | `internal-comms`, `roundup`, `meeting-minutes`, `summarize-meeting`, `monday-prep` | alta hacia dentro; **hueco** hacia un cliente externo |
| Liderazgo, negociación, conflicto | Talent Triangle Power Skills · ICB4 People | `stakeholder-craft` como consejo | baja |
| Riesgos | PMBOK Risk · ICB4 Practice | `risk-register`, `pre-mortem` | media-alta |
| Calidad | PMBOK Embed Quality | `incident-postmortem`, `postmortem` | baja: miran el fallo cuando ya ha pasado |
| Sostenibilidad | PMBOK Integrate Sustainability | ninguna | **hueco** |
| Cumplimiento normativo | ICB4 Perspective | ninguna | **hueco** |

## 5. Los huecos

- **Presupuesto, coste y gobernanza.** Tres de los siete dominios de PMBOK 8 no tienen una sola skill verificada.
- **Informe de estado a cliente externo.** Todo lo que hay mira hacia dentro: equipo, dirección, consejo.
- **Métricas de flujo Kanban** (cycle time, edad del ítem, diagrama de flujo acumulado). Las cuatro métricas que la Kanban Guide pide llevar no tienen una skill que las calcule.
- **Planning poker** y **sprint review** como skills propias. Aparecen como apartado dentro de otras.
- **Definition of Done y Definition of Ready** como artefacto independiente.
- **Agilidad a escala** (SAFe, LeSS, Nexus): nada dedicado.
- **Herramientas sin skill oficial:** Asana y Trello solo tienen skills de comunidad o de granjas de skills; Monday.com y ClickUp no aparecieron. En Azure DevOps Boards no hay skill de Microsoft dedicada a work items; la de `github/awesome-copilot` va por `az` genérico.

## 6. Riesgos y señales de calidad

- **Granjas de skills.** `majiayu000/claude-skill-registry` (609 ⭐) replica unos 21.000 `SKILL.md` sacados de otros repositorios, y `modbender/skill-library-mcp` sigue el mismo patrón. Sirven para descubrir; para instalar, hay que ir al repositorio de origen. La skill `tpm-report` de ese registro se ha dejado fuera por eso.
- **Sin licencia:** `cyanluna-git/cyanluna.skills`, `oakoss/agent-skills`, `JetBrains/skills`, `makenotion/claude-code-notion-plugin`, `menkesu/awesome-pm-skills`, `Digidai/product-manager-skills`, `juan-estrada-itti/way-of-work-tools`. Leerlas vale; copiar su texto en material propio, no.
- **Afirmaciones del autor sin comprobar:** `mohitagw15856/pm-claude-skills` dice estar en el directorio oficial de plugins de Anthropic. No se ha verificado.
- **Skills que salen de tu máquina:** `p-daily-standup` publica en Slack y `roundup` lee correo, Teams y Slack. Ninguna pide tokens escritos en el propio `SKILL.md`: los que los necesitan usan variables de entorno u OAuth.
- **Scripts sin auditar:** `meeting-prep` (JS), `kanban-ai`, `cyanluna.skills`, `sprint-planning`, `rice-prioritisation`, `pm-workbench`.
- **Repositorios pequeños**, con menos de 50 estrellas aunque con actividad en 2026: `agile-sddf`, `way-of-work-tools`, `nanopm`, `pm-workbench`, `plane-cli`. Aquí están las dos opciones en español del área ágil: `agile-sddf` (1 ⭐) y `way-of-work-tools` (0 ⭐).
- **Ninguna instrucción sospechosa** en los `SKILL.md` que se han leído.

## 7. Qué dice PMI sobre IA y gestión de proyectos

| Dato | Fuente | Fecha |
|---|---|---|
| Quienes usan IA generativa en al menos la mitad de sus proyectos pasaron del 20 % al 37 % de la muestra entre dos oleadas | PMI, [*Pushing the Limits: Transforming Project Management With GenAI Innovation*](https://www.pmi.org/-/media/pmi/documents/public/pdf/learning/thought-leadership/genai-pushing-limits-report_final.pdf), n=500 | 19-09-2024 |
| Tareas que más se automatizan: planificación y seguimiento (54-55 %), análisis de datos (37-38 %), comunicación (en torno al 42 %), riesgos (en torno al 36 %), talento y recursos (33 %) | mismo informe | 19-09-2024 |
| La implantación de IA en las organizaciones subió del 47 % (2022) al 57 % (2023); al 77 % le interesa aprender IA | PMI (capítulo de Suecia y otros), [*Artificial Intelligence and Project Management: A Global Chapter-Led Survey*](https://www.pmi.org/-/media/pmi/documents/public/pdf/artificial-intelligence/community-led-ai-and-project-management-report.pdf), n=2.314 | 2024 |
| El *Pulse of the Profession 2026* trata de proyectos complejos, no de IA: el 97 % gestionó al menos uno el último año y el 31 % de esos proyectos no alcanza los beneficios previstos | PMI, [*Driving Success in Complex Projects*](https://www.pmi.org/learning/thought-leadership/driving-success-in-complex-projects) | 2026 |

Las cifras que circulan en blogs («el 79 % de las organizaciones de alto rendimiento usa IA», «ahorra de 3 a 5 horas semanales») no aparecen en ninguna fuente primaria de PMI y se han descartado.

## 8. Por dónde empezar a leer

Si quieres ver cómo se escribe una buena skill de gestión antes de instalar nada, estas cuatro dan la foto casi entera: `phuryn/pm-skills/pm-execution` para producto y ejecución, `github/awesome-copilot` para issues, actas e informes, `automazeio/ccpm` para el flujo de especificación a issues, y `anthropics/skills/internal-comms` para el informe de estado.

Pendiente de verificar si este informe sale del `_inbox/`: la estructura exacta de PMBOK 8 contra el texto de PMI, y repetir las búsquedas que cortó el límite de peticiones.
