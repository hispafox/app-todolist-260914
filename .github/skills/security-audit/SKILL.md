---
name: security-audit
description: |
  Audita los skills locales del repositorio para detectar riesgos de seguridad,
  secretos, ejecución remota, scripts peligrosos y cambios fuera del alcance
  esperado.
license: MIT
metadata:
  version: "1.0.0"
---

# Auditoría de seguridad de skills locales

Usa esta habilidad cuando quieras revisar si un skill instalado en este repositorio es seguro antes de confiarlo.

## Alcance

Esta revisión se aplica a los skills que viven en:

- .github/skills/
- cualquier skill local o personalizado del entorno del usuario
- scripts o hooks asociados a esos skills

## Objetivo

Comprobar que un skill no:

- expone secretos o tokens
- ejecuta comandos peligrosos
- descarga código desde internet sin control
- escribe archivos fuera del alcance esperado
- abre conexiones externas no justificadas
- ejecuta código arbitrario desde entrada no confiable

## Checklist de revisión

### 1. Revisa la metadata del skill

Comprueba que el archivo `SKILL.md` tenga:

- nombre claro y descriptivo
- descripción limitada al propósito real
- licencia explícita si procede
- sin instrucciones ambiguas o muy generales

### 2. Busca indicadores de riesgo

Revisa cualquier archivo del skill en busca de patrones como estos:

- `curl ... | bash`
- `wget ... | sh`
- `eval(`
- `exec(`
- `child_process.exec`
- `require('child_process')`
- `subprocess.run(...)`
- `os.system(`
- `rm -rf`
- `chmod 777`
- `sudo`
- `git clone ...`
- `curl -X POST`
- `fetch('https://...')`
- `http://` o `https://` a dominios no esperados
- `process.env` con nombres de secretos

### 3. Busca secretos o credenciales

Busca tokens, claves o credenciales en los archivos del skill:

- `API_KEY`
- `TOKEN`
- `SECRET`
- `BEGIN PRIVATE KEY`
- `aws_access_key_id`
- `gh auth`
- `ssh` con rutas o claves privadas
- `password=`

Si hay valores sensibles en texto plano, el skill no es seguro para usarse sin revisión.

### 4. Comprueba la ejecución del código

Un skill es sospechoso si:

- crea archivos fuera del repo
- modifica el sistema operativo o el PATH
- instala paquetes sin contexto claro
- hace cambios globales no documentados
- ejecuta scripts ad hoc sin justificación

### 5. Evalúa la superficie de red

Observa si el skill hace llamadas a dominios externos sin necesidad evidente. Si hay conexiones a: 

- GitHub o registries de terceros
- endpoints de API no documentados
- localhost con comandos de instalación o limpieza

debemos entender por qué se hace y si se justifica para la tarea.

## Regla de oro

Un skill es aceptable si cumple estas condiciones:

- solo hace lo que promete en la descripción
- no contiene secretos ni rutas sensibles
- no ejecuta código remoto sin avisar al usuario
- no toca archivos o sistema fuera del alcance del proyecto
- no usa shell arbitraria ni comandos destructivos

## Resultado esperado

La auditoría debe producir una conclusión clara:

- seguro
- necesita revisión
- no recomendable

## Formato de salida recomendado

Usa este formato al responder:

1. Skill revisado: nombre
2. Ruta: .github/skills/...
3. Resultado: seguro / revisión / no recomendable
4. Riesgos encontrados: lista breve
5. Recomendación: aceptar, bloquear o corregir

## Recomendación final

Si un skill requiere acceso privado a repositorios, credenciales de GitHub o instalación global sin control, no debe aceptarse sin revisión explícita. En este caso, es preferible mantenerlo local y dejarlo documentado en el proyecto.
