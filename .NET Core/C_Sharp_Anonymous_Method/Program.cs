namespace C_Sharp_Anonymous_Method
{
    public delegate void Print(int value);
    internal class Program
    {
        static void Main(string[] args)
        {
            Print print1 = delegate (int value)
            {
                Console.WriteLine("This is an anonymous method. Value {0}", value);
            };

            print1(78);

            int i = 10;

            // The variable inside anonymous method has access to external variables
            Print print2 = delegate (int value)
            {
                value += i; 
                Console.WriteLine("This is an anonymous method. Value {0}", value);
            };

            print2(47);
        }
    }
}
