using System;
using System.Collections.Generic;

namespace AList
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<int> list = new List<int>() {1, 2, 3, 4};
            list.Remove(10);
        }
    }
}