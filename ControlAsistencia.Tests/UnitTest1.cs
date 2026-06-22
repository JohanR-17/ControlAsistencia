using ControlAsistencia.Core;
using Xunit;

namespace ControlAsistencia.Tests;

/// <summary>
/// Pruebas unitarias para validar los escenarios principales del calculo de asistencia.
/// </summary>
public class CalculadorAsistenciaTests
{
    /// <summary>
    /// Verifica el caso ideal de cuatro marcaciones y descuento correcto del almuerzo.
    /// </summary>
    [Fact]
    public void Procesar_ConCuatroMarcasNormales_CalculaNueveHoras()
    {
        // Arrange (preparar los datos de entrada)
        var dia = new MarcacionesDia
        {
            Empleado = "EMP001",
            Fecha = new DateOnly(2026, 6, 8),
            Marcas = new List<TimeOnly>
            {
                new TimeOnly(6, 0, 0),
                new TimeOnly(12, 0, 0),
                new TimeOnly(13, 0, 0),
                new TimeOnly(16, 0, 0)
            }
        };
        var inconsistencias = new List<Inconsistencia>();
        var calculador = new CalculadorAsistencia();

        // Act (ejecutar el codigo que se esta probando)
        RegistroAsistencia resultado = calculador.Procesar(dia, inconsistencias);

        // Assert (verificar que el resultado sea el esperado)
        Assert.Equal(9, resultado.HorasTrabajadas);
        Assert.Empty(inconsistencias);
    }

    /// <summary>
    /// Verifica que una marca repetida en menos de dos minutos se descarte como duplicada.
    /// </summary>
    [Fact]
    public void Procesar_ConMarcaDuplicada_LaDescartaYCalculaNormal()
    {
        var dia = new MarcacionesDia
        {
            Empleado = "EMP002",
            Fecha = new DateOnly(2026, 6, 9),
            Marcas = new List<TimeOnly>
            {
                new TimeOnly(6, 0, 0),
                new TimeOnly(6, 0, 1),   // duplicado, 1 segundo despues
                new TimeOnly(12, 0, 0),
                new TimeOnly(13, 0, 0),
                new TimeOnly(16, 0, 0)
            }
        };
        var inconsistencias = new List<Inconsistencia>();
        var calculador = new CalculadorAsistencia();

        var resultado = calculador.Procesar(dia, inconsistencias);

        Assert.Equal(9, resultado.HorasTrabajadas);
        Assert.Single(inconsistencias);
        Assert.Equal(TipoInconsistencia.MarcasDuplicadasDescartadas, inconsistencias[0].Tipo);
    }

    /// <summary>
    /// Verifica que con dos marcas se calcule la jornada sin descuento de almuerzo
    /// y se deje registrada la inconsistencia.
    /// </summary>
    [Fact]
    public void Procesar_ConDosMarcas_CalculaSinAlmuerzoYReportaInconsistencia()
    {
        var dia = new MarcacionesDia
        {
            Empleado = "EMP003",
            Fecha = new DateOnly(2026, 6, 10),
            Marcas = new List<TimeOnly>
            {
                new TimeOnly(7, 0, 0),
                new TimeOnly(15, 0, 0)
            }
        };
        var inconsistencias = new List<Inconsistencia>();
        var calculador = new CalculadorAsistencia();

        var resultado = calculador.Procesar(dia, inconsistencias);

        Assert.Equal(8, resultado.HorasTrabajadas);
        Assert.Single(inconsistencias);
        Assert.Equal(TipoInconsistencia.MarcacionesIncompletas, inconsistencias[0].Tipo);
    }

    /// <summary>
    /// Verifica que una sola marcacion no produzca horas trabajadas.
    /// </summary>
    [Fact]
    public void Procesar_ConUnaSolaMarca_NoCalculaHorasYReportaInconsistencia()
    {
        var dia = new MarcacionesDia
        {
            Empleado = "EMP004",
            Fecha = new DateOnly(2026, 6, 11),
            Marcas = new List<TimeOnly> { new TimeOnly(8, 0, 0) }
        };
        var inconsistencias = new List<Inconsistencia>();
        var calculador = new CalculadorAsistencia();

        var resultado = calculador.Procesar(dia, inconsistencias);

        Assert.Null(resultado.HorasTrabajadas);
        Assert.Single(inconsistencias);
        Assert.Equal(TipoInconsistencia.MarcacionesIncompletas, inconsistencias[0].Tipo);
    }

    /// <summary>
    /// Verifica que una cantidad de marcaciones fuera de lo esperado quede reportada.
    /// </summary>
    [Fact]
    public void Procesar_ConSeisMarcasSeparadas_NoCalculaHorasYReportaCantidadDiferente()
    {
        var dia = new MarcacionesDia
        {
            Empleado = "EMP005",
            Fecha = new DateOnly(2026, 6, 12),
            Marcas = new List<TimeOnly>
            {
                new TimeOnly(5, 55, 0),
                new TimeOnly(12, 0, 0),
                new TimeOnly(12, 30, 0),
                new TimeOnly(13, 0, 0),
                new TimeOnly(13, 30, 0),
                new TimeOnly(16, 0, 0)
            }
        };
        var inconsistencias = new List<Inconsistencia>();
        var calculador = new CalculadorAsistencia();

        var resultado = calculador.Procesar(dia, inconsistencias);

        Assert.Null(resultado.HorasTrabajadas);
        Assert.Single(inconsistencias);
        Assert.Equal(TipoInconsistencia.CantidadDiferenteALaEsperada, inconsistencias[0].Tipo);
    }
}
