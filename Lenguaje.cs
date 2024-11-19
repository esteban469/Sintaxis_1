using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
    REQUERIMIENTOS
    1) Indicar en el error léxico o sintáctico el número de línea y columna
    2) En el log colocar el nombre del archivo a compilar, la fecha y la hora, ejemplo:

    3) Agregar el resto de asignaciones:
        ID = Expresion
        ID++
        ID--
        ID IncrementoTermino Expresion --> a += b+1 || a -= 1
        ID IncrementoFactor Expresion --> a *= b+1 || a %= 1
        ID = Console.Read()
        ID = Console.ReadLine()
    4) Emular el Console.Write() y Console.WriteLine()
    5) Emular el Console.Read() y Console.ReadLine()
*/

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
        // ? Cerradura epsilon
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

            Main();
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

        // ListaIdentificadores -> identificador (,ListaIdentificadores)?
        private void ListaIdentificadores()
        {
            match(Tipos.Identificador);

            if (getContenido() == ",")
            {
                match(",");
                ListaIdentificadores();
            }
        }
        // BloqueInstrucciones -> { listaIntrucciones? }
        private void BloqueInstrucciones()
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones();
            }
            else
            {
                match("}");
            }
        }
        // ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones()
        {
            Instruccion();
            if (getContenido() != "}")
            {
                ListaInstrucciones();
            }
            else
            {
                match("}");
            }
        }


        // Instruccion -> console | If | While | do | For | Variables | Asignación
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
            else if (getClasificacion() == Tipos.TipoDato)
            {
                Variables();
            }
            else
            {
                Asignacion();

            }
        }
        /*3) Agregar el resto de asignaciones:
         ID = Expresion
         ID++
         ID--
         ID IncrementoTermino Expresion --> a += b+1 || a -= 1
         ID IncrementoFactor Expresion --> a *= b+1 || a %= 1
         ID = Console.Read()
         ID = Console.ReadLine()*/
        private void Asignacion()
        {
            match(Tipos.Identificador);

            if (getContenido() == "=")
            {
                match("=");
                if (getContenido() == "Console")
                {
                    match("Console");
                    match(".");
                    if (getContenido() == "Read")
                    {
                        match("Read");
                        match("(");
                        Console.Read();
                        match(")");
                        match(";");
                    }
                    else 
                    {
                        match("ReadLine");
                        match("(");
                        Console.ReadLine();
                        match(")");
                        match(";");
                    }
                }
                else
                {
                    Expresion();
                    match(";");
                }
            }
            else if (getContenido() == "++" || getContenido() == "--")
            {
                match(Tipos.IncrementoTermino);
                if (getContenido() != ")")
                {
                    match(";");
                }
            }
            else if(getContenido() == "+=" || getContenido() == "-=")
            {
                match(Tipos.IncrementoTermino);
                Expresion();
                if (getContenido() != ")")
                {
                    match(";");
                }
            }
            else if (getContenido() == "*=" || getContenido() == "%=")
            {
                match(Tipos.IncrementoFactor);
                Expresion();
                if (getContenido() != ")")
                {
                    match(";");
                }
            }
        }

        // If -> if (Condicion) bloqueInstrucciones | instruccion
        // (else bloqueInstrucciones | instruccion)?
        private void If()
        {
            match("if");
            match("(");
            Condicion();
            match(")");

            if (getContenido() == "{")
            {
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }

            if (getContenido() == "else")
            {
                match("else");

                if (getContenido() == "{")
                {
                    BloqueInstrucciones();
                }
                else
                {
                    Instruccion();
                }
            }
        }
        // Condicion -> Expresion operadorRelacional Expresion
        private void Condicion()
        {
            Expresion();
            match(Tipos.OperadorRelacional);
            Expresion();
        }
        // While -> while(Condicion) bloqueInstrucciones | instruccion
        private void While()
        {
            match("while");
            match("(");
            Condicion();
            match(")");

            if (getContenido() == "{")
            {
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }
        // Do -> do 
        // bloqueInstrucciones | intruccion 
        // while(Condicion);
        private void Do()
        {
            match("do");

            if (getContenido() == "{")
            {
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }

            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");
        }
        // For -> for(Asignacion; Condicion; Asignación) 
        // BloqueInstrucciones | Intruccion
        private void For()
        {
            match("for");
            match("(");
            Asignacion();
            //match(";");
            Condicion();
            match(";");
            Asignacion();
            match(")");

            if (getContenido() == "{")
            {
                BloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }
        // Console -> Console.(WriteLine|Write) (cadena concatenaciones?);
        private void console()
        {
            match("Console");
            match(".");
            if (getContenido() == "WriteLine")
            {
                match("WriteLine");
                match("(");
                Console.WriteLine(getContenido().Trim('\"').Trim(')'));
                if (getClasificacion() == Tipos.Cadena)
                {
                    match(Tipos.Cadena);
                }
            }
            else if (getContenido() == "Write")
            {
                match("Write");
                match("(");
                Console.WriteLine(getContenido().Trim('\"').Trim(')'));
                if (getClasificacion() == Tipos.Cadena)
                {
                    match(Tipos.Cadena);
                }
            }
            match(")");
            match(";");
        }

        // Main -> static void Main(string[] args) BloqueInstrucciones 
        private void Main()
        {

            match("static");
            match("void");
            match("Main");
            match("(");
            match("string");
            match("[");
            match("]");
            match("args");
            match(")");
            BloqueInstrucciones();

        }
        // Expresion -> Termino MasTermino
        private void Expresion()
        {
            Termino();
            MasTermino();
        }
        // MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                match(Tipos.OperadorTermino);
                Termino();
            }
        }
        // Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();
        }
        // PorFactor -> (OperadorFactor Factor)?
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                match(Tipos.OperadorFactor);
                Factor();
            }
        }
        // Factor -> numero | identificador | (Expresion)
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