namespace C__Stack_Queue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //// Creating a stack, which is FILO, LIFO
            //Stack<int> stack2 = new Stack<int>();
            //stack2.Push(1);
            //stack2.Push(2);
            //stack2.Push(3);
            //stack2.Push(4);

            //foreach (var item in stack2)
            //{
            //    Console.Write(item + ", ");
            //}

            //Console.WriteLine();

            //// Create a stack from an array
            //int[] ints = stack2.ToArray();

            //Stack<int> stack1 = new Stack<int>(ints);
            //foreach (var item in stack1)
            //{
            //    Console.Write(item + ", ");
            //}

            //Console.WriteLine("Poping out the last item: {0}", stack1.Pop());
            //foreach (var item in stack1)
            //{
            //    Console.Write(item + ", ");
            //}

            Stack<int> stack3 = new Stack<int>();
            stack3.Push(1);
            stack3.Push(2);
            stack3.Push(3);
            stack3.Push(4);

            Console.Write("Number of elements in Stack: {0}", stack3.Count);

            while (stack3.Count > 0)
                Console.Write(stack3.Pop() + ",");

            Console.Write("Number of elements in Stack: {0}", stack3.Count);
        }
    }
}
