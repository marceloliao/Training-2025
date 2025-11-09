using System.Reflection;
using System.Reflection.Metadata;

namespace C_Sharp_Reflection
{
    internal class Program
    {
        // Reflection in C# is a powerful feature that allows you to inspect and interact with the metadata of
        // assemblies, types, and members at runtime. It enables you to dynamically create instances of types,
        // invoke methods, access fields and properties, and more. Here are some key uses of reflection:

        // 1. Inspecting Metadata:
        // You can examine the structure of assemblies, modules, types, and members.
        // This includes information like method names, parameter types, access modifiers, and attributes.
        
        // 2. Dynamic Invocation:
        // You can dynamically invoke methods, access fields, and properties without knowing their names at compile time.
        // This is useful for scenarios like plugin systems or serialization.
        
        // 3 Creating Instances:
        // You can create instances of types dynamically using reflection, which is helpful for factories or
        // dependency injection.
        static void Main(string[] args)
        {
            // Get the type information
            Type unknownType = typeof(Unknown);

            // Create an instance of the type
            object? unknownInstance = Activator.CreateInstance(unknownType);

            // Set the property value using reflection
            PropertyInfo? numberProperty = unknownType.GetProperty("Number");
            numberProperty?.SetValue(unknownInstance, 42);

            // Invoke the method using reflection
            MethodInfo? printMethod = unknownType?.GetMethod("PrintNumber");
            printMethod?.Invoke(unknownInstance, null);
        }
    }

    public class Unknown
    {
        public int Number { get; set; }

        public void PrintNumber()
        {
            Console.WriteLine($"Number: {Number}");  
        }
    }
}
