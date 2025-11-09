using C__LINQ.Models;

namespace C__LINQ
{
    // Declare a class to hold more than 1 event arguments, by default only can be passed in the default EventHandler
    public class EventArguments : EventArgs
    {
        public List<Student> Students { get; set; } = null!;
        public List<Teacher> Teachers { get; set; } = null!;
        public List<Course> Courses { get; set; } = null!;
    }

    public class NotifySubscribers
    {
        // Declaring an event using the built-in EventHandler, this way we don't need to declare a delegate
        public event EventHandler<EventArguments>? ObjectsPopulated;
        
        private List<Student> _students;
        private List<Teacher> _teachers;
        private List<Course> _courses;

        public NotifySubscribers(EventArguments args)
        {
            this._students = args.Students;
            this._teachers = args.Teachers;
            this._courses = args.Courses;
        }

        public void Notify()
        {
            // Call the method to notify
            OnObjectsPopulated(new EventArguments {Courses = this._courses, Students = this._students, Teachers = this._teachers });
        }

        protected virtual void OnObjectsPopulated(EventArguments args)
        {
            // The ?. operator checks if the left-hand side (in this case, ObjectsPopulated) is null.
            // If it is not null, it proceeds to invoke the method or access the member.
            // If it is null, it simply returns null without throwing a NullReferenceException.
            ObjectsPopulated?.Invoke(this, args);
        }
    }    
}


