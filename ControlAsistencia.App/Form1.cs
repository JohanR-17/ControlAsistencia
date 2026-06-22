using ControlAsistencia.Core;

namespace ControlAsistencia.App
{
    /// <summary>
    /// Ventana principal de la aplicacion de escritorio para procesar archivos de asistencia.
    /// </summary>
    public partial class Form1 : Form
    {
        private List<RegistroAsistencia> registrosProcesados = new();
        private List<Inconsistencia> inconsistenciasProcesadas = new();

        /// <summary>
        /// Inicializa los componentes visuales del formulario.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Selecciona el archivo Excel de entrada y procesa las marcaciones.
        /// </summary>
        /// <param name="sender">Control que dispara el evento.</param>
        /// <param name="e">Argumentos del evento de clic.</param>
        private void btnLeerArchivo_Click(object sender, EventArgs e)
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
                // Procesa el archivo seleccionado y acumula las novedades encontradas.
                inconsistenciasProcesadas = new List<Inconsistencia>();
                var procesador = new ProcesadorAsistencia();
                registrosProcesados = procesador.ProcesarArchivo(rutaEntrada, inconsistenciasProcesadas);

                lblArchivo.Text = $"Archivo cargado: {Path.GetFileName(rutaEntrada)}";
                lblEstado.Text = $"{registrosProcesados.Count} registros procesados. {inconsistenciasProcesadas.Count} novedades encontradas.";
                btnGuardarResultados.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al leer el archivo:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Exporta el resumen y el reporte de novedades generados previamente.
        /// </summary>
        /// <param name="sender">Control que dispara el evento.</param>
        /// <param name="e">Argumentos del evento de clic.</param>
        private void btnGuardarResultados_Click(object sender, EventArgs e)
        {
            if (registrosProcesados.Count == 0)
            {
                MessageBox.Show("Primero debes leer un archivo de marcaciones.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // Solicita al usuario la ubicacion del archivo de resumen.
                SaveFileDialog dialogoGuardarResumen = new SaveFileDialog();
                dialogoGuardarResumen.Filter = "Archivo de Excel (*.xlsx)|*.xlsx";
                dialogoGuardarResumen.Title = "Guardar resumen de asistencia";
                dialogoGuardarResumen.FileName = "Resumen.xlsx";

                if (dialogoGuardarResumen.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                // Solicita al usuario la ubicacion del archivo de novedades.
                SaveFileDialog dialogoGuardarNovedades = new SaveFileDialog();
                dialogoGuardarNovedades.Filter = "Archivo de Excel (*.xlsx)|*.xlsx";
                dialogoGuardarNovedades.Title = "Guardar reporte de novedades";
                dialogoGuardarNovedades.FileName = "Novedades.xlsx";

                if (dialogoGuardarNovedades.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                // Genera los archivos finales en formato Excel.
                var exportador = new ExportadorExcel();
                exportador.ExportarResumen(registrosProcesados, dialogoGuardarResumen.FileName);
                exportador.ExportarNovedades(inconsistenciasProcesadas, dialogoGuardarNovedades.FileName);

                MessageBox.Show($"Listo! Se guardaron {registrosProcesados.Count} registros y {inconsistenciasProcesadas.Count} novedades.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrio un error al guardar los archivos:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
