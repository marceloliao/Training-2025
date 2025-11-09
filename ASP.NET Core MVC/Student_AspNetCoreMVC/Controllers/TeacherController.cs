using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Student_AspNetCoreMVC.Data;
using Student_AspNetCoreMVC.Models;

namespace Student_AspNetCoreMVC.Controllers
{
    public class TeacherController : Controller
    {
        private ApplicationDbContext _context;

        public TeacherController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        // GET: TeacherController
        public ActionResult Index()
        {
            return View(_context.Teachers.OrderBy(t=> t.TeacherId));
        }

        // GET: TeacherController/Edit
        public ActionResult DisplayRegistrationForm()
        {
            return View(nameof(Register));
        }

        // POST: TeacherController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Teacher teacher)
        {
            ModelState.Remove("TeacherId"); // Skips validation for StudentId
            
            if (ModelState.IsValid)
            {
                var existingTeacher = _context.Teachers.Find(teacher.TeacherId);

                // If TeacherId is 0, it can only be a new teacher registration, since now TeacherID is hidden in Registrarion form, it would always contain TeacherID when an existing teacher record ie being edited, so the following lines are not needed.
                //if (teacher.TeacherId == 0)
                //{
                //    existingTeacher = _context.Teacher.SingleOrDefault(t => t.FirstName == teacher.FirstName && t.LastName == teacher.LastName);
                //}

                if (existingTeacher == null) // A total new record
                {
                    // Find the last teacher ID and increment by 1
                    int lastID = _context.Teachers.OrderByDescending(t => t.TeacherId).FirstOrDefault()?.TeacherId ?? 0;
                    teacher.TeacherId = lastID + 1;

                    _context.Teachers.Add(teacher);
                }
                else // An existing record to update
                {
                    existingTeacher.FirstName = teacher.FirstName;
                    existingTeacher.LastName = teacher.LastName;                    
                }

                await _context.SaveChangesAsync();
                return View(nameof(Index), _context.Teachers);
            }
            else
                return BadRequest(ModelState);
        }

        // GET: TeacherController/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var existingTeacher = _context.Teachers.SingleOrDefault(t => t.TeacherId == id);

                if (existingTeacher != null)
                {
                    return View("Register", existingTeacher);
                }
            }

            return View(nameof(Register));
        }

        // GET: SudentController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var existingTeacher = _context.Teachers.SingleOrDefault(t => t.TeacherId == id);

                if (existingTeacher != null)
                {
                    _context.Teachers.Remove(existingTeacher);
                    await _context.SaveChangesAsync();
                }
            }

            return View(nameof(Index), _context.Teachers);
        }
    }
}
