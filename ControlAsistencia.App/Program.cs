using ControlAsistencia.Core;

namespace ControlAsistencia.App
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // --- PRUEBA TEMPORAL ---
            var inconsistencias = new List<Inconsistencia>();
            var procesador = new ProcesadorAsistencia();
            var registros = procesador.ProcesarArchivo(
                @"C:\Users\usuario\Documents\Sistema de Registros\ControlAsistencia\data\Data.xls",
                inconsistencias);
            var exportador = new ExportadorExcel();
            exportador.ExportarResumen(
                registros,
                @"C:\Users\usuario\Desktop\Pruebas\Resumen.xlsx");

            MessageBox.Show($"¡Listo! {registros.Count} registros procesados, {inconsistencias.Count} inconsistencias encontradas.");
            // --- FIN PRUEBA TEMPORAL ---
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}