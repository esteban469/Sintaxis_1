using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sintaxis_1
{
    public class Sintaxis : Lexico
    {
        public Sintaxis() : base()
        {
            nextToken();
        }

        public Sintaxis(string nombre) : base(nombre)
        {
            nextToken();
        }
        public void match(string contenido)
        {
            if (contenido == getContenido())
            {
                nextToken();
            }
            else
            {
                throw new Error("Sintaxis: Se espera \" " + contenido + " \" en: [linea " + linea + ", columna " + columna + "]");
            }
        }

        public void match(Tipos clasificacion)
        {
            if (clasificacion == getClasificacion())
            {
                nextToken();
            }
            else
            {
                throw new Error("Sintaxis: Se espera \" " + clasificacion + " \" en: [linea " + linea + ", columna " + columna + "]");
            }
        }
    }
}