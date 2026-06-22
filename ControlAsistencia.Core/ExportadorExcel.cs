using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ControlAsistencia.Core
{
    /// <summary>
    /// Genera archivos Excel de salida con los resultados del procesamiento.
    /// </summary>
    public class ExportadorExcel
    {
        /// <summary>
        /// Crea el archivo de resumen con los horarios consolidados y las horas trabajadas.
        /// </summary>
        /// <param name="registros">Registros de asistencia calculados.</param>
        /// <param name="rutaSalida">Ruta donde se guardara el archivo Excel generado.</param>
        public void ExportarResumen(List<RegistroAsistencia> registros, string rutaSalida)
        {
            IWorkbook libro = new XSSFWorkbook();
            ISheet hoja = libro.CreateSheet("Resumen");

            // Encabezados solicitados en el requerimiento principal.
            IRow encabezado = hoja.CreateRow(0);
            encabezado.CreateCell(0).SetCellValue("Empleado");
            encabezado.CreateCell(1).SetCellValue("Fecha");
            encabezado.CreateCell(2).SetCellValue("Entrada");
            encabezado.CreateCell(3).SetCellValue("Inicio Almuerzo");
            encabezado.CreateCell(4).SetCellValue("Fin Almuerzo");
            encabezado.CreateCell(5).SetCellValue("Salida");
            encabezado.CreateCell(6).SetCellValue("Horas Trabajadas");

            for (int i = 0; i < registros.Count; i++)
            {
                RegistroAsistencia registro = registros[i];
                IRow fila = hoja.CreateRow(i + 1);

                // Los valores nulos se exportan como vacio para evidenciar datos faltantes.
                fila.CreateCell(0).SetCellValue(registro.Empleado);
                fila.CreateCell(1).SetCellValue(registro.Fecha.ToString("yyyy-MM-dd"));
                fila.CreateCell(2).SetCellValue(registro.Entrada?.ToString("HH:mm:ss") ?? "");
                fila.CreateCell(3).SetCellValue(registro.InicioAlmuerzo?.ToString("HH:mm:ss") ?? "");
                fila.CreateCell(4).SetCellValue(registro.FinAlmuerzo?.ToString("HH:mm:ss") ?? "");
                fila.CreateCell(5).SetCellValue(registro.Salida?.ToString("HH:mm:ss") ?? "");
                fila.CreateCell(6).SetCellValue(registro.HorasTrabajadas?.ToString("0.00") ?? "");
            }

            using var stream = File.Create(rutaSalida);
            libro.Write(stream);
        }

        /// <summary>
        /// Crea el archivo de novedades con las inconsistencias encontradas.
        /// </summary>
        /// <param name="inconsistencias">Novedades detectadas durante el procesamiento.</param>
        /// <param name="rutaSalida">Ruta donde se guardara el archivo Excel generado.</param>
        public void ExportarNovedades(List<Inconsistencia> inconsistencias, string rutaSalida)
        {
            IWorkbook libro = new XSSFWorkbook();
            ISheet hoja = libro.CreateSheet("Novedades");

            // Encabezados del reporte adicional de inconsistencias.
            IRow encabezado = hoja.CreateRow(0);
            encabezado.CreateCell(0).SetCellValue("Empleado");
            encabezado.CreateCell(1).SetCellValue("Fecha");
            encabezado.CreateCell(2).SetCellValue("Tipo");
            encabezado.CreateCell(3).SetCellValue("Detalle");

            for (int i = 0; i < inconsistencias.Count; i++)
            {
                Inconsistencia inconsistencia = inconsistencias[i];
                IRow fila = hoja.CreateRow(i + 1);

                fila.CreateCell(0).SetCellValue(inconsistencia.Empleado);
                fila.CreateCell(1).SetCellValue(inconsistencia.Fecha.ToString("yyyy-MM-dd"));
                fila.CreateCell(2).SetCellValue(inconsistencia.Tipo.ToString());
                fila.CreateCell(3).SetCellValue(inconsistencia.Detalle);
            }

            using var stream = File.Create(rutaSalida);
            libro.Write(stream);
        }
    }
}
