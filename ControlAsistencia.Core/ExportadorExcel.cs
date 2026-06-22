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
            ICellStyle estiloEncabezado = CrearEstiloEncabezado(libro);
            ICellStyle estiloTexto = CrearEstiloTexto(libro);
            ICellStyle estiloNumero = CrearEstiloNumero(libro);

            // Encabezados solicitados en el requerimiento principal.
            IRow encabezado = hoja.CreateRow(0);
            encabezado.CreateCell(0).SetCellValue("Empleado");
            encabezado.CreateCell(1).SetCellValue("Fecha");
            encabezado.CreateCell(2).SetCellValue("Entrada");
            encabezado.CreateCell(3).SetCellValue("Inicio Almuerzo");
            encabezado.CreateCell(4).SetCellValue("Fin Almuerzo");
            encabezado.CreateCell(5).SetCellValue("Salida");
            encabezado.CreateCell(6).SetCellValue("Horas Trabajadas");
            AplicarEstiloFila(encabezado, estiloEncabezado);

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

                ICell celdaHoras = fila.CreateCell(6);
                if (registro.HorasTrabajadas.HasValue)
                {
                    celdaHoras.SetCellValue(registro.HorasTrabajadas.Value);
                    celdaHoras.CellStyle = estiloNumero;
                }
                else
                {
                    celdaHoras.SetCellValue("");
                    celdaHoras.CellStyle = estiloNumero;
                }

                AplicarEstiloFila(fila, estiloTexto, exceptoColumna: 6);
            }

            DarFormatoHoja(hoja, 18, 14, 14, 18, 16, 14, 18);

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
            ICellStyle estiloEncabezado = CrearEstiloEncabezado(libro);
            ICellStyle estiloTexto = CrearEstiloTexto(libro);

            // Encabezados del reporte adicional de inconsistencias.
            IRow encabezado = hoja.CreateRow(0);
            encabezado.CreateCell(0).SetCellValue("Empleado");
            encabezado.CreateCell(1).SetCellValue("Fecha");
            encabezado.CreateCell(2).SetCellValue("Tipo");
            encabezado.CreateCell(3).SetCellValue("Detalle");
            AplicarEstiloFila(encabezado, estiloEncabezado);

            for (int i = 0; i < inconsistencias.Count; i++)
            {
                Inconsistencia inconsistencia = inconsistencias[i];
                IRow fila = hoja.CreateRow(i + 1);

                fila.CreateCell(0).SetCellValue(inconsistencia.Empleado);
                fila.CreateCell(1).SetCellValue(inconsistencia.Fecha.ToString("yyyy-MM-dd"));
                fila.CreateCell(2).SetCellValue(inconsistencia.Tipo.ToString());
                fila.CreateCell(3).SetCellValue(inconsistencia.Detalle);
                AplicarEstiloFila(fila, estiloTexto);
            }

            DarFormatoHoja(hoja, 18, 14, 28, 70);

            using var stream = File.Create(rutaSalida);
            libro.Write(stream);
        }

        /// <summary>
        /// Crea el estilo visual de los encabezados de los reportes.
        /// </summary>
        /// <param name="libro">Libro Excel donde se aplicara el estilo.</param>
        /// <returns>Estilo de encabezado con color suave y texto en negrita.</returns>
        private ICellStyle CrearEstiloEncabezado(IWorkbook libro)
        {
            IFont fuente = libro.CreateFont();
            fuente.IsBold = true;
            fuente.Color = IndexedColors.White.Index;

            ICellStyle estilo = libro.CreateCellStyle();
            estilo.SetFont(fuente);
            estilo.FillForegroundColor = IndexedColors.DarkTeal.Index;
            estilo.FillPattern = FillPattern.SolidForeground;
            estilo.Alignment = HorizontalAlignment.Center;
            estilo.VerticalAlignment = VerticalAlignment.Center;
            AplicarBordes(estilo);

            return estilo;
        }

        /// <summary>
        /// Crea el estilo base para celdas de texto.
        /// </summary>
        /// <param name="libro">Libro Excel donde se aplicara el estilo.</param>
        /// <returns>Estilo con bordes y alineacion vertical centrada.</returns>
        private ICellStyle CrearEstiloTexto(IWorkbook libro)
        {
            ICellStyle estilo = libro.CreateCellStyle();
            estilo.VerticalAlignment = VerticalAlignment.Center;
            AplicarBordes(estilo);

            return estilo;
        }

        /// <summary>
        /// Crea el estilo para valores numericos de horas trabajadas.
        /// </summary>
        /// <param name="libro">Libro Excel donde se aplicara el estilo.</param>
        /// <returns>Estilo numerico con dos decimales.</returns>
        private ICellStyle CrearEstiloNumero(IWorkbook libro)
        {
            ICellStyle estilo = CrearEstiloTexto(libro);
            estilo.Alignment = HorizontalAlignment.Right;
            estilo.DataFormat = libro.CreateDataFormat().GetFormat("0.00");

            return estilo;
        }

        /// <summary>
        /// Aplica bordes finos a una celda para mejorar la lectura del reporte.
        /// </summary>
        /// <param name="estilo">Estilo al que se agregaran los bordes.</param>
        private void AplicarBordes(ICellStyle estilo)
        {
            estilo.BorderTop = BorderStyle.Thin;
            estilo.BorderRight = BorderStyle.Thin;
            estilo.BorderBottom = BorderStyle.Thin;
            estilo.BorderLeft = BorderStyle.Thin;
            estilo.TopBorderColor = IndexedColors.Grey40Percent.Index;
            estilo.RightBorderColor = IndexedColors.Grey40Percent.Index;
            estilo.BottomBorderColor = IndexedColors.Grey40Percent.Index;
            estilo.LeftBorderColor = IndexedColors.Grey40Percent.Index;
        }

        /// <summary>
        /// Aplica un estilo a todas las celdas creadas en una fila.
        /// </summary>
        /// <param name="fila">Fila que recibira el estilo.</param>
        /// <param name="estilo">Estilo que se aplicara a las celdas.</param>
        /// <param name="exceptoColumna">Columna opcional que conserva su propio estilo.</param>
        private void AplicarEstiloFila(IRow fila, ICellStyle estilo, int? exceptoColumna = null)
        {
            for (int i = 0; i < fila.LastCellNum; i++)
            {
                if (exceptoColumna.HasValue && i == exceptoColumna.Value)
                {
                    continue;
                }

                fila.GetCell(i).CellStyle = estilo;
            }
        }

        /// <summary>
        /// Ajusta columnas y congela el encabezado para que el archivo sea mas legible.
        /// </summary>
        /// <param name="hoja">Hoja Excel que se va a ajustar.</param>
        /// <param name="anchosColumnas">Anchos de columnas medidos aproximadamente en caracteres.</param>
        private void DarFormatoHoja(ISheet hoja, params int[] anchosColumnas)
        {
            hoja.CreateFreezePane(0, 1);

            for (int i = 0; i < anchosColumnas.Length; i++)
            {
                hoja.SetColumnWidth(i, anchosColumnas[i] * 256);
            }
        }
    }
}
