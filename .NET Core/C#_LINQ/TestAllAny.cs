using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__LINQ
{
    internal class TestAllAny
    {
        internal static void ObjectsPopulatedNotified(Object? sender, EventArguments args)
        {
            Console.WriteLine();
            Console.WriteLine("Testing All");

            // Method Syntax
            bool areAllStudentTeenagers = args.Students.All(s => s.Age > 12 && s.Age < 19);
            Console.WriteLine($"All students are teens? {areAllStudentTeenagers}");

            bool anyStudentTeenager = args.Students.Any(s => s.Age > 12 && s.Age < 19);
            Console.WriteLine($"Any teen student? {anyStudentTeenager}");
        }
    }
}
