using C__LINQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__LINQ
{
    internal class TestGroupBy
    {
        internal static void ObjectsPopulatedNotified(object? sender, List<Student> students)
        {
            Console.WriteLine();
            Console.WriteLine("Testing GroupBy");

            // Query Syntax
            //var studentGroupBy = from s in students
            //                     group s by s.Age into g
            //                     select new
            //                     {
            //                         Age = g.Key,
            //                         Students = g
            //                     };

            // Method Syntax
            var studentGroupBy = students.GroupBy(s => s.Age);

            foreach (var group in studentGroupBy)
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
