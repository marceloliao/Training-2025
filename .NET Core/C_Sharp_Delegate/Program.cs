using System.IO;

namespace C_Sharp_Delegate
{
    // Declare a delegate
    public delegate void MyDelegate(string msg);

    // Generic delegate
    public delegate T Sum<T>(T param1, T param2);

    public class Program
    {
        static void Main(string[] args)
        {
            MyDelegate del1 = new MyDelegate(ClassA.MethodA);
            del1.Invoke("Hello Marcelo");

            // You can set the target method by assigning a method directly without creating an object of
            // delegate e.g., MyDelegate del = MethodA.
            MyDelegate del2 = ClassB.MethodB;
            del2("Hello Aidan");

            // Or set lambda expression 
            MyDelegate del3 = (string msg) => Console.WriteLine("Called lambda expression: " + msg);
            del3("Hello Elizabeth");

            // Pass a delegate as parameter
            MyDelegate del4 = ClassA.MethodA;
            InvokeDelegate(del4);

            // Multicast a delegate
            MyDelegate? del5 = del1 + del2; // Combine del1 + del2
            del5("Hello YaWen");

            del5 += del3; // Combines del1 + del2 + del3
            del5("Hello Minh");

            del5 = del5 - del2; // Removes del2
            del5("Hello Anabelle");

            del5 -= del1; // Removes del1
            del5("Hello World");

            // Generic delegate
            Sum<int> addition = AddTowNumbers;
            Console.WriteLine(addition(3, 8));

            Sum<string> concactenation = ConcactenateTwoStrings;
            Console.WriteLine(concactenation("Hello ", "Leanna"));

            // Func delegate, a built-in generic delegate
            Func<int, int, string> addition2 = AddTowNumbersButReturnString;
            Console.WriteLine("Using Func - {0}", addition2(-75, 45));

            // Action delegate, similar to Func except it doesn't return any value, as if it was void
            // with anonymous function
            Action<int> printActionDel1 = delegate (int x)
                                        {
                                            Console.WriteLine(x);
                                        };

            printActionDel1(145);

            // with lambda expression
            Action<string> printActionDel2 = input => Console.WriteLine(input);
            printActionDel2("Hello Sergei");

            // Predicate delegate
            Predicate<string> isUpper = IsUpperCase;
            bool result = isUpper("HELLO WORLD");
            Console.WriteLine($"Predicate result: {result}");
        }

        // To be used with Predicate delegate
        public static bool IsUpperCase(string input)
        {
            return input == (input.ToUpper());
        }

        public static int AddTowNumbers(int val1, int val2)
        {
            return val1 + val2;
        }

        public static string AddTowNumbersButReturnString(int val1, int val2)
        {
            return (val1 + val2).ToString();
        }

        public static string ConcactenateTwoStrings(string str1, string str2)
        {
            return str1 + str2;
        }

        static void InvokeDelegate(MyDelegate del)
        {
            del("Passing a delegate as parameter");
        }

        internal class ClassA
        {
            public static void MethodA(string message)
            {
                Console.WriteLine("Called ClassA.MethodA() with parameter: " + message);
            }
        }
        internal class ClassB
        {
            public static void MethodB(string message)
            {
                Console.WriteLine("Called ClassB.MethodB() with parameter: " + message);
            }
        }
    }
}
