using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__LINQ
{
    internal class TestJoin
    {
        internal static void ObjectsPopulatedNotified(object? sender, EventArguments args)
        {
            Console.WriteLine();
            Console.WriteLine("Testing Join");

            // Inner Join Query Syntax  
            var courseTeachers = from c in args.Courses
                                 join t in args.Teachers on
                                 new { FirstName = c.TeacherFirstName, LastName = c.TeacherLastName }
                                 equals new { t.FirstName, t.LastName }
                                 select new { c.CourseName, c.TeacherFirstName, c.TeacherLastName };

            // Method Syntax
            //var courseTeachers = args.Teachers.Join(args.Courses,
            //        teacher => new { FirstName = teacher.FirstName, LastName = teacher.LastName },
            //        course => new { FirstName = course.TeacherFirstName, LastName = course.TeacherLastName },
            //        (teacher, course) => new
            //        {
            //            CourseName = course.CourseName,
            //            TeacherFirstName = teacher.FirstName,
            //            TeacherLastName = teacher.LastName
            //        });

            foreach (var course in courseTeachers)
            {
                Console.WriteLine($"Course Name: {course.CourseName} | Course Teacher: {course.TeacherFirstName + " " + course.TeacherLastName}");
            }
        }
    }
}
