using NPOI.SS.UserModel;

namespace ControlAsistencia.Core
{
    public class LectorExcel
    {
        public List<MarcacionesDia> Leer(string rutaArchivo)
        {
            var resultado = new List<MarcacionesDia>();

            using var stream = File.OpenRead(rutaArchivo);
            IWorkbook libro = WorkbookFactory.Create(stream);
            ISheet hoja = libro.GetSheetAt(0);

            for (int i = 1; i <= hoja.LastRowNum; i++)
            {
                IRow fila = hoja.GetRow(i);
                if (fila == null) continue;

                var dia = new MarcacionesDia
                {
                    Empleado = fila.GetCell(0).ToString(),
                    Fecha = DateOnly.Parse(fila.GetCell(1).ToString())
                };

                string textoHoras = fila.GetCell(5).ToString();
                foreach (string textoHora in textoHoras.Split(';'))
                {
                    if (TimeOnly.TryParse(textoHora, out TimeOnly hora))
                    {
                        dia.Marcas.Add(hora);
                    }
                }

                dia.Marcas.Sort();
                resultado.Add(dia);
            }

            return resultado;
        }
    }
}
    

