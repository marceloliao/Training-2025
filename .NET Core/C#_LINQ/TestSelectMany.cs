using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C__LINQ.Models;

namespace C__LINQ
{
    internal class TestSelectMany
    {
        internal static void ObjectsPopulatedNotified(Object? sender, EventArguments args)
        {
            Console.WriteLine();
            Console.WriteLine("Testing Select Many");

            // Define a list of students  
            List<MedicalStudent> students = new List<MedicalStudent> {
                   new MedicalStudent { FirstName = "Alice", Subjects = new List<string>{"Math", "Science", "Cinema" } },
                   new MedicalStudent { FirstName = "Bob", Subjects = new List<string>{"History", "Geography" } },
                   new MedicalStudent { FirstName = "Charlie", Subjects = new List<string>{"Math", "Art" } }
               };

            // Using query syntax to achieve SelectMany  
            var allSubjects = from student in students
                              from subject in student.Subjects ?? Enumerable.Empty<string>()
                              select subject;

            // Method Syntax  
            var allSubjects2 = students.SelectMany(s => s.Subjects ?? Enumerable.Empty<string>());

            foreach (var subject in allSubjects)
            {
                Console.WriteLine($"{subject}");
            }

            foreach (var subject in allSubjects2)
            {
                Console.WriteLine($"{subject}");
            }
        }
    }
}
