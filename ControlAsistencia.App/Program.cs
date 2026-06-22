namespace ControlAsistencia.App
{
    /// <summary>
    /// Punto de entrada de la aplicacion Windows Forms.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Configura la aplicacion e inicia el formulario principal.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}
