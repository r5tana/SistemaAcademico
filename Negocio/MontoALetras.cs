using System;
using System.Text;

public static class MontoALetras
{
    private static readonly string[] Unidades = {
        "", "UN", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE"
    };

    private static readonly string[] DecenasEspeciales = {
        "DIEZ", "ONCE", "DOCE", "TRECE", "CATORCE", "QUINCE", "DIECISÉIS", "DIECISIETE", "DIECIOCHO", "DIECINUEVE"
    };

    private static readonly string[] Decenas = {
        "", "", "VEINTE", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA"
    };

    private static readonly string[] Centenas = {
        "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS"
    };

    /// <summary>
    /// Convierte la parte entera de un número (hasta 999.999.999) a su representación en letras.
    /// </summary>
    private static string ConvertirGrupo(int n, bool esMillon)
    {
        if (n == 0) return "";

        var sb = new StringBuilder();

        // 1. Centenas (ej. "CIENTO", "DOSCIENTOS")
        int centena = n / 100;
        if (centena > 0)
        {
            if (n == 100)
            {
                sb.Append("CIEN ");
            }
            else
            {
                sb.Append(Centenas[centena]).Append(" ");
            }
        }
        int resto = n % 100;

        // 2. Decenas y Unidades
        if (resto > 0)
        {
            if (resto < 10) // 1-9
            {
                if (resto == 1 && esMillon)
                {
                    // "UN" MILLÓN/MIL
                    sb.Append("UN ");
                }
                else
                {
                    sb.Append(Unidades[resto]).Append(" ");
                }
            }
            else if (resto < 20) // 10-19
            {
                sb.Append(DecenasEspeciales[resto - 10]).Append(" ");
            }
            else
            {
                int decena = resto / 10;
                int unidad = resto % 10;

                if (resto == 20) // 20
                {
                    sb.Append("VEINTE ");
                }
                else if (resto < 30) // 21-29 (ej. VEINTIUNO)
                {
                    sb.Append("VEINTI").Append(Unidades[unidad]).Append(" ");
                }
                else // 30-99 (ej. TREINTA Y CINCO)
                {
                    sb.Append(Decenas[decena]);
                    if (unidad > 0)
                    {
                        sb.Append(" Y ").Append(Unidades[unidad]);
                    }
                    sb.Append(" ");
                }
            }
        }
        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Convierte un monto decimal a letras con centavos en número, usando la moneda CÓRDOBAS.
    /// </summary>
    /// <param name="monto">El monto a convertir.</param>
    /// <returns>La cadena de texto del monto.</returns>
    public static string ConvertirMontoACordobas(decimal monto)
    {
        // 1. Validar que el monto no sea negativo
        if (monto < 0)
        {
            return "MONTO NEGATIVO NO SOPORTADO";
        }

        // 2. Separar la parte entera (córdobas) y la parte decimal (centavos)
        long cordobas = (long)Math.Truncate(monto);

        // Multiplicar los decimales por 100 y redondear al entero más cercano.
        int centavos = (int)((monto - cordobas) * 100);

        string textoCordobas = ConvertirNumeroEntero(cordobas);

        // 3. Formato final
        StringBuilder resultado = new StringBuilder();

        if (cordobas == 0 && centavos == 0)
        {
            resultado.Append("CERO");
        }
        else
        {
            resultado.Append(textoCordobas);

            // Añadir el singular/plural de la moneda
            if (cordobas == 1 && centavos == 0)
            {
                resultado.Append(" CÓRDOBA");
            }
            else
            {
                resultado.Append(" CÓRDOBAS");
            }
        }

        // 4. Añadir los centavos en formato numérico
        resultado.Append($" CON {centavos:D2}/100"); // :D2 asegura dos dígitos (ej. 05)

        return resultado.ToString().Trim().ToUpper();
    }

    /// <summary>
    /// Método auxiliar para dividir el número entero en grupos de miles y millones.
    /// </summary>
    private static string ConvertirNumeroEntero(long n)
    {
        if (n == 0) return "";

        string letras = "";

        // Millones
        int millones = (int)(n / 1000000);
        if (millones > 0)
        {
            letras += ConvertirGrupo(millones, true) + (millones == 1 ? " MILLÓN " : " MILLONES ");
        }

        // Miles
        int miles = (int)((n % 1000000) / 1000);
        if (miles > 0)
        {
            // La palabra "MIL" no lleva "UN"
            if (miles == 1)
            {
                letras += "MIL ";
            }
            else
            {
                letras += ConvertirGrupo(miles, false) + " MIL ";
            }
        }

        // Cientos
        int cientos = (int)(n % 1000);
        if (cientos > 0)
        {
            letras += ConvertirGrupo(cientos, false);
        }

        return letras.Trim();
    }
}