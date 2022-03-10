﻿using System;
namespace Compilador.Core
{
    public enum TokenType
    {
        DolarVariableGlobal,
        IgualAsignacion,
        CorcheteIzq,
        CorcheteDer,
        Comma,
        TruePalabraReservada,
        FalsePalabraReservada,
        NumerosLiteral,
        StringLiteral,
        ComiaIzq,
        ComiaDer,
        Mas,
        Menos,
        Multiplicacion,
        Division,
        Porcentaje,
        IgualDoble,
        Distinto,
        Menor,
        Mayor,
        MenorIgual,
        MayorIgual,
        And,
        Or,
        Not,
        Incremento,
        Decremento,
        EndPalabraReservada,
        ForPalabraReservada,
        InPalabraReservada,
        DoPalabraReservada,
        WhilePalabraReservada,
        BreakPalabraReservada,
        LoopPalabraReservada,
        IfPalabraReservada,
        ElsePalabraReservada,
        EachPalabraReservada,
        HashtagComentario,
        BeginPalabraReservada,
        DefPalabraReservada,
        ParentesisIzq,
        ParentesisDer,
        ClassPalabraReservada,
        InitializePalabraReservada,
        NewPalabraReservada,
        PutsPalabraReservada,
        GetsPalabraReservada,
        identificador,
        MultiLinCommentPalabraReservada,
        MultiLinEndPalabraReservada,
        Punto,
        FinaldelArchivo,
        chompPalabraReservada,
        to_iPalabraReservada,
        to_SPalabraReservada,
        BasicType,
        pipe, 
        ComplexType
    }
   
}