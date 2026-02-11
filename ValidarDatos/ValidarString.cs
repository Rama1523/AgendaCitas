using System;
using System.Text;

namespace ValidarDatos
{
    public class ValidarString
    {
        public static string LimiteCatacteres(string Parametro, int longitudMaxima)
        {
            if (!string.IsNullOrEmpty(Parametro))
            {
                Parametro = Parametro.Trim().Normalize(NormalizationForm.FormC);
                if (Parametro.Length > longitudMaxima)
                {
                    return "-1";
                }
                return Parametro;
            }

            return "-2";
        }
    }
}
