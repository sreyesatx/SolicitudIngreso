using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SolicitudValidar
{
    public static class Validar
    {
        public static string ValidaString(string p)
        {
            StringBuilder b = new StringBuilder(p);
            b.Replace("#", "");
            b.Replace("%", "");
            b.Replace("&", "");
            b.Replace("'", "");
            b.Replace("(", "");
            b.Replace(")", "");
            b.Replace("/", "");
            b.Replace("\"", "");
            b.Replace(":", "");
            b.Replace(";", "");
            b.Replace("<", "");
            b.Replace(">", "");
            b.Replace("=", "");
            b.Replace("[", "");
            b.Replace("]", "");
            b.Replace("?", "");
            b.Replace("¿", "");
            b.Replace("`", "");
            b.Replace("|", "");
            b.Replace("{", "");
            b.Replace("}", "");
            b.Replace("°", "");
            b.Replace("!", "");
            b.Replace("¡", "");
            b.Replace("^", "");
            b.Replace("~", "");
            b.Replace("$", "");
            b.Replace("@", "");
            b.Replace("»", "");
            b.Replace("Ï", "");
            b.Replace("\r", "");
            b.Replace("\n", "");
            b.Replace(@"\", "");

            return b.ToString();
        }

        public static bool ValidaRut(string txtRut)
        {
            try
            {
                if (txtRut.Trim() != string.Empty)
                {
                    var rutValidar = txtRut.Split('-');
                    int rut = int.Parse(rutValidar[0]);
                    string digitoVerificador = rutValidar[1].ToUpper();

                    if (digitoVerificador == "P")
                    {
                        return true;
                    }

                    int contador = 2;
                    int acumulador = 0;

                    while (rut != 0)
                    {
                        int multiplo = (rut % 10) * contador;
                        acumulador = acumulador + multiplo;
                        rut = rut / 10;
                        contador = contador + 1;
                        if (contador == 8)
                        {
                            contador = 2;
                        }
                    }

                    int digito = 11 - (acumulador % 11);
                    string rutDigito = digito.ToString().Trim();

                    if (digito == 10)
                    {
                        rutDigito = "K";
                    }
                    if (digito == 11)
                    {
                        rutDigito = "0";
                    }

                    if (rutDigito == digitoVerificador)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}