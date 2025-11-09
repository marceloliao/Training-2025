using C__LINQ.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__LINQ
{
    internal class TestOrderBy
    {
        internal static void ObjectsPopulatedNotified(object? sender, List<Student> students)
        {
            Console.WriteLine();
            Console.WriteLine("Testing OrderBy");

            // Query Syntax, with multiple sorting, multiple sorting is only available in Query Syntax 
            //var studentOrderBy = from s in students
            //                     orderby s.Age, s.FirstName
            //                     select s;

            // Method Syntax
            var studentOrderBy = students.OrderByDescending(s => s.Age).ThenBy(s=>s.LastName);

            foreach (var student in studentOrderBy)
            {
                Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName + " " + student.LastName}, Age: {student.Age}");
            }

            //var studentOrderByDescending = from s in students
            //                               orderby s.Age descending
            //                               select s;

            //var studentOrderByDescending = students.OrderByDescending(s => s.Age);

            //foreach (var student in studentOrderByDescending)
            //{
            //    Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName + " " + student.LastName}, Age: {student.Age}");
            //}
        }
    }
}
