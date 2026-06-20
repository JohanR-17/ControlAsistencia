namespace ControlAsistencia.Core
{
    public enum TipoInconsistencia
    {
         MarcacionesIncompletas,
         CantidadDiferenteALaEsperada,
         FaltaIngreso,
         FaltaSalida,
         FaltaRetornoAlimentacion,
         MarcasDuplicadasDescartadas
    }

    public class Inconsistencia
    {
        public string Empleado { get; set; } = "";
        public DateOnly Fecha {  get; set; }
        public TipoInconsistencia Tipo { get; set; }
        public string Detalle { get; set; } = "";
    }
}
