using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C__LINQ.Models
{
    internal class MedicalStudent: Student
    {
        public int? ClassId { get; set; } = null!;

        public List<String>? Subjects { get; set; } = null!;
    }
}
