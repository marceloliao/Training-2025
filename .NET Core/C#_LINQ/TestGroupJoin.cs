using C__LINQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__LINQ
{
    internal class TestGroupJoin
    {
        internal static void ObjectsPopulatedNotified(Object? sender, EventArguments args)
        {
            Console.WriteLine();
            Console.WriteLine("Testing GroupJoin");

            IList<MedicalStudent> studentList = new List<MedicalStudent>() {
                new MedicalStudent() { StudentId = 1, FirstName = "John", ClassId =1 },
                new MedicalStudent() { StudentId = 2, FirstName = "Moin", ClassId =1 },
                new MedicalStudent() { StudentId = 3, FirstName = "Bill", ClassId =2 },
                new MedicalStudent() { StudentId = 4, FirstName = "Ram",  ClassId =2 },
                new MedicalStudent() { StudentId = 5, FirstName = "Ron", ClassId =2 }
            };

            IList<Class> classList = new List<Class>() {
                new Class(){ ClassId = 1, ClassName="Class 301"},
                new Class(){ ClassId = 2, ClassName="Class 302"},
                new Class(){ ClassId = 3, ClassName="Class 303"}
            };

            // Method Syntax
            var classJoin = classList.GroupJoin(studentList,
                        cls => cls.ClassId,
                        s => s.ClassId,
                        (cls, studentGroup) => new
                        {
                            Students = studentGroup,
                            ClassFullName = cls.ClassName
                        });

            // Query Syntax
            var classJoin2 = from cls in classList
                             join s in studentList
                             on cls.ClassId equals s.ClassId
                             into studentGroup
                             select new
                             {
                                 Students = studentGroup,
                                 ClassFullName = cls.ClassName
                             };


            foreach(var item in classJoin)
            {
                Console.WriteLine(item.ClassFullName);
                foreach (var student in item.Students)
                    Console.WriteLine(student.FirstName);
            }

            foreach (var item in classJoin2)
            {
                Console.WriteLine(item.ClassFullName);
                foreach (var student in item.Students)
                    Console.WriteLine(student.FirstName);
            }
        }
    }
}
