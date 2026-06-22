using NPOI.SS.UserModel;

namespace ControlAsistencia.Core
{
    /// <summary>
    /// Lee el archivo Excel de entrada y transforma cada fila en marcaciones diarias.
    /// </summary>
    public class LectorExcel
    {
        /// <summary>
        /// Extrae las marcaciones desde la primera hoja del archivo Excel.
        /// </summary>
        /// <param name="rutaArchivo">Ruta del archivo Excel que contiene las marcaciones.</param>
        /// <returns>Lista de marcaciones agrupadas por empleado y fecha.</returns>
        public List<MarcacionesDia> Leer(string rutaArchivo)
        {
            var resultado = new List<MarcacionesDia>();

            using var stream = File.OpenRead(rutaArchivo);
            IWorkbook libro = WorkbookFactory.Create(stream);
            ISheet hoja = libro.GetSheetAt(0);

            // Se inicia en la fila 1 porque la fila 0 corresponde al encabezado.
            for (int i = 1; i <= hoja.LastRowNum; i++)
            {
                IRow fila = hoja.GetRow(i);
                if (fila == null) continue;

                var dia = new MarcacionesDia
                {
                    Empleado = fila.GetCell(0).ToString(),
                    Fecha = DateOnly.Parse(fila.GetCell(1).ToString())
                };

                // La columna 5 contiene las horas separadas por punto y coma.
                string textoHoras = fila.GetCell(5).ToString();
                foreach (string textoHora in textoHoras.Split(';'))
                {
                    if (TimeOnly.TryParse(textoHora.Trim(), out TimeOnly hora))
                    {
                        dia.Marcas.Add(hora);
                    }
                }

                // Ordenar las marcas garantiza que el calculo use la secuencia correcta.
                dia.Marcas.Sort();
                resultado.Add(dia);
            }

            return resultado;
        }
    }
}
    

