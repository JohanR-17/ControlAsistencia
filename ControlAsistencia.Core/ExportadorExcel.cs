using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace ControlAsistencia.Core
{
    public class ExportadorExcel
    {
        public void ExportarResumen (List<RegistroAsistencia> registros, string rutaSalida)
        {
            IWorkbook libro = new XSSFWorkbook();
            ISheet hoja = libro.CreateSheet("Resumen");

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

        public void ExportarNovedades (List<Inconsistencia> inconsistencia, string rutaSalida)
        {
            IWorkbook libro = new XSSFWorkbook();
            ISheet hoja = libro.CreateSheet("Novedades");

            IRow encabezado = hoja.CreateRow(0);
            encabezado.CreateCell(0).SetCellValue("Empleado");
            encabezado.CreateCell(1).SetCellValue("Fecha");
            encabezado.CreateCell(2).SetCellValue("Tipo");
            encabezado.CreateCell(3).SetCellValue("Detalle");

            for (int i = 0; i < inconsistencia.Count; i++)
            {
                Inconsistencia inconsistencias = inconsistencia[i];
                IRow fila = hoja.CreateRow(i + 1);

                fila.CreateCell(0).SetCellValue(inconsistencias.Empleado);
                fila.CreateCell(1).SetCellValue(inconsistencias.Fecha.ToString("yyyy-MM-dd"));
                fila.CreateCell(2).SetCellValue(inconsistencias.Tipo.ToString());
                fila.CreateCell(3).SetCellValue(inconsistencias.Detalle);
            }

            using var stream = File.Create(rutaSalida);
            libro.Write(stream);
        }
}
}
