// Genera docs/manual-usuario.docx a partir del contenido de docs/manual-usuario.md.
// Las capturas se insertan desde docs/manual/img/*.png si existen (generadas con Playwright);
// si una captura todavía no se ha generado, se deja un aviso de texto en su lugar.
const fs = require('fs')
const path = require('path')
const {
  Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell,
  ImageRun, HeadingLevel, AlignmentType, BorderStyle, WidthType,
  ShadingType, LevelFormat, TableOfContents, PageBreak, PageNumber,
  Header, Footer,
} = require('docx')

const CARPETA_CAPTURAS = path.join(__dirname, 'manual', 'img')
const ANCHO_PAGINA_CONTENIDO = 9360 // US Letter, márgenes de 1"

function imagenOAviso(nombreFichero, descripcion) {
  const rutaImagen = path.join(CARPETA_CAPTURAS, nombreFichero)
  if (fs.existsSync(rutaImagen)) {
    return new Paragraph({
      alignment: AlignmentType.CENTER,
      spacing: { before: 120, after: 240 },
      children: [
        new ImageRun({
          type: 'png',
          data: fs.readFileSync(rutaImagen),
          transformation: { width: 600, height: 375 },
          altText: { title: descripcion, description: descripcion, name: nombreFichero },
        }),
      ],
    })
  }
  return new Paragraph({
    spacing: { before: 120, after: 240 },
    shading: { fill: 'FFF3CD', type: ShadingType.CLEAR },
    children: [
      new TextRun({
        italics: true,
        text: `[Captura pendiente: ${nombreFichero} — ${descripcion}. Genera las capturas con el script de docs/manual/manual-screenshots.spec.ts]`,
      }),
    ],
  })
}

function h1(texto) {
  return new Paragraph({ heading: HeadingLevel.HEADING_1, children: [new TextRun(texto)] })
}
function h2(texto) {
  return new Paragraph({ heading: HeadingLevel.HEADING_2, children: [new TextRun(texto)] })
}
function h3(texto) {
  return new Paragraph({ heading: HeadingLevel.HEADING_3, children: [new TextRun(texto)] })
}
function p(texto, opciones = {}) {
  return new Paragraph({ spacing: { after: 160 }, children: [new TextRun(texto)], ...opciones })
}
function nota(texto) {
  return new Paragraph({
    spacing: { before: 80, after: 160 },
    shading: { fill: 'E7F1FF', type: ShadingType.CLEAR },
    children: [new TextRun({ text: `Nota: ${texto}`, italics: true })],
  })
}
function pasos(items) {
  return items.map(
    (texto, indice) =>
      new Paragraph({
        numbering: { reference: 'pasos', level: 0 },
        spacing: { after: 80 },
        children: [new TextRun(texto)],
      }),
  )
}

const border = { style: BorderStyle.SINGLE, size: 1, color: 'CCCCCC' }
const borders = { top: border, bottom: border, left: border, right: border }

function tablaDosColumnas(cabeceras, filas, anchos = [3120, 6240]) {
  const filaCabecera = new TableRow({
    tableHeader: true,
    children: cabeceras.map(
      (texto, i) =>
        new TableCell({
          borders,
          width: { size: anchos[i], type: WidthType.DXA },
          shading: { fill: 'D5E8F0', type: ShadingType.CLEAR },
          margins: { top: 80, bottom: 80, left: 120, right: 120 },
          children: [new Paragraph({ children: [new TextRun({ text: texto, bold: true })] })],
        }),
    ),
  })

  const filasDatos = filas.map(
    (fila) =>
      new TableRow({
        children: fila.map(
          (texto, i) =>
            new TableCell({
              borders,
              width: { size: anchos[i], type: WidthType.DXA },
              margins: { top: 80, bottom: 80, left: 120, right: 120 },
              children: [new Paragraph({ children: [new TextRun(texto)] })],
            }),
        ),
      }),
  )

  return new Table({
    width: { size: anchos.reduce((a, b) => a + b, 0), type: WidthType.DXA },
    columnWidths: anchos,
    rows: [filaCabecera, ...filasDatos],
  })
}

