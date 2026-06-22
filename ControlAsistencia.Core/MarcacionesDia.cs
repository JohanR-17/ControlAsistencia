namespace ControlAsistencia.Core
{
    /// <summary>
    /// Agrupa todas las marcaciones realizadas por un empleado en una misma fecha.
    /// </summary>
    public class MarcacionesDia
    {
        /// <summary>
        /// Nombre, codigo o identificador del empleado.
        /// </summary>
        public string Empleado { get; set; } = "";

        /// <summary>
        /// Fecha a la que pertenecen las marcaciones.
        /// </summary>
        public DateOnly Fecha { get; set; }

        /// <summary>
        /// Lista ordenada de horas marcadas durante el dia.
        /// </summary>
        public List<TimeOnly> Marcas { get; set; } = new();
    }
}
