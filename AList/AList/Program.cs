// See https://aka.ms/new-console-template for more information

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json;
using AList;


InterestingList<int> lst = new();
lst.Add(5);
lst.Add(10);
lst.Add(20);
lst.Add(30);
for (int i = 40; i < 10000; i+=10)
{
    lst.Add(i);
}
Console.WriteLine(lst._array.ToList().IndexOf(1200));
Console.WriteLine(lst.SingleAsync(1200));

string jsonstring = System.Text.Json.JsonSerializer.Serialize(lst);
Console.WriteLine(jsonstring);

var hm = JsonSerializer.Deserialize<InterestingList<int>>(jsonstring);
Console.WriteLine(hm.Length);

//Console.WriteLine(lst.Single());