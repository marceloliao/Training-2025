namespace C_Sharp_Generic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataStore<string> cities = new DataStore<string>();
            cities.AddOrUpdate(0, "Mumbai");
            cities.AddOrUpdate(1, "Chicago");
            cities.AddOrUpdate(2, "London");

            DataStore<int> empIds = new DataStore<int>();
            empIds.AddOrUpdate(0, 50);
            empIds.AddOrUpdate(1, 65);
            empIds.AddOrUpdate(2, 89);

            DataStore2<int> classes = new DataStore2<int>();
        }

        internal class DataStore<T>
        {
            // Generic field
            private readonly T? data1;

            // Generic array
            public T[] data2 = new T[10];

            public T? Data3 { get; set; }

            // Generic method
            public void AddOrUpdate(int index, T item)
            {
                if (index >= 0 && index < 10)
                    data2[index] = item;
            }

            public T? GetData(int index)
            {
                if (index >= 0 && index < 10)
                    return data2[index];
                else
                    return default;
            }
        }

        // Generic Constraints
        // where T : class, this means it only accepts reference types, such as class, interface, delegate or array
        // where T : struct, this means it only accepts non-nullable value types, such as int, bool, float
        //internal class DataStore2<T> where T : class

        internal class DataStore2<T> where T : struct
        {
            // Generic field
            private readonly T? data1;

            // Generic array
            public T[] data2 = new T[10];

            public T? Data3 { get; set; }

            // Generic method
            public void AddOrUpdate(int index, T item)
            {
                if (index >= 0 && index < 10)
                    data2[index] = item;
            }

            public T? GetData(int index)
            {
                if (index >= 0 && index < 10)
                    return data2[index];
                else
                    return default;
            }

        }
    }
}
