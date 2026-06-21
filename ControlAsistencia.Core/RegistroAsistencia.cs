namespace ControlAsistencia.Core
{
    public class RegistroAsistencia
    {
        public string Empleado { get; set; } = "";
        public DateOnly Fecha { get; set; }
        public TimeOnly? Hora { get; set; }
        public TimeOnly? Entrada { get; set; }
        public TimeOnly? InicioAlmuerzo { get; set; }
        public TimeOnly? FinAlmuerzo { get; set; }
        public TimeOnly? Salida { get; set; }
        public double? HorasTrabajadas {  get; set; }
    }
}
