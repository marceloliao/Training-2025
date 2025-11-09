using Microsoft.AspNetCore.Mvc;
using StudentApi_AspNetCoreWebAPI.Data;
using StudentApi_AspNetCoreWebAPI.Models;

namespace StudentApi_AspNetCoreWebAPI.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CourseController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("GetAllCourses")]
        public IActionResult GetAllCourses()
        {
            var courses = _dbContext.Courses.ToList();
            return Ok(courses);
        }

        [HttpGet]
        [Route("GetCourse/{id}")]
        public IActionResult Get(int id)
        {
            var existingCourse = _dbContext.Courses.Find(id);
            return (existingCourse is not null) ? Ok(existingCourse) : NotFound();
        }

        [HttpPost]
        [Route("AddCourse")]
        public IActionResult Add([FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check whether a course with same name already exists
            var existingCourse = _dbContext.Courses.FirstOrDefault(c => c.CourseName == course.CourseName);

            if (existingCourse == null)
            {
                _dbContext.Courses.Add(course);
                _dbContext.SaveChanges();
                return Ok(course);
            }
            else
            {
                return BadRequest("Course with same name already exists");
            }
        }
    }
}
