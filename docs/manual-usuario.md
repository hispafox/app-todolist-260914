# Manual de Usuario — App Todo List

**Versión:** 1.0
**Generado el:** 2026-09-16

---

## 1. Introducción

**App Todo List** es una aplicación web para organizar tus tareas del día a día. Te permite crear tareas sueltas o repetitivas, agruparlas por categorías con colores, y guardar plantillas para no escribir siempre lo mismo.

### ¿Para quién es?

Para cualquier persona que quiera llevar un control sencillo de sus pendientes, sin necesidad de crear una cuenta ni instalar nada: se usa directamente desde el navegador.

### ¿Qué puedes hacer con ella?

- Crear, editar, completar y eliminar tareas.
- Marcar tareas como **repetitivas** (diarias, semanales o mensuales) para que, al completarlas, se genere automáticamente la siguiente.
- Clasificar tareas por **categorías** con un color identificativo.
- Guardar **plantillas** de tareas frecuentes y crear una tarea nueva a partir de ellas con un clic.

---

## 2. Primeros pasos

### Acceder a la aplicación

Abre la aplicación en tu navegador con la dirección que te haya facilitado tu equipo técnico. Verás la pantalla principal con la pestaña **Tareas** activa.

![Pantalla principal de la aplicación con la pestaña Tareas activa](manual/img/01-pantalla-tareas.png)

### Navegación básica

En la parte superior encontrarás tres pestañas:

| Pestaña | Para qué sirve |
|---|---|
| **Tareas** | Crear y gestionar tus tareas del día a día |
| **Plantillas** | Guardar modelos de tareas que repites a menudo |
| **Categorías** | Crear las etiquetas de color para clasificar tareas |

Haz clic en cualquiera de ellas para cambiar de sección. La aplicación recuerda los datos ya cargados mientras la tengas abierta.

---

## 3. Gestión de Tareas

### Crear una tarea

1. Ve a la pestaña **Tareas**.
2. Escribe el título en el campo de texto.
3. Si se repite periódicamente, marca la casilla **Es repetitiva** y elige la frecuencia: **Diaria**, **Semanal** o **Mensual**.
4. Si quieres clasificarla, elige una **categoría** en el desplegable (o déjala en "Sin categoría").
5. Haz clic en **Guardar**.

![Formulario de creación de una tarea repetitiva con categoría](manual/img/02-formulario-nueva-tarea.png)

La tarea aparece al instante en el listado, con una etiqueta de color si tiene categoría y otra etiqueta indicando la recurrencia si es repetitiva.

![Listado de tareas mostrando las etiquetas de categoría y recurrencia](manual/img/03-lista-tareas-con-etiquetas.png)

### Completar una tarea

Haz clic en el botón **Completar** de la tarea. Pasa a mostrarse con la etiqueta **Completada** y desaparece el botón de completar.

> **Tareas repetitivas:** si la tarea era repetitiva, al completarla la aplicación crea automáticamente la siguiente ocurrencia (por ejemplo, la tarea de la semana que viene), para que no tengas que volver a escribirla.

![Tarea marcada como completada y nueva ocurrencia generada automáticamente](manual/img/04-tarea-completada.png)

### Editar una tarea

1. Haz clic en **Editar** sobre la tarea que quieras modificar.
2. Cambia el título, la recurrencia o la categoría según necesites.
3. Haz clic en **Guardar** para confirmar, o en **Cancelar** para descartar los cambios.

![Formulario de edición abierto sobre una tarea existente](manual/img/05-editar-tarea.png)

### Eliminar una tarea

Haz clic en **Eliminar** sobre la tarea. Se borra inmediatamente del listado; esta acción no se puede deshacer.

---

## 4. Gestión de Plantillas

Las plantillas te permiten guardar el "molde" de una tarea que repites a menudo (por ejemplo, "Reunión de equipo semanal") y crear una tarea nueva a partir de ella con un solo clic, sin rellenar el formulario cada vez.

### Pantalla de Plantillas

