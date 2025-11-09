using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_AspNetCoreMVC.Data;
using Student_AspNetCoreMVC.Models;

namespace Student_AspNetCoreMVC.Controllers
{
    public class StudentController : Controller
    {
        private ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StudentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: StudentController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: StudentController/Edit
        public ActionResult DisplayRegistrationForm()
        {
            return View(nameof(Register));
        }

        // POST: StudentController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Student student)
        {
            ModelState.Remove("StudentId"); // Skips validation for StudentId
            if (ModelState.IsValid)
            {
                var existingStudent = _context.Students.Find(student.StudentId);

                if (student.StudentId == 0)
                {
                    // If StudentId is 0, it might be a new student registration or an edit on an existing student, then we look for one that has the same first name, last name and age.
                    existingStudent = _context.Students.SingleOrDefault(s => s.FirstName == student.FirstName && s.LastName == student.LastName && s.Age == student.Age);
                }

                if (existingStudent == null) // A total new record
                {
                    // Find the last student ID and increment by 1
                    int lastID = _context.Students.OrderByDescending(s => s.StudentId).FirstOrDefault()?.StudentId ?? 0;
                    student.StudentId = lastID + 1;

                    _context.Students.Add(student);
                }
                else // An existing record to update
                {
                    existingStudent.FirstName = student.FirstName;
                    existingStudent.LastName = student.LastName;
                    existingStudent.Age = student.Age;
                }

                await _context.SaveChangesAsync();
                return View(nameof(Index));
            }
            else
                return BadRequest(ModelState);
        }

        // GET: StudentController/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var existingStudent = _context.Students.SingleOrDefault(s => s.StudentId == id);

                if (existingStudent != null)
                {
                    return View("Register", existingStudent);
                }
            }

            return View(nameof(Register));
        }

        // POST: StudentController/Edit/5
        
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: StudentController/Delete/5        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var existingStudent = _context.Students.SingleOrDefault(s => s.StudentId == id);

                if (existingStudent != null)
                {
                    _context.Students.Remove(existingStudent);
                    await _context.SaveChangesAsync();
                }
            }

            return View(nameof(Index));
        }

        // POST: StudentController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
