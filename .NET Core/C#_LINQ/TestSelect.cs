using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__LINQ
{
    internal class TestSelect
    {
        internal static void ObjectsPopulatedNotified(Object? sender, EventArguments args)
        {
            Console.WriteLine();
            Console.WriteLine("Testing Select");

            // Query Syntax
            //var students = from s in args.Students
            //               select new { Apelido = "Mr. " + s.FirstName, Idade = s.Age};

            // Method Syntax
            var students = args.Students.Select(s => new { Apelido = "Mr. " + s.FirstName, Idade = s.Age });
            
            foreach (var student in students)
            {
                Console.WriteLine($"{student.Apelido} is {student.Idade} year old");
            }
        }
    }
}