![Pestaña de Plantillas con el formulario de creación](manual/img/06-pantalla-plantillas.png)

### Crear una plantilla

1. Ve a la pestaña **Plantillas**.
2. Escribe el título que tendrán las tareas generadas.
3. Si las tareas generadas deben ser repetitivas, marca **Genera tareas repetitivas** y elige la frecuencia.
4. Haz clic en **Guardar**.

![Formulario de nueva plantilla repetitiva antes de guardar](manual/img/07-formulario-nueva-plantilla.png)

La plantilla aparece en el listado, lista para usarse.

![Listado de plantillas guardadas](manual/img/08-lista-plantillas.png)

### Crear una tarea desde una plantilla

Haz clic en **Crear tarea** sobre la plantilla que quieras usar. La aplicación genera automáticamente una tarea nueva con los valores de la plantilla y te muestra un mensaje de confirmación.

![Mensaje de confirmación tras crear una tarea a partir de una plantilla](manual/img/09-plantilla-instanciar.png)

Consulta la pestaña **Tareas** para ver la tarea recién creada.

### Editar o eliminar una plantilla

Usa los botones **Editar** o **Eliminar** de la plantilla, igual que en las tareas.

---

## 5. Gestión de Categorías

Las categorías te permiten clasificar visualmente tus tareas asignándoles un nombre y un color.

### Pantalla de Categorías

![Pestaña de Categorías con el listado existente](manual/img/10-pantalla-categorias.png)

### Crear una categoría

1. Ve a la pestaña **Categorías**.
2. Escribe el **nombre** de la categoría.
3. Elige un **color** con el selector de color.
4. Haz clic en **Guardar**.

![Formulario de nueva categoría con nombre y color seleccionados](manual/img/11-formulario-categoria.png)

### Editar una categoría

1. Haz clic en **Editar** sobre la categoría.
2. El formulario se rellena con sus datos actuales.
3. Cambia el nombre o el color y haz clic en **Actualizar**, o en **Cancelar** para descartar los cambios.

![Formulario de edición de una categoría existente](manual/img/12-editar-categoria.png)

### Eliminar una categoría

Haz clic en **Eliminar** sobre la categoría. La aplicación te pedirá confirmación antes de borrarla, ya que las tareas que la usaban dejarán de tener esa clasificación.

---

## 6. Preguntas frecuentes

**¿Qué pasa si completo una tarea repetitiva por error?**
La aplicación no permite deshacer la acción desde la interfaz. La tarea original queda marcada como completada y se genera una nueva ocurrencia; si fue un error, elimina la ocurrencia nueva manualmente.

**¿Puedo tener una tarea sin categoría?**
Sí. La categoría es opcional; puedes dejarla en "Sin categoría" al crear o editar una tarea.

**¿Qué diferencia hay entre una tarea repetitiva y una plantilla?**
Una tarea **repetitiva** genera automáticamente su siguiente ocurrencia al completarse. Una **plantilla** es un modelo reutilizable que tú decides cuándo convertir en una tarea nueva, haciendo clic en "Crear tarea".

**Si elimino una plantilla, ¿se eliminan las tareas que creé con ella?**
No. Las tareas ya creadas son independientes; eliminar la plantilla no afecta a las tareas existentes.

---

## 7. Solución de problemas

| Problema | Posible causa / solución |
|---|---|
| No se cargan las tareas, plantillas o categorías | Comprueba tu conexión y recarga la página. Si el problema persiste, contacta con soporte técnico. |
| Aparece un mensaje de error al guardar | Revisa que el título no esté vacío y que, si marcaste "Es repetitiva", hayas seleccionado una frecuencia. |
| No veo la categoría que acabo de crear en el formulario de tareas | Vuelve a la pestaña Tareas; la lista de categorías se actualiza al entrar en la pantalla. |
| Eliminé una tarea o categoría por error | La eliminación es inmediata y no se puede deshacer desde la aplicación. Vuelve a crearla manualmente. |
