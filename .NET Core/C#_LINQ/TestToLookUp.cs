using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C__LINQ.Models;

namespace C__LINQ
{
    internal class TestToLookUp
    {
        internal static void ObjectsPopulatedNotified(object? sender, List<Student> students)
        {
            Console.WriteLine();
            Console.WriteLine("Testing ToLookup");
            
            // Only available in Method Syntax
            var studentToLookUp = students.ToLookup(s => s.Age);
            
            foreach (var group in studentToLookUp)
            {
                Console.WriteLine($"Age: {group.Key}");
                foreach (var student in group)
                {
                    Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName + " " + student.LastName}, Age: {student.Age}");
                }
            }
        }
    }
}
