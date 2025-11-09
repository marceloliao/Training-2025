using C__LINQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__LINQ
{
    internal class TestWhere
    {
        //internal static bool CheckAge(Student student, int index)
        //{
        //    if (student.Age > 12 && student.Age < 20 && index % 2 == 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        internal static bool CheckAge(Student student)
        {
            if (student.Age > 12 && student.Age < 20)
            {
                return true;
            }

            return false;
        }

        internal static void ObjectsPopulatedNotified(object? sender, List<Student> students)
        {
            Console.WriteLine("Testing Where");

            Predicate<Student> predicate = CheckAge;
            Func<Student, bool> func = CheckAge;

            var studentsOver18 = from s in students
                                 where func(s)
                                 select s;

            //var studentsOver18 = from s in students
            //                     where s.Age > 12 && s.Age < 20
            //                     select s;

            foreach (var student in studentsOver18)
            {
                Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName + " " + student.LastName}, Age: {student.Age}");
            }

            var studentsOver18Lambda = students.Where(s => s.Age > 12 && s.Age < 20);

            // Overloaded Where method
            //var studentsOver18Lambda = students.Where((s, i) =>
            //{
            //    // The index starts at 0, so we need to check if the index is even
            //    if (s.Age > 12 && s.Age < 20 && i % 2 == 0)
            //        return true;
            //    return false;
            //});

            foreach (var student in studentsOver18Lambda)
            {
                Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName + " " + student.LastName}, Age: {student.Age}");
            }
        }
    }
}
