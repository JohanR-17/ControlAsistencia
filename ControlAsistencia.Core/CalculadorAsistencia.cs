namespace ControlAsistencia.Core;

/// <summary>
/// Contiene la logica de negocio para convertir las marcaciones de un dia
/// en un registro de asistencia consolidado.
/// </summary>
public class CalculadorAsistencia
{
    /// <summary>
    /// Procesa las marcaciones de un empleado en una fecha especifica y registra
    /// las novedades encontradas durante el calculo.
    /// </summary>
    /// <param name="dia">Marcaciones agrupadas por empleado y fecha.</param>
    /// <param name="inconsistencias">Lista donde se agregan las novedades detectadas.</param>
    /// <returns>Registro de asistencia calculado para el dia indicado.</returns>
    public RegistroAsistencia Procesar(MarcacionesDia dia, List<Inconsistencia> inconsistencias)
    {
        var marcasFiltradas = EliminarDuplicados(dia.Marcas, dia.Empleado, dia.Fecha, inconsistencias);

        var registro = new RegistroAsistencia
        {
            Empleado = dia.Empleado,
            Fecha = dia.Fecha
        };

        if (marcasFiltradas.Count == 4)
        {
            // Caso ideal: las cuatro marcaciones permiten identificar entrada,
            // salida y regreso de almuerzo, y salida final.
            registro.Entrada = marcasFiltradas[0];
            registro.InicioAlmuerzo = marcasFiltradas[1];
            registro.FinAlmuerzo = marcasFiltradas[2];
            registro.Salida = marcasFiltradas[3];
            registro.HorasTrabajadas = CalcularHoras(
                marcasFiltradas[0], marcasFiltradas[1], marcasFiltradas[2], marcasFiltradas[3]);
        }
        else if (marcasFiltradas.Count == 2 || marcasFiltradas.Count == 3)
        {
            // Si faltan marcas intermedias, se calcula con primera y ultima marca,
            // dejando la novedad para revision porque no se puede descontar almuerzo.
            registro.Entrada = marcasFiltradas[0];
            registro.Salida = marcasFiltradas[^1];
            registro.HorasTrabajadas = (registro.Salida.Value - registro.Entrada.Value).TotalHours;

            inconsistencias.Add(new Inconsistencia
            {
                Empleado = dia.Empleado,
                Fecha = dia.Fecha,
                Tipo = TipoInconsistencia.MarcacionesIncompletas,
                Detalle = $"Se encontraron {marcasFiltradas.Count} marcaciones; no se pudo identificar el almuerzo. Horas calculadas sin descontar almuerzo."
            });
        }
        else if (marcasFiltradas.Count == 1)
        {
            // Una sola marcacion no permite determinar una jornada trabajada.
            inconsistencias.Add(new Inconsistencia
            {
                Empleado = dia.Empleado,
                Fecha = dia.Fecha,
                Tipo = TipoInconsistencia.MarcacionesIncompletas,
                Detalle = $"Solo se encontró 1 marcación ({marcasFiltradas[0]}). No es posible calcular horas trabajadas."
            });
        }
        else
        {
            // Cualquier otra cantidad de marcas queda reportada como fuera de lo esperado.
            inconsistencias.Add(new Inconsistencia
            {
                Empleado = dia.Empleado,
                Fecha = dia.Fecha,
                Tipo = TipoInconsistencia.CantidadDiferenteALaEsperada,
                Detalle = $"Se encontraron {marcasFiltradas.Count} marcaciones, se esperaban 4."
            });
        }

        return registro;
    }

    /// <summary>
    /// Descarta marcaciones repetidas o demasiado cercanas entre si, usando un margen
    /// minimo de dos minutos entre una marca valida y la siguiente.
    /// </summary>
    /// <param name="marcas">Lista de horas ordenadas del dia.</param>
    /// <param name="empleado">Identificador o nombre del empleado.</param>
    /// <param name="fecha">Fecha a la que pertenecen las marcaciones.</param>
    /// <param name="inconsistencias">Lista donde se reportan las marcas descartadas.</param>
    /// <returns>Lista de marcaciones sin duplicados cercanos.</returns>
    private List<TimeOnly> EliminarDuplicados(List<TimeOnly> marcas, string empleado, DateOnly fecha, List<Inconsistencia> inconsistencias)
    {
        var resultado = new List<TimeOnly> { marcas[0] };

        for (int i = 1; i < marcas.Count; i++)
        {
            TimeSpan diferencia = marcas[i] - resultado[^1];

            if (diferencia.TotalSeconds >= 120)
            {
                resultado.Add(marcas[i]);
            }
            else
            {
                // Se considera duplicado cuando el empleado marca de nuevo casi de inmediato.
                inconsistencias.Add(new Inconsistencia
                {
                    Empleado = empleado,
                    Fecha = fecha,
                    Tipo = TipoInconsistencia.MarcasDuplicadasDescartadas,
                    Detalle = $"Se descartó la marca {marcas[i]} por estar a {diferencia.TotalSeconds:0} segundos de la marca anterior."
                });
            }
        }

        return resultado;
    }

    /// <summary>
    /// Calcula las horas efectivas trabajadas descontando el tiempo de alimentacion.
    /// </summary>
    /// <param name="entrada">Hora de ingreso a la empresa.</param>
    /// <param name="inicioAlmuerzo">Hora de salida a alimentacion.</param>
    /// <param name="finAlmuerzo">Hora de regreso de alimentacion.</param>
    /// <param name="salida">Hora de salida final de la empresa.</param>
    /// <returns>Total de horas trabajadas en formato decimal.</returns>
    private double CalcularHoras(TimeOnly entrada, TimeOnly inicioAlmuerzo, TimeOnly finAlmuerzo, TimeOnly salida)
    {
        TimeSpan totalJornada = salida - entrada;
        TimeSpan tiempoAlmuerzo = finAlmuerzo - inicioAlmuerzo;
        TimeSpan horasEfectivas = totalJornada - tiempoAlmuerzo;

        return horasEfectivas.TotalHours;
    }
}
