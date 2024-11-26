using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sintaxis_1;
/*
    REQUERIMIENTOS
    1.- Concatenacion
    2.- Inicializar una variable desde la declaracion
    3.- Evaluar las expresiones matematicas
    4.- Levantar excepcion si en el Read no se ingresan numeros **** Hacerlo en asignacion ****
    5.- Modificar la variable con el resto de operadores (incrementoFactor, incrementoTermino) **** Hacerlo en asignacion ****
    6.- 
*/

namespace Sintaxis_1
{

    public class Lenguaje : Sintaxis
    {
        Stack<float> s;
        List<Variable> l;
        public Lenguaje() : base()
        {
            s = new Stack<float>();
            l = new List<Variable>();
            //log.WriteLine("Constructor lenguaje");
        }

        public Lenguaje(string name) : base(name)
        {
            s = new Stack<float>();
            l = new List<Variable>();
            //log.WriteLine("Constructor lenguaje");
        }

        private void displayStack()
        {
            //Console.WriteLine("Contenido del Stack");
            foreach (float elemento in s)
            {
                Console.WriteLine(elemento);
            }
        }

        private void displayLista()
        {
            log.WriteLine("Lista de variables");
            foreach (Variable elemento in l)
            {
                log.WriteLine($"{elemento.getNombre()} {elemento.getTipoDato()} {elemento.getValor()}");

            }
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
            displayLista();
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
            Variable.TipoDato t = Variable.TipoDato.Char;
            switch (getContenido())
            {
                case "int": t = Variable.TipoDato.Int; break;
                case "float": t = Variable.TipoDato.Float; break;
            }
            match(Tipos.TipoDato);
            ListaIdentificadores(t);
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
        private void ListaIdentificadores(Variable.TipoDato t)
        {
            if (l.Find(Variable => Variable.getNombre() == getContenido()) != null)
            {
                throw new Error("La variable " + getContenido() + " ya existe en Linea: " + linea + " Columna: " + columna);
            }
            l.Add(new Variable(t, getContenido()));
            match(Tipos.Identificador);

            if(getContenido() == "=")
            {

            }

            if (getContenido() == ",")
            {
                match(",");
                ListaIdentificadores(t);
            }
        }
        // BloqueInstrucciones -> { listaIntrucciones? }
        private void BloqueInstrucciones(bool ejecutar)
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones(ejecutar);
            }
            else
            {
                match("}");
            }
        }
        // ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones(bool ejecutar)
        {
            Instruccion(ejecutar);
            if (getContenido() != "}")
            {
                ListaInstrucciones(ejecutar);
            }
            else
            {
                match("}");
            }
        }


        // Instruccion -> console | If | While | do | For | Variables | Asignación
        private void Instruccion(bool ejecutar)
        {
            if (getContenido() == "Console")
            {
                console(ejecutar);
            }
            else if (getContenido() == "if")
            {
                If(ejecutar);
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
            Variable? v = l.Find(Variable => Variable.getNombre() == getContenido());
            if (v == null)
            {
                throw new Error("La variable " + getContenido() + " NO esta definida");
            }

            //Console.Write(getContenido() + "=");
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
            else if (getContenido() == "+=" || getContenido() == "-=")
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

            float r = s.Pop();
            v.setValor(r);

            //Console.WriteLine();
            //Console.WriteLine(" = " + s.Pop());
            //displayStack();
        }

        // If -> if (Condicion) bloqueInstrucciones | instruccion
        // (else bloqueInstrucciones | instruccion)?
        private void If(bool ejecutar2)
        {
            match("if");
            match("(");
            bool ejecutar = Condicion() && ejecutar2;
            Console.WriteLine(ejecutar);
            match(")");

            if (getContenido() == "{")
            {
                BloqueInstrucciones(ejecutar);
            }
            else
            {
                Instruccion(ejecutar);
            }

            if (getContenido() == "else")
            {
                match("else");

                if (getContenido() == "{")
                {
                    BloqueInstrucciones(false);
                }
                else
                {
                    Instruccion(false);
                }
            }
        }
        // Condicion -> Expresion operadorRelacional Expresion
        private bool Condicion()
        {
            Expresion();
            float valor1 = s.Pop();
            string operador = getContenido();
            match(Tipos.OperadorRelacional);
            Expresion();
            float valor2 = s.Pop();

            switch (operador)
            {
                case ">": return valor1 > valor2;
                case "<": return valor1 < valor2;
                case ">=": return valor1 >= valor2;
                case "<=": return valor1 <= valor2;
                case "==": return valor1 == valor2;
                default: return valor1 != valor2;
                
            }
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
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
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
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
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
                BloqueInstrucciones(true);
            }
            else
            {
                Instruccion(true);
            }
        }
        // Console -> Console.(WriteLine|Write) (cadena concatenaciones?);
        private void console(bool ejecutar)
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

            if(ejecutar == true)
            {
                // imprimir el console
            }
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
            BloqueInstrucciones(true);

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
                string operador = getContenido();
                match(Tipos.OperadorTermino);
                Termino();
                //Console.Write(operador + " " );
                float n1 = s.Pop();
                float n2 = s.Pop();

                switch (operador)
                {
                    case "+": s.Push(n2 + n1); break;
                    case "-": s.Push(n2 - n1); break;
                }
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
                string operador = getContenido();
                match(Tipos.OperadorFactor);
                Factor();
                //Console.Write(operador + " ");
                float n1 = s.Pop();
                float n2 = s.Pop();

                switch (operador)
                {
                    case "*": s.Push(n2 * n1); break;
                    case "/": s.Push(n2 / n1); break;
                    case "%": s.Push(n2 % n1); break;

                }
            }
        }
        // Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasificacion() == Tipos.Numero)
            {
                s.Push(float.Parse(getContenido()));
                //Console.Write(getContenido() + " " );
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                Variable? v = l.Find(Variable => Variable.getNombre() == getContenido());
                if (v == null)
                {
                    throw new Error("La variable " + getContenido() + " NO esta definida");
                }

                s.Push(v.getValor());
                //Console.Write(getContenido() + " " );
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