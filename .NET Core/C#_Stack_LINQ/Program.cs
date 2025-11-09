using System.Collections;
using System.Collections.Generic;

namespace C__Stack_LINQ
{
    delegate bool FindStudent(Student student);

    internal class Program
    {
        static void Main(string[] args)
        {
            Student[] students = new Student[]
            {
                new Student() { StudentID = 1, StudentName = "John", Age = 18 },
                new Student() { StudentID = 2, StudentName = "Steve", Age = 20 },
                new Student() { StudentID = 3, StudentName = "Bill", Age = 22 },
                new Student() { StudentID = 4, StudentName = "Ram", Age = 19 },
                new Student() { StudentID = 5, StudentName = "Ron", Age = 18 },
                new Student() { StudentID = 6, StudentName = "Chris", Age = 30 },
                new Student() { StudentID = 7, StudentName = "Rob", Age = 22 },
                new Student() { StudentID = 8, StudentName = "Sara", Age = 25 }
            };

            Student[] result = StudentExtension.where(students, delegate (Student student)
            {
                return student.Age > 25;
            });

            foreach (var student in result)
            {
                Console.WriteLine($"StudentID: {student.StudentID}, StudentName: {student.StudentName}, Age: {student.Age}");
            }

            // Use Lambda Expression
            Student[] result2 = StudentExtension.where(students, (student) => student.Age > 25);

            Student[] result3 = students.Where(s => s.Age > 20).ToArray();
            foreach (var student in result3)
            {
                Console.WriteLine($"StudentID: {student.StudentID}, StudentName: {student.StudentName}, Age: {student.Age}");
            }

            Student? result4 = students.Where(s => s.StudentName == "Chris").FirstOrDefault();
            if (result4 != null)
            {
                Console.WriteLine($"StudentID: {result4.StudentID}, StudentName: {result4.StudentName}, Age: {result4.Age}");
            }
            else
            {
                Console.WriteLine("Student not found");
            }

            // Use LINQ Query Syntax
            Console.WriteLine();
            Console.WriteLine("Using LINQ Query Syntax:");
            var result5 = from s in students
                          where s.Age > 20
                          select s;

            foreach (var student in result5)
                Console.WriteLine($"StudentID: {student.StudentID}, StudentName: {student.StudentName}, Age: {student.Age}");

            // Use LINQ Method Syntax
            Console.WriteLine();            
            Console.WriteLine("Using LINQ Method Syntax:");
            var result6 = students.Where(s => s.Age > 22).ToList<Student>();

            foreach (var student in result6)
                Console.WriteLine($"StudentID: {student.StudentID}, StudentName: {student.StudentName}, Age: {student.Age}");

            // Use func
            Console.WriteLine();            
            Console.WriteLine("Result 7. Using func and assign func at Where using Method Syntax");
            Func<Student, bool> isStudentOver22 = (student) => student.Age > 22;

            var result7 = students.Where(isStudentOver22).ToList<Student>();
            
            foreach (var student in result7)
                Console.WriteLine($"StudentID: {student.StudentID}, StudentName: {student.StudentName}, Age: {student.Age}");

            // Use func
            Console.WriteLine();
            Console.WriteLine("Result 8. Using func and use func at Where using Query Syntax");

            var result8 = from s in students
                          where isStudentOver22(s)
                          select s;

            foreach (var student in result8)
                Console.WriteLine($"StudentID: {student.StudentID}, StudentName: {student.StudentName}, Age: {student.Age}");

            // Where, a second overload method that providea index
            Console.WriteLine();
            Console.WriteLine("Result 9. Using \"where\" but with a second \"index\" parameter");

            // Convert the entire students list to dictionary with indexes
            Dictionary<int, Student> studentsDictionary = students
                .Select((student, index) => new { index, student })
                .ToDictionary(item => item.index, item => item.student);

            // Print the dictionary
            foreach (var kvp in studentsDictionary)
            {
                Console.WriteLine($"Index: {kvp.Key}, Value: StudentID: {kvp.Value.StudentID}, StudentName: {kvp.Value.StudentName}, Age: {kvp.Value.Age}");
            }
            
            // Run LINQ on a dictionary
            var result9 = studentsDictionary.Where(kvp => kvp.Value.Age > 15 && kvp.Key % 2 == 0).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // Print the resulted dictionary
            foreach (var kvp in result9)
            {
                Console.WriteLine($"Index: {kvp.Key}, Value: StudentID: {kvp.Value.StudentID}, StudentName: {kvp.Value.StudentName}, Age: {kvp.Value.Age}");
            }


        }
    }
}


