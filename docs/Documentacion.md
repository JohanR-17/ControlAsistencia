# Documentación Técnica - Sistema de Control de Asistencia

## 1. Enfoque utilizado

La solución se desarrolló en C# (.NET 8), dividida en tres proyectos dentro
de una sola solución: lógica de negocio, interfaz gráfica, y pruebas
unitarias. Para leer y escribir archivos Excel se usó la librería NPOI,
elegida específicamente porque el archivo de entrada llega en formato
.xls (binario, formato antiguo de Excel) — librerías más comunes como
ClosedXML o EPPlus solo soportan el formato .xlsx moderno.

La estrategia de procesamiento es: por cada empleado/día, se toman todas
sus marcaciones, se ordenan cronológicamente, y se descartan marcas que
estén a menos de 2 minutos de diferencia entre sí (indicativo de una
doble marcación en el lector biométrico, no un evento real distinto).
Si tras esto quedan exactamente 4 marcas, se clasifican posicionalmente
(1ra = Entrada, 2da = Salida Almuerzo, 3ra = Regreso Almuerzo, 4ta =
Salida Final) y se calculan las horas trabajadas descontando el tiempo
de almuerzo. Los casos que no calzan en este patrón se reportan como
inconsistencias en un archivo separado, en lugar de asumir datos que no
se pueden confirmar con certeza.

La interfaz gráfica (Windows Forms) se diseñó alrededor de un único botón
que guía al usuario paso a paso: seleccionar el archivo de entrada,
procesar, y elegir dónde guardar cada uno de los dos archivos de salida —
todo mediante los diálogos estándar de Windows, sin rutas fijas en el
código, para que la solución funcione en cualquier computador.

## 2. Estructura de la solución

- **ControlAsistencia.Core** (Class Library): toda la lógica de negocio,
  sin dependencias de interfaz.
  - `RegistroAsistencia`, `Inconsistencia` / `TipoInconsistencia`,
    `MarcacionesDia`: modelos de datos.
  - `LectorExcel`: lee el archivo de entrada con NPOI y lo convierte en
    una lista de `MarcacionesDia`.
  - `CalculadorAsistencia`: contiene la lógica de negocio — elimina
    duplicados, clasifica las marcas, calcula horas, y genera las
    inconsistencias correspondientes.
  - `ProcesadorAsistencia`: coordina `LectorExcel` y
    `CalculadorAsistencia` para procesar el archivo completo.
  - `ExportadorExcel`: genera los dos archivos de salida (resumen y
    novedades) con NPOI.
- **ControlAsistencia.App** (Windows Forms): interfaz gráfica con un
  único formulario y botón, que orquesta todo el flujo y maneja errores
  con `try/catch` para evitar mostrar excepciones sin control al usuario.
- **ControlAsistencia.Tests** (xUnit): pruebas automatizadas que cubren
  las 4 rutas posibles de la lógica de clasificación (caso normal,
  duplicados resueltos, marcaciones incompletas, y cantidad inesperada
  de marcas).

## 3. Supuestos realizados

- Con exactamente 4 marcas (tras eliminar duplicados), se asume el orden
  cronológico: Entrada, Salida Almuerzo, Regreso Almuerzo, Salida Final.
- Se considera "duplicado" cualquier par de marcas consecutivas separadas
  por menos de 2 minutos. Este umbral se eligió tras analizar el archivo
  de ejemplo real: los duplicados genuinos estaban separados por
  segundos, mientras que patrones de doble descanso (6 marcas) tenían
  separaciones de varios minutos — por lo que el umbral nunca fusiona
  eventos que en realidad son distintos.
- Con 2 o 3 marcas, se calculan las horas trabajadas usando la primera y
  la última marca, sin descuento de almuerzo, y se reporta como
  inconsistencia para revisión manual.
- Con 1 sola marca, o más de 4 que no se resuelven al eliminar
  duplicados, no se calculan horas trabajadas — se reporta únicamente
  como inconsistencia, para no presentar un número basado en una
  suposición poco confiable.