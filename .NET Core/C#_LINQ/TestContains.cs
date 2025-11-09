using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace C__LINQ
{
    internal class TestContains
    {
        internal static void ObjectsPopulatedNotified(object? sender, EventArguments args)
        {
            Console.WriteLine();
            Console.WriteLine("Testing Contains");

            //var firstNames = args.Students.Select(s => s.FirstName).ToList();
            bool anyoneNamedJennifer = args.Students.Select(s => s.FirstName).Contains("William");
            Console.WriteLine($"Any student named William? {anyoneNamedJennifer}");
            
            // Contains extension method only compares reference of an object but not the actual values of an object.So to compare values of the student object,
            // you need to create a class by implementing IEqualityComparer interface, that compares values of two Student objects and returns boolean.
        }
    }
}
