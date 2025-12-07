using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Negocio
{
    public class Utilitario
    {

        public static string ConvertirMontoALetras(decimal monto)
        {
            long parteEntera = (long)Math.Truncate(monto);
            int centavos = (int)Math.Round((monto - parteEntera) * 100);

            string letras = NumeroALetras(parteEntera);

            return $"{letras} CON {centavos:00}/100 CÓRDOBAS";
        }

        private static string NumeroALetras(long numero)
        {
            if (numero == 0)
                return "CERO";

            if (numero < 0)
                return "MENOS " + NumeroALetras(Math.Abs(numero));

            string letras = "";

            if ((numero / 1000000) > 0)
            {
                if (numero / 1000000 == 1)
                    letras += "UN MILLÓN ";
                else
                    letras += NumeroALetras(numero / 1000000) + " MILLONES ";

                numero %= 1000000;
            }

            if ((numero / 1000) > 0)
            {
                if (numero / 1000 == 1)
                    letras += "MIL ";
                else
                    letras += NumeroALetras(numero / 1000) + " MIL ";

                numero %= 1000;
            }

            if ((numero / 100) > 0)
            {
                if (numero / 100 == 1 && numero % 100 == 0)
                    letras += "CIEN ";
                else
                    letras += Centenas(numero / 100);

                numero %= 100;
            }

            if (numero > 0)
                letras += Decenas(numero);

            return letras.Trim();
        }

        private static string Centenas(long numero)
        {
            switch (numero)
            {
                case 1: return "CIENTO ";
                case 2: return "DOSCIENTOS ";
                case 3: return "TRESCIENTOS ";
                case 4: return "CUATROCIENTOS ";
                case 5: return "QUINIENTOS ";
                case 6: return "SEISCIENTOS ";
                case 7: return "SETECIENTOS ";
                case 8: return "OCHOCIENTOS ";
                case 9: return "NOVECIENTOS ";
                default: return "";
            }
        }

        private static string Decenas(long numero)
        {
            if (numero < 10)
                return Unidades(numero);

            if (numero >= 10 && numero < 20)
            {
                switch (numero)
                {
                    case 10: return "DIEZ";
                    case 11: return "ONCE";
                    case 12: return "DOCE";
                    case 13: return "TRECE";
                    case 14: return "CATORCE";
                    case 15: return "QUINCE";
                    case 16: return "DIECISÉIS";
                    case 17: return "DIECISIETE";
                    case 18: return "DIECIOCHO";
                    case 19: return "DIECINUEVE";
                }
            }

            if (numero >= 20 && numero < 30)
            {
                if (numero == 20)
                    return "VEINTE";
                return "VEINTI" + Unidades(numero % 10).ToLower();
            }

            string[] decenas = {
            "", "", "VEINTE", "TREINTA", "CUARENTA", "CINCUENTA",
            "SESENTA", "SETENTA", "OCHENTA", "NOVENTA"
        };

            long decena = numero / 10;
            long unidad = numero % 10;

            if (unidad == 0)
                return decenas[decena];

            return decenas[decena] + " Y " + Unidades(unidad);
        }

        private static string Unidades(long numero)
        {
            switch (numero)
            {
                case 1: return "UNO";
                case 2: return "DOS";
                case 3: return "TRES";
                case 4: return "CUATRO";
                case 5: return "CINCO";
                case 6: return "SEIS";
                case 7: return "SIETE";
                case 8: return "OCHO";
                case 9: return "NUEVE";
                default: return "";
            }
        }

    }
}
