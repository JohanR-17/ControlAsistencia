namespace ControlAsistencia.Core;

public class CalculadorAsistencia
{
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
            registro.Entrada = marcasFiltradas[0];
            registro.InicioAlmuerzo = marcasFiltradas[1];
            registro.FinAlmuerzo = marcasFiltradas[2];
            registro.Salida = marcasFiltradas[3];
            registro.HorasTrabajadas = CalcularHoras(
                marcasFiltradas[0], marcasFiltradas[1], marcasFiltradas[2], marcasFiltradas[3]);
        }
        else if (marcasFiltradas.Count == 2 || marcasFiltradas.Count == 3)
        {
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

    private double CalcularHoras(TimeOnly entrada, TimeOnly inicioAlmuerzo, TimeOnly finAlmuerzo, TimeOnly salida)
    {
        TimeSpan totalJornada = salida - entrada;
        TimeSpan tiempoAlmuerzo = finAlmuerzo - inicioAlmuerzo;
        TimeSpan horasEfectivas = totalJornada - tiempoAlmuerzo;

        return horasEfectivas.TotalHours;
    }
}