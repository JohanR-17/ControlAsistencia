using ControlAsistencia.Core;

namespace ControlAsistencia.App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogoAbrir = new OpenFileDialog();
            dialogoAbrir.Filter = "Archivos de Excel (*.xls;*.xlsx)|*.xls;*.xlsx";
            dialogoAbrir.Title = "Selecciona el archivo de marcaciones";

            if (dialogoAbrir.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string rutaEntrada = dialogoAbrir.FileName;

            try
            {
                var inconsistencias = new List<Inconsistencia>();
                var procesador = new ProcesadorAsistencia();
                var registros = procesador.ProcesarArchivo(rutaEntrada, inconsistencias);

                SaveFileDialog dialogoGuardarResumen = new SaveFileDialog();
                dialogoGuardarResumen.Filter = "Archivo de Excel (*.xlsx)|*.xlsx";
                dialogoGuardarResumen.Title = "Guardar resumen de asistencia";
                dialogoGuardarResumen.FileName = "Resumen.xlsx";

                if (dialogoGuardarResumen.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                SaveFileDialog dialogoGuardarNovedades = new SaveFileDialog();
                dialogoGuardarNovedades.Filter = "Archivo de Excel (*.xlsx)|*.xlsx";
                dialogoGuardarNovedades.Title = "Guardar reporte de novedades";
                dialogoGuardarNovedades.FileName = "Novedades.xlsx";

                if (dialogoGuardarNovedades.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                var exportador = new ExportadorExcel();
                exportador.ExportarResumen(registros, dialogoGuardarResumen.FileName);
                exportador.ExportarNovedades(inconsistencias, dialogoGuardarNovedades.FileName);

                MessageBox.Show($"¡Listo! {registros.Count} registros procesados, {inconsistencias.Count} inconsistencias encontradas.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al procesar el archivo:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
