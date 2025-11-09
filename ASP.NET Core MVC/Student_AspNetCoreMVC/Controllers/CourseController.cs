using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json.Linq;
using NuGet.DependencyResolver;
using Student_AspNetCoreMVC.Data;
using Student_AspNetCoreMVC.Models;

namespace Student_AspNetCoreMVC.Controllers
{
    public class CourseController : Controller
    {
        private ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseController
        public IActionResult Index()
        {
            return View(_context.Courses);
        }

        // GET: CourseController/DisplayRegistrationForm
        public ActionResult DisplayRegistrationForm()
        {

            // Use ListViewModel to pass on items to the View
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var teacher in _context.Teachers)
            {
                items.Add(new SelectListItem { Value = teacher.TeacherId.ToString(), Text = $"{teacher.FirstName} {teacher.LastName}"});
            }

            var model = new ListViewModel { SelectedItem = "", Items = items, Course = new Course { CourseId = 0, CourseName = "", TeacherFirstName = "Default", TeacherLastName = "Default" } };

            return View("AddCourse", model);

            // Compile a list of Teacher Id + Teacher.FirstName + Teacher.LastName
            //List<SelectListItem> items = new List<SelectListItem>();

            //int counter = 0;
            //foreach (var teacher in _context.Teachers)
            //{
            //    items.Add(new SelectListItem { Value = "counter.ToString()", Text = $"{teacher.TeacherId} - {teacher.FirstName} {teacher.LastName}" });
            //    counter++;
            //}

            //{                
            //    new SelectListItem { Text = "Item1", Value = "1" },
            //    new SelectListItem { Text = "Item2", Value = "2" },
            //    new SelectListItem { Text = "Item3", Value = "3" }
            //};

            // Use ViewBag to pass items to the View
            //ViewBag.Items = items;           

            //return View("AddCourse", new CombinedModels { Course = new Course { CourseId = 0, CourseName = "", TeacherFirstName = "", TeacherLastName = "" }, Teachers = _context.Teachers });
        }

        // POST: CourseController/AddCourse        
        public ActionResult AddCourse(ListViewModel listViewModel)
        {
            ModelState.Remove("Items"); // Skips validation for StudentId

            if (ModelState.IsValid)
            {                
                var selectedTeacher = _context.Teachers.Find(Convert.ToInt32(listViewModel.SelectedItem));

                if (selectedTeacher != null)
                {
                    listViewModel.Course.TeacherFirstName = selectedTeacher.FirstName;
                    listViewModel.Course.TeacherLastName = selectedTeacher.LastName;

                    // Find the last course ID and increment by 1
                    int lastID = _context.Courses.OrderByDescending(c => c.CourseId).FirstOrDefault()?.CourseId ?? 0;
                    //listViewModel.Course.CourseId = null;

                    _context.Courses.Add(listViewModel.Course);
                    _context.SaveChanges();
                    return View("index", _context.Courses);
                }
                else
                { 
                    return View("The selected teacher cannot be found!"); 
                }
            }
            else
                return BadRequest(ModelState);
        }

        // POST: CourseController/EditCourse        
        public async Task<IActionResult> EditCourse(Course course)
        {
            ModelState.Remove("CourseId"); // Skips validation for CourseId

            if (ModelState.IsValid)
            {
                var selectedCourse = _context.Courses.Find(course.CourseId);

                if (selectedCourse != null)
                {
                    selectedCourse.CourseName = course.CourseName;
                    selectedCourse.TeacherFirstName = course.TeacherFirstName;
                    selectedCourse.TeacherLastName = course.TeacherLastName;
                    
                    await _context.SaveChangesAsync();
                    return View("index", _context.Courses);
                }
                else
                {
                    return View("The course cannot be found!");
                }
            }
            else
                return BadRequest(ModelState);
        }

        public IActionResult Edit (int? id)
        {
            if (id.HasValue)
            {
                var existingCourse = _context.Courses.SingleOrDefault(c => c.CourseId == id);

                // Compile a list of Teacher Id + Teacher.FirstName + Teacher.LastName
                List<SelectListItem> items = new List<SelectListItem>();
                List<SelectListItem> firstNameItems = new List<SelectListItem>();
                List<SelectListItem> lastNameItems = new List<SelectListItem>();

                foreach (var teacher in _context.Teachers)
                {
                    items.Add(new SelectListItem { Value = teacher.TeacherId.ToString(), Text = $"{teacher.FirstName} {teacher.LastName}" });
                    firstNameItems.Add(new SelectListItem { Value = teacher.TeacherId.ToString(), Text = $"{teacher.FirstName}" }); 
                    lastNameItems.Add(new SelectListItem { Value = teacher.TeacherId.ToString(), Text = $"{teacher.LastName}" });                    
                }

                // Use ViewBag to pass items to the View
                ViewBag.Items = items;
                ViewBag.FirstNameItems = firstNameItems;
                ViewBag.LastNameItems = lastNameItems;
                ViewBag.CurrentlySelected = id;                

                //return View("AddCourse", new CombinedModels { Course = new Course { CourseId = 0, CourseName = "", TeacherFirstName = "", TeacherLastName = "" }, Teachers = _context.Teachers });

                if (existingCourse != null)
                {
                    return View(existingCourse);
                }
            }

            return View();
        }
        

        // GET: CourseController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var existingCourse = _context.Courses.SingleOrDefault(c => c.CourseId == id);

                if (existingCourse != null)
                {
                    _context.Courses.Remove(existingCourse);
                    await _context.SaveChangesAsync();                    
                }
            }

            return View(nameof(Index), _context.Courses.OrderBy(c=> c.CourseId));
        }
    }
}
