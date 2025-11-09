namespace C_Sharp_Event
{
    // Step 1: Define a delegate
    public delegate void NotifyEventHandler(object sender, EventArgs e);

    // Step 2: Declare the event
    public class Publisher
    {        
        public event NotifyEventHandler? Notify;

        public void DoSomething()
        {
            Console.WriteLine("Doing something...");
            
            // Step 3: Raise the event
            OnNotify();
        }

        protected virtual void OnNotify()
        {
            if (Notify != null)
            {
                Notify(this, EventArgs.Empty);
            }
        }
    }

    public class Subscriber
    {
        public void OnNotifyReceived(object sender, EventArgs e)
        {
            Console.WriteLine("Subscriber received notification.");
        }
    }
    internal class Program
    {
        // Renamed Main to DoNothing since there can be only one Main
        static void DoNothing(string[] args)
        {
            Publisher publisher = new Publisher();
            Subscriber subscriber = new Subscriber();

            // Step 4: Subscribe to the event
            publisher.Notify += subscriber.OnNotifyReceived;

            publisher.DoSomething();

            // Output:
            // Doing something...
            // Subscriber received notification.
        }
    }
}
