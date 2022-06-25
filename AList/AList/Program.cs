// See https://aka.ms/new-console-template for more information

using System;
using System.Numerics;


T Add<T>(T lhs, T rhs)
    where T : INumber<T>
{
    return lhs + rhs;
}

decimal d = 1*10;

Console.WriteLine((int)d is int);