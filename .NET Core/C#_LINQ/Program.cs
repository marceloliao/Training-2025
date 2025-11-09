using C__LINQ.Data;
using C__LINQ.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using System;
using System.IO;


namespace C__LINQ
{
    internal class Program
    {
        private static List<Student>? _students;
        private static List<Course>? _courses;
        private static List<Teacher>? _teachers;

        static void Main(string[] args)
        {
            //var builder = new HostBuilder()
            //    .ConfigureServices((hostContext, services) =>
            //    {
            //        services.AddDbContext<ApplicationDbContext>();
            //    });

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            using (var context = new ApplicationDbContext(optionsBuilder.Options))
            {
                _students = context.Students.ToList();
                _courses = context.Courses.ToList();
                _teachers = context.Teachers.ToList();

                foreach (var student in _students)
                {
                    Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName + " " + student.LastName}, Age: {student.Age}");
                }

                //foreach (var course in _courses)
                //{
                //    Console.WriteLine($"ID: {course.CourseId}, Name: {course.CourseName}, Teachers: {course.TeacherFirstName + " " + course.TeacherLastName}");
                //}

                //foreach (var teacher in _teachers)
                //{
                //    Console.WriteLine($"ID: {teacher.TeacherId}, Name: {teacher.FirstName + " " + teacher.LastName}");
                //}

                // Instantiate a publisher and raise an event to notify that the data has been loaded
                NotifySubscribers publisher = new NotifySubscribers(new EventArguments {Courses = _courses, Students = _students, Teachers = _teachers });
                //publisher.ObjectsPopulated += ToBeNotified;
                //publisher.ObjectsPopulated += TestWhere.ObjectsPopulatedNotified;
                //publisher.ObjectsPopulated += TestOfType.ObjectsPopulatedNotified;
                //publisher.ObjectsPopulated += TestOrderBy.ObjectsPopulatedNotified;
                //publisher.ObjectsPopulated += TestGroupBy.ObjectsPopulatedNotified;
                //publisher.ObjectsPopulated += TestToLookUp.ObjectsPopulatedNotified;
                //publisher.ObjectsPopulated += TestJoin.ObjectsPopulatedNotified;
                //publisher.ObjectsPopulated += TestGroupJoin.ObjectsPopulatedNotified;
                //publisher.ObjectsPopulated += TestSelect.ObjectsPopulatedNotified;
                //publisher.ObjectsPopulated += TestSelectMany.ObjectsPopulatedNotified;
                //publisher.ObjectsPopulated += TestAllAny.ObjectsPopulatedNotified;
                //publisher.ObjectsPopulated += TestContains.ObjectsPopulatedNotified;
                publisher.ObjectsPopulated += TestAggregate.ObjectsPopulatedNotified;

                publisher.Notify();
            }
        }

        //public static void ToBeNotified(object? sender, List<Student> students)
        //{
        //    Console.WriteLine("Students populated!");

        //    foreach (var student in students)
        //    {
        //        Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName + " " + student.LastName}, Age: {student.Age}");
        //    }
        //}
    }
}


