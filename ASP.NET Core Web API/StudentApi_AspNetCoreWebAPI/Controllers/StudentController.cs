using Microsoft.AspNetCore.Mvc;
using StudentApi_AspNetCoreWebAPI.Data;
using StudentApi_AspNetCoreWebAPI.Models;

namespace StudentApi_AspNetCoreWebAPI.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Get all students
        [HttpGet]
        [Route("GetAllStudents")]
        public IActionResult GetAllStudents()
        {
            var students = _dbContext.Students.ToList();
            return Ok(students);
        }

        // POST: Add a new student
        [HttpPost]
        [Route("AddStudent")]
        public IActionResult AddStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check whether a student with same name already exists
            var existingStudent = _dbContext.Students.FirstOrDefault(s => s.FirstName == student.FirstName && s.LastName == student.LastName);

            if (existingStudent == null)
            {
            _dbContext.Students.Add(student);
            _dbContext.SaveChanges();
            return Ok(student);
            }
            else
            {
                return BadRequest("Student with same name already exists");
            }
        }

        [HttpPut]
        [Route("UpdateStudent/{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(id != student.StudentId)
            {
                return BadRequest("The Id does not match");
            }

            var existingStudent = _dbContext.Students.Find(student.StudentId);
            
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Age = student.Age;
            _dbContext.SaveChanges();
            return Ok(existingStudent);
        }
    }
}
