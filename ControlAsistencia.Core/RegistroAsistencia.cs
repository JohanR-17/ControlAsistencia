namespace ControlAsistencia.Core
{
    /// <summary>
    /// Representa el resultado consolidado de asistencia de un empleado para una fecha.
    /// </summary>
    public class RegistroAsistencia
    {
        /// <summary>
        /// Nombre, codigo o identificador del empleado.
        /// </summary>
        public string Empleado { get; set; } = "";

        /// <summary>
        /// Fecha del registro de asistencia.
        /// </summary>
        public DateOnly Fecha { get; set; }

        /// <summary>
        /// Hora individual de marcacion. Se mantiene disponible para casos donde
        /// se necesite representar una marca sin consolidar.
        /// </summary>
        public TimeOnly? Hora { get; set; }

        /// <summary>
        /// Primera marcacion del dia, correspondiente al ingreso a la empresa.
        /// </summary>
        public TimeOnly? Entrada { get; set; }

        /// <summary>
        /// Marcacion de salida al periodo de alimentacion.
        /// </summary>
        public TimeOnly? InicioAlmuerzo { get; set; }

        /// <summary>
        /// Marcacion de regreso del periodo de alimentacion.
        /// </summary>
        public TimeOnly? FinAlmuerzo { get; set; }

        /// <summary>
        /// Ultima marcacion del dia, correspondiente a la salida final.
        /// </summary>
        public TimeOnly? Salida { get; set; }

        /// <summary>
        /// Total de horas trabajadas, descontando alimentacion cuando es posible.
        /// </summary>
        public double? HorasTrabajadas { get; set; }
    }
}
