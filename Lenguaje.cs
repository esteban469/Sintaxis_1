using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sintaxis_1
{
    public class Lenguaje : Sintaxis
    {
        public Lenguaje() : base()
        {
            log.WriteLine("Constructor lenguaje");
        }

        public Lenguaje(string name) : base(name)
        {
            log.WriteLine("Constructor lenguaje");
        }

        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            if (getContenido() == "using")
            {
                Librerias();
            }

            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
            main();
        }

        //Librerias -> using ListaLibrerias; Librerias?
        private void Librerias()
        {
            match("using");
            ListaLibrerias();
            match(";");

            if (getContenido() == "using")
            {
                Librerias();
            }
        }

        //Variables -> tipo_dato Lista_identificadores; Variables?
        private void Variables()
        {
            match(Tipos.TipoDato);
            ListaIdentificadores();
            match(";");
            if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
        }

        //ListaLibrerias -> identificador (.ListaLibrerias)?
        private void ListaLibrerias()
        {
            match(Tipos.Identificador);
            if (getContenido() == ".")
            {
                match(".");
                ListaLibrerias();
            }
        }

        //ListaIdentificadores -> identificador (,ListaIdentificadores)?
        private void ListaIdentificadores()
        {
            match(Tipos.Identificador);
            if (getContenido() == ".")
            {
                match(".");
                ListaIdentificadores();
            }
        }

        //BloqueInstrucciones -> { listaIntrucciones? }
        private void BloqueInstrucciones()
        {
            ListaInstrucciones();
            match("{");
        }

        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones()
        {
            Instruccion();
            if (getContenido() != "}")
            {
                ListaInstrucciones();
            }
        }


        //Instruccion -> Console | If | While | do | For | Variables | Asignación
        private void Instruccion()
        {
            if (getContenido() == "Console")
            {
                console();
            }
            else if (getContenido() == "if")
            {
                If();
            }
            else if (getContenido() == "while")
            {
                While();
            }
            else if (getContenido() == "do")
            {
                Do();
            }
            else if (getContenido() == "for")
            {
                For();
            }

        }

        //Asignacion -> Identificador = Expresion;
        private void Asignacion()
        {
            match(Tipos.Identificador);
            match("=");
            Expresion();
            match(";");
        }

        //If -> if (Condicion) bloqueInstrucciones | instruccion
        //(else bloqueInstrucciones | instruccion)?
        private void If()
        {
            match("if");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
            {
                match("{");
                BloqueInstrucciones();
                match("}");
                if (getContenido() == "else")
                {
                    match("else");
                    match("{");
                    BloqueInstrucciones();
                    match("}");

                }
            }
            else
            {
                Instruccion();
            }

        }

        //Condicion -> Expresion operadorRelacional Expresion
        private void Condicion()
        {
            Expresion();
            match(Tipos.OperadorRelacional);
            Expresion();
        }

        //While -> while(Condicion) bloqueInstrucciones | instruccion
        private void While()
        {
            match("while");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
            {
                match("{");
                BloqueInstrucciones();
                match("}");
            }
            else
            {
                Instruccion();
            }
        }
        //Do -> do 
        //bloqueInstrucciones | instruccion 
        //while(Condicion);
        private void Do()
        {
            match("do");
            if (getContenido() == "{")
            {
                match("{");
                BloqueInstrucciones();
                match("}");
                match("while");
                match("(");
                Condicion();
                match(")");
                match(";");
            }
            else
            {
                Instruccion();
                match("while");
                match("(");
                Condicion();
                match(")");
                match(";");
            }
        }
        //For -> for(Asignacion; Condicion; Asignacion)
        private void For()
        {
            match("for");
            match("(");
            Asignacion();
            match(";");
            Condicion();
            match(";");
            Asignacion();
            match(")");
        }
        
        //Console -> Console.(WriteLine|Write) (cadena concatenaciones?);
        private void console()
        {
            match("Console");
            match(".");
            if(getContenido() == "WriteLine")
            {
                match("WriteLine");
                match("(");
                CadenaConcatenacion();
                match(")");
                match(";");
            }
            else if(getContenido() == "Write")
            {
                match("Write");
                match("(");
                CadenaConcatenacion();
                match(")");
                match(";");
            }
        }
        //CadenaConcatenacion -> cadena 
        private void CadenaConcatenacion()
        {
            match("\"");
            if(getContenido() != "\"")
            {
                CadenaConcatenacion();
            }
            else
            {
            match("\"");
            }
        }

        //Main      -> static void Main(string[] args) BloqueInstrucciones 
        private void main()
        {
            if (getContenido() == "static")
            {
                match("static");
                match("void");
                match("Main");
                match("(");
                match("string");
                match("[");
                match("]");
                match("args");
                BloqueInstrucciones();
            }
        }

        //Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                match(Tipos.OperadorTermino);
                Termino();
            }
        }
        //T
        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        //PorFactor -> (OperadorFactor Factor)?
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                match(Tipos.OperadorFactor);
                Factor();
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasificacion() == Tipos.Numero)
            {
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                match(Tipos.Identificador);
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }

    }
}