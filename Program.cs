using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sintaxis_1
{
    class Program : Token
    {
        static void Main(string[] args)
        {

            try
            {
                {
                    using Lenguaje l = new("prueba.cpp");
                    //token.GetAllTokens();
                    l.Programa();


                    // *************** LEER HORA Y FECHA DE PRUEBA.CPP ****************************
                    string path = @"C:\Users\Esteban\Documents\ITQ\AUTOMATAS\Sintaxis_1\prueba.cpp";
                    DateTime dt = File.GetLastWriteTime(path);
                    File.SetLastWriteTime(path, DateTime.Now);
                    dt = File.GetLastWriteTime(path);
                    l.log.WriteLine("La ultima modificacion del archivo \"prueba.cpp\" fue: {0}.", dt);
                    // *************** FIN LEER HORA Y FECHA DE PRUEBA.CPP ****************************
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

        }
    }
}
