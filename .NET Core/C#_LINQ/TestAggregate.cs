using C__LINQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__LINQ
{
    internal class TestAggregate
    {
        internal static void ObjectsPopulatedNotified(Object? sender, EventArguments args)
        {
            Console.WriteLine();
            Console.WriteLine("Testing Aggregate");

            // Aggreagte() Overloads 
            // public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func);
            // public static TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func);
            // public static TResult Aggregate<TSource, TAccumulate, TResult>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector);

            // Using Aggregate to concatenate all student names
            string? allStudentNames = args.Students.Select(s => s.FirstName + " " + s.LastName).Aggregate((n1, n2) => n1 + ", " + n2);
            Console.WriteLine($"All student names: {allStudentNames}");

            // Using Aggregate to calculate the total age of all students
            int? totalAge = args.Students.Select(s => s.Age).Aggregate((a1, a2) => a1 + a2);
            Console.WriteLine($"Total age of all students: {totalAge}");

            // Using Aggregate with a seed value to calculate the total age of all students
            int? totalAgeWithSeed = args.Students.Select(s => s.Age).Aggregate(50, (a1, a2) => a1 + a2 ?? 0);
            Console.WriteLine($"Total age with seed of all students: {totalAgeWithSeed}");

            string? commaSeparatedStudentNames = args.Students.Aggregate<Student, string>(
                                        "Student Names: ",  // Seed Value
                                        (str, s) => str += s.FirstName + ",");

            //string? commaSeparatedStudentNames = args.Students.Select(s => s.FirstName).Aggregate("Student Names: ", (n1, n2) => n1 + ", " + n2);
            Console.WriteLine($"{commaSeparatedStudentNames}");

        }
    }
}
