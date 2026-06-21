namespace ControlAsistencia.Core
{
    public class ProcesadorAsistencia
    {
        public List<RegistroAsistencia> ProcesarArchivo (string rutaArchivo, List<Inconsistencia> Inconsistencias)
        {
            var lector = new LectorExcel();
            var calculador = new CalculadorAsistencia();

            List<MarcacionesDia> dias = lector.Leer(rutaArchivo);
            var registros = new List<RegistroAsistencia>();

            foreach (MarcacionesDia dia in dias)
            {
                RegistroAsistencia registro = calculador.Procesar(dia, Inconsistencias);
                registros.Add(registro);
            }

            return registros;
        }
    }
}
