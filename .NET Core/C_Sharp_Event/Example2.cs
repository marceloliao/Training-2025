using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_Event
{
    // Declare a delegate
    public delegate void Notify();

    // The following class is the publisher
    public class ProcessBusinessLogic
    {
        public event Notify? ProcessCompleted;

        public void StartProcessing()
        {
            Console.WriteLine("Process started!");
            Task.Delay(2000).Wait();
            Console.WriteLine("Process completed!");

            // Call the method to notify
            OnProcessCompleted();

        }

        protected virtual void OnProcessCompleted()
        {
            //The?. operator checks if the left - hand side(in this case, ProcessCompleted) is null.
            //If it is not null, it proceeds to invoke the method or access the member.
            //If it is null, it simply returns null without throwing a NullReferenceException.
            ProcessCompleted?.Invoke();
        }

    }

    // Example2 class is a subscriber of the event
    class Example2
    {
        static void DoNothing(string[] args)
        {
            ProcessBusinessLogic b1 = new ProcessBusinessLogic();
            b1.ProcessCompleted += ToBeNotified;
            b1.StartProcessing();            
        }

        public static void ToBeNotified()
        { 
            Console.WriteLine("Notification received!");
        }

    }
}
