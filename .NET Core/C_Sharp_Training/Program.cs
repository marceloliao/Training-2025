using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;

namespace C_Sharp_Training
{
    class Program()
    {
        static int counter;

        static void Main(string[] args)
        {
            //// Implicitly-Typed Variables 
            //var i = 10;
            //Console.WriteLine("Type of i is {0}", i.GetType());

            //var arr = new[] { 1, 10, 20, 30 };
            //Console.WriteLine($"Type of arr is {arr.GetType()}");

            //var file = new FileInfo("MyFile");
            //Console.WriteLine("Type of file is {0}", file.GetType());

            //// string collection
            //IList<string> stringList = new List<string>() {
            //                            "C# Tutorials",
            //                            "VB.NET Tutorials",
            //                            "Learn C++",
            //                            "MVC Tutorials" ,
            //                            "Java"
            //                            };

            //// LINQ Query Syntax
            //var result = from s in stringList
            //             where s.Contains("Tutorials")
            //             select s;

            //Console.WriteLine($"Type of arr is {result.GetType()}");
            //foreach (var item in result)
            //{
            //    Console.WriteLine(item);
            //}

            //// Data Types
            //uint ui = 100u;
            //Console.WriteLine($"Type of ui is {ui.GetType()}");

            float fl = 10.2f;
            Console.WriteLine($"Type of fl is {fl.GetType()}");

            //long l = 45755452222222L;
            //Console.WriteLine($"Type of l is {l.GetType()}"); 

            ulong ul = 45755452222222UL;
            Console.WriteLine($"Type of ul is {ul.GetType()}");

            //double d = 11452222.555D;
            //Console.WriteLine($"Type of d is {d.GetType()}");

            //decimal mon = 1000.15m;
            //Console.WriteLine($"Type of mon is {mon.GetType()}");

            // Default Values
            //int i = default(int); // 0
            //float f = default(float);// 0
            //decimal d = default(decimal);// 0
            //bool b = default(bool);// false
            //char c = default(char);// ''
            //Console.WriteLine($"The default value of i is {i}");
            //Console.WriteLine($"The default value of f is {f}");
            //Console.WriteLine($"The default value of d is {d}");
            //Console.WriteLine($"The default value of b is {b}");
            //Console.WriteLine($"The default value of c is {c}");

            // C# 7.1 onwards
            //int i = default; // 0
            //float f = default;// 0
            //decimal d = default;// 0
            //bool b = default;// false
            //char c = default;// ''
            //Console.WriteLine($"The default value of i is {i}");
            //Console.WriteLine($"The default value of f is {f}");
            //Console.WriteLine($"The default value of d is {d}");
            //Console.WriteLine($"The default value of b is {b}");
            //Console.WriteLine($"The default value of c is {c}");

            //// Implicit Conversion
            //int i = 345;
            //float f = i;

            //Console.WriteLine(f); //output: 345

            //// Explicit Conversion
            //int x = 100;
            //uint y = (uint)x;
            //Console.Write(y);

            //Coordinate point = new Coordinate();
            //Console.WriteLine(point.x); //output: 0  
            //Console.WriteLine(point.y); //output: 0

            //Coordinate point2;
            //point2.x = 10;
            //point2.y = 15;

            //Console.WriteLine(point2.x); //output: 0  
            //Console.WriteLine(point2.y); //output: 0

            //Coordinate point = new Coordinate(10, 25);
            //Console.WriteLine(point.X); //output: 0  
            //Console.WriteLine(point.Y); //output: 0

            //Coordinate point2 = new Coordinate();
            //Console.WriteLine(point2.X); //output: 0  
            //Console.WriteLine(point2.Y); //output: 0

            //point2.CoordinatesChanged += StructEventHandler;
            //point2.X = 30;
            //point2.Y = 50;

            //// String Builder
            //StringBuilder sb = new StringBuilder();
            //sb.Append("Hello ");
            //sb.AppendLine("World!");
            //sb.AppendLine("Hello C#");
            //Console.WriteLine(sb.ToString());

            //StringBuilder sbAmount = new StringBuilder();
            //sbAmount.AppendFormat("{0:C}", 25);
            //Console.WriteLine(sbAmount); //output: Your total amount is $ 25.00

            // Anonymous Type
            var student = new { Id = 1, FirstName = "Steve", LastName = "Brown" };
            Console.WriteLine(student.Id);
            Console.WriteLine(student.FirstName);
            Console.WriteLine(student.LastName);
            Console.WriteLine(student.GetType().ToString());

            // LINQ Query returns an Anonymous Type
            //IList<Student> studentList = new List<Student>()
            //{
            //new Student() { StudentID = 1, StudentName = "John", age = 18 },
            //new Student() { StudentID = 2, StudentName = "Steve",  age = 21 },
            //new Student() { StudentID = 3, StudentName = "Bill",  age = 18 },
            //new Student() { StudentID = 4, StudentName = "Ram" , age = 20  },
            //new Student() { StudentID = 5, StudentName = "Ron" , age = 21 },
            //};

            //var students = from s in studentList
            //               select new { Id = s.StudentID, Name = s.StudentName };
            //foreach(var student in  students)
            //{
            //    Console.WriteLine($"{student.Id} - {student.Name}");
            //}

            // Interface
            Console.WriteLine("Type of Student is {0}", typeof(Student));
            Console.WriteLine("Type of IPerson is {0}", typeof(IPerson));

            // Dynamic Types
            dynamic MyDynamicVar = 1;

            Console.WriteLine(MyDynamicVar.GetType());

            // Static Class, trying to instantiate Calculator which is a static class
            // Calculator calc1 = new Calculator(MyDynamicVar);

            var result = Calculator.Sum(10, 25);
            Calculator.Store(result);

            var calcType = Calculator.Type;
            Calculator.Type = "Scientific";

            // Nullable Type
            Nullable<int> i = null;
            //Console.WriteLine(i.Value); // This will raise an error
            Console.WriteLine(i.GetValueOrDefault()); // This will not raise an error

            // Shorthand Syntax for Nullable
            int? j = null;
            Console.WriteLine(j.GetValueOrDefault());

            int? k = j ?? -1;
            Console.WriteLine(j ?? -5);

            // Accessing static property
            Console.WriteLine($"The value of 'counter' is {counter}");

            // Accessing non-static method
            Display("Show me some text");
        }

        static void StructEventHandler(int point)
        {
            Console.WriteLine($"Coordinate changed to {point}");

        }

        protected static void Display(string text)
        {
            Console.WriteLine(text);
        }
    }

    internal class Student
    {
        public int StudentID { get; set; }
        public string? StudentName { get; set; }
        public int age { get; set; }
    }

    internal struct Coordinate(int x, int y)
    {
        public int X
        {
            get { return x; }
            set
            {
                x = value;
                CoordinatesChanged(x);
            }
        }
        public int Y
        {
            get { return y; }
            set
            {
                y = value;
                CoordinatesChanged(y);
            }
        }

        // Add an event
        public event Action<int> CoordinatesChanged;
    }

    internal interface IPerson
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int Age { get; set; }

        abstract void SayName(string name);
    }

    internal class Employee : IPerson
    {
        string IPerson.FirstName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IPerson.LastName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IPerson.Age { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Sing()
        {
            Console.WriteLine("I am singing");
        }

        void IPerson.SayName(string name)
        {
            Console.WriteLine("I am singing");
        }
    }
}
