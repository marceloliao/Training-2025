using C__LINQ.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__LINQ
{
    internal class TestOfType
    {
        internal static void ObjectsPopulatedNotified(object? sender, List<Student> students)
        {
            Console.WriteLine();
            Console.WriteLine("Testing OfType");

            var studentsOver18Lambda = students.Where((s, i) =>
            {
                // The index starts at 0, so we need to check if the index is even
                if (s.Age > 12 && s.Age < 20 && i % 2 == 0)
                    return true;
                return false;
            });

            ArrayList mixedList = new ArrayList();
            mixedList.Add(0);
            mixedList.Add("One");
            mixedList.Add("Two");
            mixedList.Add(3);
            mixedList.AddRange(studentsOver18Lambda.ToList());
                        
            foreach (var item in mixedList)
            {
                Console.WriteLine($"Item: {item}");
            }

            //var studentsOfType = from s in mixedList.OfType<Student>()
            //                     select s;

            var studentsOfType = mixedList.OfType<Student>();

            foreach (var student in studentsOfType)
            {
                Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName + " " + student.LastName}, Age: {student.Age}");
            }
        }
    }
}
