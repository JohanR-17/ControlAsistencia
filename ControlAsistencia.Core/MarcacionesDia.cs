namespace ControlAsistencia.Core
{
    public class MarcacionesDia
    {
        public string Empleado { get; set; } = "";
        public DateOnly Fecha { get; set; }
        public List<TimeOnly> Marcas { get; set; } = new();
    }
}
