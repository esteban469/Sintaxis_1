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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
