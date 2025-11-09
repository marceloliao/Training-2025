namespace C__Stack_LINQ
{
    static class StudentExtension
    {
        public static Student[] where(this Student[] students, FindStudent del)
        {
            List<Student> result = new List<Student>();

            foreach (var student in students)
            {
                if (del(student))
                {
                    result.Add(student);
                }
            }
            return result.ToArray();
        }
    }
}


