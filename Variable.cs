using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sintaxis_1
{
    public class Variable
    {
        public enum TipoDato
        {
            Char,Int,Float
        }
    TipoDato tipo;
    String nombre;
    float valor;

    public Variable(TipoDato tipo, String nombre, float valor = 0){
        this.tipo = tipo;
        this.nombre = nombre;
        this.valor = valor;
    }

    public void setValor(float valor){
        this.valor = valor;
    }
    public float getValor(){
        return valor;
    }

    public string getNombre(){
        return nombre;
    }

    public TipoDato getTipoDato(){
        return tipo;
    }



    } 
}