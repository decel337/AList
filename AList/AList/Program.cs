// See https://aka.ms/new-console-template for more information

using System;
using System.Text.Json;
using AList;


Console.WriteLine("Hello World");

var lst = new InterestingList<double>();
for (int i = -1000; i < 10000; i+=10)
{
    lst.Add(i);
}

string jsonstring = JsonSerializer.Serialize(lst);
Console.WriteLine(jsonstring);
Console.WriteLine(new String('-', 15));
var asSameLst = JsonSerializer.Deserialize<InterestingList<double>>(jsonstring);
Console.WriteLine(asSameLst.ToString());