const doc = new Document({
  styles: {
    default: { document: { run: { font: 'Arial', size: 22 } } },
    paragraphStyles: [
      { id: 'Heading1', name: 'Heading 1', basedOn: 'Normal', next: 'Normal', quickFormat: true,
        run: { size: 32, bold: true, font: 'Arial', color: '1F4E79' },
        paragraph: { spacing: { before: 360, after: 240 }, outlineLevel: 0 } },
      { id: 'Heading2', name: 'Heading 2', basedOn: 'Normal', next: 'Normal', quickFormat: true,
        run: { size: 26, bold: true, font: 'Arial', color: '2E75B6' },
        paragraph: { spacing: { before: 280, after: 160 }, outlineLevel: 1 } },
      { id: 'Heading3', name: 'Heading 3', basedOn: 'Normal', next: 'Normal', quickFormat: true,
        run: { size: 24, bold: true, font: 'Arial' },
        paragraph: { spacing: { before: 200, after: 120 }, outlineLevel: 2 } },
    ],
  },
  numbering: {
    config: [
      { reference: 'pasos',
        levels: [{ level: 0, format: LevelFormat.DECIMAL, text: '%1.', alignment: AlignmentType.LEFT,
          style: { paragraph: { indent: { left: 720, hanging: 360 } } } }] },
    ],
  },
  sections: [{
    properties: {
      page: {
        size: { width: 12240, height: 15840 },
        margin: { top: 1440, right: 1440, bottom: 1440, left: 1440 },
      },
    },
    headers: {
      default: new Header({ children: [new Paragraph({ children: [new TextRun('App Todo List — Manual de Usuario')] })] }),
    },
    footers: {
      default: new Footer({
        children: [new Paragraph({
          alignment: AlignmentType.CENTER,
          children: [new TextRun('Página '), new TextRun({ children: [PageNumber.CURRENT] })],
        })],
      }),
    },
    children: [
      // Portada
      new Paragraph({ spacing: { before: 2400 }, alignment: AlignmentType.CENTER,
        children: [new TextRun({ text: 'Manual de Usuario', bold: true, size: 56, color: '1F4E79' })] }),
      new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 240 },
        children: [new TextRun({ text: 'App Todo List', size: 36 })] }),
      new Paragraph({ alignment: AlignmentType.CENTER, spacing: { before: 480 },
        children: [new TextRun({ text: 'Versión 1.0', size: 24 })] }),
      new Paragraph({ alignment: AlignmentType.CENTER,
        children: [new TextRun({ text: 'Generado el 2026-09-16', size: 24 })] }),
      new Paragraph({ children: [new PageBreak()] }),

      new TableOfContents('Índice', { hyperlink: true, headingStyleRange: '1-3' }),
      new Paragraph({ children: [new PageBreak()] }),

      // 1. Introducción
      h1('1. Introducción'),
      p('App Todo List es una aplicación web para organizar tus tareas del día a día. Te permite crear tareas sueltas o repetitivas, agruparlas por categorías con colores, y guardar plantillas para no escribir siempre lo mismo.'),
      h2('¿Para quién es?'),
      p('Para cualquier persona que quiera llevar un control sencillo de sus pendientes, sin necesidad de crear una cuenta ni instalar nada: se usa directamente desde el navegador.'),
      h2('¿Qué puedes hacer con ella?'),
      ...pasos([
        'Crear, editar, completar y eliminar tareas.',
        'Marcar tareas como repetitivas (diarias, semanales o mensuales) para que, al completarlas, se genere automáticamente la siguiente.',
        'Clasificar tareas por categorías con un color identificativo.',
        'Guardar plantillas de tareas frecuentes y crear una tarea nueva a partir de ellas con un clic.',
      ]),

      // 2. Primeros pasos
      h1('2. Primeros pasos'),
      h2('Acceder a la aplicación'),
      p('Abre la aplicación en tu navegador con la dirección que te haya facilitado tu equipo técnico. Verás la pantalla principal con la pestaña Tareas activa.'),
      imagenOAviso('01-pantalla-tareas.png', 'Pantalla principal con la pestaña Tareas activa'),
      h2('Navegación básica'),
      p('En la parte superior encontrarás tres pestañas:'),
      tablaDosColumnas(['Pestaña', 'Para qué sirve'], [
        ['Tareas', 'Crear y gestionar tus tareas del día a día'],
        ['Plantillas', 'Guardar modelos de tareas que repites a menudo'],
        ['Categorías', 'Crear las etiquetas de color para clasificar tareas'],
      ]),
      p('Haz clic en cualquiera de ellas para cambiar de sección.'),

      // 3. Gestión de Tareas
      h1('3. Gestión de Tareas'),
      h2('Crear una tarea'),
      ...pasos([
        'Ve a la pestaña Tareas.',
        'Escribe el título en el campo de texto.',
        'Si se repite periódicamente, marca la casilla "Es repetitiva" y elige la frecuencia: Diaria, Semanal o Mensual.',
        'Si quieres clasificarla, elige una categoría en el desplegable (o déjala en "Sin categoría").',
        'Haz clic en Guardar.',
      ]),
      imagenOAviso('02-formulario-nueva-tarea.png', 'Formulario de creación de una tarea repetitiva con categoría'),
      p('La tarea aparece al instante en el listado, con una etiqueta de color si tiene categoría y otra etiqueta indicando la recurrencia si es repetitiva.'),
      imagenOAviso('03-lista-tareas-con-etiquetas.png', 'Listado de tareas con etiquetas de categoría y recurrencia'),
      h2('Completar una tarea'),
      p('Haz clic en el botón Completar de la tarea. Pasa a mostrarse con la etiqueta Completada y desaparece el botón de completar.'),
      nota('si la tarea era repetitiva, al completarla la aplicación crea automáticamente la siguiente ocurrencia, para que no tengas que volver a escribirla.'),
      imagenOAviso('04-tarea-completada.png', 'Tarea completada y nueva ocurrencia generada automáticamente'),
      h2('Editar una tarea'),
      ...pasos([
        'Haz clic en Editar sobre la tarea que quieras modificar.',
        'Cambia el título, la recurrencia o la categoría según necesites.',
        'Haz clic en Guardar para confirmar, o en Cancelar para descartar los cambios.',
      ]),
      imagenOAviso('05-editar-tarea.png', 'Formulario de edición abierto sobre una tarea existente'),
      h2('Eliminar una tarea'),
      p('Haz clic en Eliminar sobre la tarea. Se borra inmediatamente del listado; esta acción no se puede deshacer.'),

      // 4. Gestión de Plantillas
      h1('4. Gestión de Plantillas'),
      p('Las plantillas te permiten guardar el "molde" de una tarea que repites a menudo y crear una tarea nueva a partir de ella con un solo clic.'),
      imagenOAviso('06-pantalla-plantillas.png', 'Pestaña de Plantillas con el formulario de creación'),
      h2('Crear una plantilla'),
      ...pasos([
        'Ve a la pestaña Plantillas.',
        'Escribe el título que tendrán las tareas generadas.',
        'Si las tareas generadas deben ser repetitivas, marca "Genera tareas repetitivas" y elige la frecuencia.',
        'Haz clic en Guardar.',
      ]),
      imagenOAviso('07-formulario-nueva-plantilla.png', 'Formulario de nueva plantilla repetitiva antes de guardar'),
      imagenOAviso('08-lista-plantillas.png', 'Listado de plantillas guardadas'),
      h2('Crear una tarea desde una plantilla'),
      p('Haz clic en Crear tarea sobre la plantilla que quieras usar. La aplicación genera automáticamente una tarea nueva con los valores de la plantilla y muestra un mensaje de confirmación.'),
      imagenOAviso('09-plantilla-instanciar.png', 'Mensaje de confirmación tras crear una tarea desde una plantilla'),
      h2('Editar o eliminar una plantilla'),
      p('Usa los botones Editar o Eliminar de la plantilla, igual que en las tareas.'),

      // 5. Gestión de Categorías
      h1('5. Gestión de Categorías'),
      p('Las categorías te permiten clasificar visualmente tus tareas asignándoles un nombre y un color.'),
      imagenOAviso('10-pantalla-categorias.png', 'Pestaña de Categorías con el listado existente'),
      h2('Crear una categoría'),
      ...pasos([
        'Ve a la pestaña Categorías.',
        'Escribe el nombre de la categoría.',
        'Elige un color con el selector de color.',
        'Haz clic en Guardar.',
      ]),
      imagenOAviso('11-formulario-categoria.png', 'Formulario de nueva categoría con nombre y color seleccionados'),
      h2('Editar una categoría'),
      ...pasos([
        'Haz clic en Editar sobre la categoría.',
        'El formulario se rellena con sus datos actuales.',
        'Cambia el nombre o el color y haz clic en Actualizar, o en Cancelar para descartar los cambios.',
      ]),
      imagenOAviso('12-editar-categoria.png', 'Formulario de edición de una categoría existente'),
      h2('Eliminar una categoría'),
      p('Haz clic en Eliminar sobre la categoría. La aplicación pedirá confirmación antes de borrarla, ya que las tareas que la usaban dejarán de tener esa clasificación.'),

      // 6. FAQ
      h1('6. Preguntas frecuentes'),
      h3('¿Qué pasa si completo una tarea repetitiva por error?'),
      p('La tarea original queda marcada como completada y se genera una nueva ocurrencia; si fue un error, elimina la ocurrencia nueva manualmente.'),
      h3('¿Puedo tener una tarea sin categoría?'),
      p('Sí. La categoría es opcional; puedes dejarla en "Sin categoría" al crear o editar una tarea.'),
      h3('¿Qué diferencia hay entre una tarea repetitiva y una plantilla?'),
      p('Una tarea repetitiva genera automáticamente su siguiente ocurrencia al completarse. Una plantilla es un modelo reutilizable que tú decides cuándo convertir en una tarea nueva.'),
      h3('Si elimino una plantilla, ¿se eliminan las tareas que creé con ella?'),
      p('No. Las tareas ya creadas son independientes; eliminar la plantilla no afecta a las tareas existentes.'),

      // 7. Solución de problemas
      h1('7. Solución de problemas'),
      tablaDosColumnas(['Problema', 'Posible causa / solución'], [
        ['No se cargan las tareas, plantillas o categorías', 'Comprueba tu conexión y recarga la página. Si persiste, contacta con soporte técnico.'],
        ['Aparece un mensaje de error al guardar', 'Revisa que el título no esté vacío y que, si marcaste "Es repetitiva", hayas seleccionado una frecuencia.'],
        ['No veo la categoría recién creada en el formulario de tareas', 'Vuelve a la pestaña Tareas; la lista de categorías se actualiza al entrar en la pantalla.'],
        ['Eliminé una tarea o categoría por error', 'La eliminación es inmediata y no se puede deshacer desde la aplicación.'],
      ]),
    ],
  }],
})

Packer.toBuffer(doc).then((buffer) => {
  const destino = path.join(__dirname, 'manual-usuario.docx')
  fs.writeFileSync(destino, buffer)
  console.log(`Documento generado: ${destino}`)
})
