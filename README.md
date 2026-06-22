# Sistema de Control de Asistencia

Aplicación de escritorio que procesa los registros de marcación de
entrada y salida de los empleados a partir de un archivo Excel, calcula
las horas trabajadas descontando el tiempo de almuerzo, y genera un
reporte de inconsistencias para los casos que no se ajustan al patrón
esperado de 4 marcaciones diarias.

## Tecnologías

- C# / .NET 8
- NPOI (lectura y escritura de archivos Excel .xls y .xlsx)
- Windows Forms (interfaz gráfica)
- xUnit (pruebas unitarias)
- Git / GitHub (control de versiones)

## Cómo ejecutar

1. Clonar este repositorio.
2. Abrir `ControlAsistencia.sln` con Visual Studio 2022 o superior.
3. Restaurar los paquetes NuGet (se hace automáticamente al compilar).
4. Ejecutar el proyecto `ControlAsistencia.App` (F5).
5. En la ventana de la aplicación, hacer clic en "Procesar Archivo de
   Asistencia", seleccionar el archivo Excel de entrada (puedes usar
   `data/Data.xls`, incluido en este repositorio como ejemplo), y elegir
   dónde guardar el resumen y el reporte de novedades.

## Estructura del proyecto