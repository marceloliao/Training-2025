using Microsoft.AspNetCore.Mvc;
using StudentApi_AspNetCoreWebAPI.Data;
using StudentApi_AspNetCoreWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentApi_AspNetCoreWebAPI.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public TeacherController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Get all teachers
        [HttpGet]
        [Route("GetAllTeachers")]
        public IActionResult GetAllTeachers()
        {
            return Ok(_dbContext.Teachers.ToList());
        }

        // POST: Add a new teacher
        [HttpPost]
        [Route("AddTeacher")]
        public IActionResult AddTeacher([FromBody] Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check whether a teacher with same name already exists
            var existingTeacher = _dbContext.Teachers.FirstOrDefault(t => t.FirstName == teacher.FirstName && t.LastName == teacher.LastName);
            if (existingTeacher == null)
            {
                _dbContext.Teachers.Add(teacher);
                _dbContext.SaveChanges();
                return Ok(teacher);
            }
            else
            {
                return BadRequest("Teacher with same name already exists");
            }
        }

        // Get a teacher by Id
        //[HttpGet]
        //[Route("GetATeacher/{id}")]
        //public IActionResult GetATeacher(int id)
        //{
        //    // Check whether the teacher exists
        //    var existingTeacher = _dbContext.Teachers.Find(id);

        //    if (existingTeacher == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        updatedTeacher.TeacherId = existingTeacher.TeacherId;
        //        updatedTeacher.TeacherId = existingTeacher.TeacherId;
        //        updatedTeacher.TeacherId = existingTeacher.TeacherId;
        //    }


        //}

        // PUT: Update a teacher
        [HttpPut]
        [Route("UpdateTeacher/{id}")]
        public IActionResult UpdateTeacher(int id, [FromBody] Teacher updatedTeacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check whether the teacher exists
            var existingTeacher = _dbContext.Teachers.Find(id);
            
            if (existingTeacher == null)
            {
                return NotFound();
            }            

            existingTeacher.TeacherId = updatedTeacher.TeacherId;
            existingTeacher.FirstName = updatedTeacher.FirstName;
            existingTeacher.LastName = updatedTeacher.LastName;
            _dbContext.SaveChanges();
            return Ok(existingTeacher);
        }

        // DELETE: Delete a teacher
        [HttpDelete]
        [Route("DeleteTeacher/{id}")]
        public IActionResult DeleteTeacher(int id)
        {
            var teacher = _dbContext.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound();
            }
            _dbContext.Teachers.Remove(teacher);
            _dbContext.SaveChanges();
            return Ok(teacher);
        }
    }
}
