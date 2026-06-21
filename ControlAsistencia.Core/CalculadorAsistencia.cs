namespace ControlAsistencia.Core
{
    public class CalculadorAsistencia
    {

        public RegistroAsistencia Procesar (MarcacionesDia dia, List<Inconsistencia> inconsistencia)
        {
            var marcasFiltradas = EliminarDuplicados(dia.Marcas);

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
                registro.HorasTrabajadas = CalcularHoras(marcasFiltradas[0], marcasFiltradas[1], marcasFiltradas[2], marcasFiltradas[3]);
            }
            else if (marcasFiltradas.Count == 2 || marcasFiltradas.Count == 3)
            {
                registro.Entrada = marcasFiltradas[0];
                registro.Salida = marcasFiltradas[^1];
                registro.HorasTrabajadas = (registro.Salida.Value - registro.Entrada.Value).TotalHours;

                inconsistencia.Add(new Inconsistencia
                {
                    Empleado = dia.Empleado,
                    Fecha = dia.Fecha,
                    Tipo = TipoInconsistencia.MarcacionesIncompletas,
                    Detalle = $"Se encontraron {marcasFiltradas.Count} marcaciones. No se pudo identificar el almuerzo. Horas calculadas sin descontar almuerzo."
                });
            }
            else if (marcasFiltradas.Count == 1)
            {
                inconsistencia.Add(new Inconsistencia
                {
                    Empleado = dia.Empleado,
                    Fecha = dia.Fecha,
                    Tipo = TipoInconsistencia.MarcacionesIncompletas,
                    Detalle = $"Se encontro 1 marcacion {marcasFiltradas[0]}. No se puede calcular las horas trabajadas."
                });
            }
            else
            {
                inconsistencia.Add(new Inconsistencia
                {
                    Empleado = dia.Empleado,
                    Fecha = dia.Fecha,
                    Tipo = TipoInconsistencia.CantidadDiferenteALaEsperada,
                    Detalle = $"Se encontraron {marcasFiltradas.Count} marcaciones, se esperaban 4."
                });
            }

            return registro;
        }

        private List<TimeOnly> EliminarDuplicados (List<TimeOnly> marcas)
        {
            var resultado = new List<TimeOnly> { marcas[0] };

            for (int i = 1; i < marcas.Count; i++)
            {
                TimeSpan diferencia = marcas[i] - resultado[^1];

                if (diferencia.TotalSeconds >= 120)
                {
                    resultado.Add(marcas[i]);
                }
            }

            return resultado;
        }

        private double CalcularHoras(TimeOnly entrada, TimeOnly inicioAlmuerzo, TimeOnly finAlmuerzo, TimeOnly salida)
        {
            TimeSpan totalJornada = salida - entrada;
            TimeSpan totalAlmuerzo = finAlmuerzo - inicioAlmuerzo;
            TimeSpan horasTrabajadas = totalJornada - totalAlmuerzo;

            return horasTrabajadas.TotalHours;
        }
    }
}
