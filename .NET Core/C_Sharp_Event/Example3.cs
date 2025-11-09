using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_Event
{
    // The following class is the publisher
    public class ProcessNewBusinessLogic
    {
        // Declaring an event using the built-in EventHandler, this way we don't need to declare a delegate
        public event EventHandler? ProcessCompleted;

        public void StartProcessing()
        {
            Console.WriteLine("Process started!");
            Task.Delay(2000).Wait();
            Console.WriteLine("Process completed!");

            // Call the method to notify
            OnProcessCompleted(EventArgs.Empty);

        }

        protected virtual void OnProcessCompleted(EventArgs e)
        {
            //The?. operator checks if the left - hand side(in this case, ProcessCompleted) is null.
            //If it is not null, it proceeds to invoke the method or access the member.
            //If it is null, it simply returns null without throwing a NullReferenceException.
            ProcessCompleted?.Invoke(this, e);
        }

    }
    class Example3
    {
        static void Main(string[] args)
        {
            ProcessNewBusinessLogic b1 = new ProcessNewBusinessLogic();
            b1.ProcessCompleted += ToBeNotified;
            b1.StartProcessing();
        }

        public static void ToBeNotified(object? sender, EventArgs e)
        {
            Console.WriteLine("Notification received!");
        }
    }
}
