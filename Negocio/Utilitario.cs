using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class Utilitario
    {
        public  string Encriptar(string texto)
        {
            string clave = "RQA";
            byte[] textoBytes = System.Text.Encoding.UTF8.GetBytes(texto);
            byte[] claveBytes = System.Text.Encoding.UTF8.GetBytes(clave);

            for (int i = 0; i < textoBytes.Length; i++)
            {
                textoBytes[i] = (byte)(textoBytes[i] ^ claveBytes[i % claveBytes.Length]);
            }

            return Convert.ToBase64String(textoBytes);
        }

        public string Desencriptar(string textoEncriptado)
        {
            string clave = "RQA";

            byte[] textoBytes = Convert.FromBase64String(textoEncriptado);
            byte[] claveBytes = System.Text.Encoding.UTF8.GetBytes(clave);

            for (int i = 0; i < textoBytes.Length; i++)
            {
                textoBytes[i] = (byte)(textoBytes[i] ^ claveBytes[i % claveBytes.Length]);
            }

            return System.Text.Encoding.UTF8.GetString(textoBytes);
        }

    }
}
