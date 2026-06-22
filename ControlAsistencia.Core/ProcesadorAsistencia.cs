namespace ControlAsistencia.Core
{
    /// <summary>
    /// Coordina el flujo completo de lectura y calculo de asistencia.
    /// </summary>
    public class ProcesadorAsistencia
    {
        /// <summary>
        /// Lee un archivo de marcaciones y genera los registros consolidados por empleado y fecha.
        /// </summary>
        /// <param name="rutaArchivo">Ruta del archivo Excel con las marcaciones originales.</param>
        /// <param name="inconsistencias">Lista donde se acumulan las novedades detectadas.</param>
        /// <returns>Lista de registros de asistencia procesados.</returns>
        public List<RegistroAsistencia> ProcesarArchivo(string rutaArchivo, List<Inconsistencia> inconsistencias)
        {
            var lector = new LectorExcel();
            var calculador = new CalculadorAsistencia();

            List<MarcacionesDia> dias = lector.Leer(rutaArchivo);
            var registros = new List<RegistroAsistencia>();

            // Cada grupo diario se procesa de forma independiente para conservar
            // las novedades asociadas a su empleado y fecha.
            foreach (MarcacionesDia dia in dias)
            {
                RegistroAsistencia registro = calculador.Procesar(dia, inconsistencias);
                registros.Add(registro);
            }

            return registros;
        }
    }
}
