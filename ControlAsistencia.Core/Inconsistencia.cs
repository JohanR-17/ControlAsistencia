namespace ControlAsistencia.Core
{
    /// <summary>
    /// Clasifica los tipos de novedades que pueden aparecer al procesar marcaciones.
    /// </summary>
    public enum TipoInconsistencia
    {
        /// <summary>
        /// El dia no tiene todas las marcas necesarias para identificar la jornada completa.
        /// </summary>
        MarcacionesIncompletas,

        /// <summary>
        /// La cantidad de marcaciones no coincide con las cuatro esperadas.
        /// </summary>
        CantidadDiferenteALaEsperada,

        /// <summary>
        /// No se encontro la marcacion de ingreso.
        /// </summary>
        FaltaIngreso,

        /// <summary>
        /// No se encontro la marcacion de salida final.
        /// </summary>
        FaltaSalida,

        /// <summary>
        /// No se encontro la marcacion de regreso de alimentacion.
        /// </summary>
        FaltaRetornoAlimentacion,

        /// <summary>
        /// Se encontraron marcaciones repetidas o muy cercanas y fueron descartadas.
        /// </summary>
        MarcasDuplicadasDescartadas
    }

    /// <summary>
    /// Describe una novedad o inconsistencia encontrada durante el procesamiento.
    /// </summary>
    public class Inconsistencia
    {
        /// <summary>
        /// Nombre, codigo o identificador del empleado asociado a la novedad.
        /// </summary>
        public string Empleado { get; set; } = "";

        /// <summary>
        /// Fecha en la que se detecto la novedad.
        /// </summary>
        public DateOnly Fecha { get; set; }

        /// <summary>
        /// Tipo de inconsistencia detectada.
        /// </summary>
        public TipoInconsistencia Tipo { get; set; }

        /// <summary>
        /// Descripcion legible de la novedad para facilitar su revision.
        /// </summary>
        public string Detalle { get; set; } = "";
    }
}
