using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Student_AspNetCoreMVC.Configurations;
using Student_AspNetCoreMVC.Data;
using Student_AspNetCoreMVC.Models;

namespace Student_AspNetCoreMVC.Components
{
    public class StudentListViewComponent: ViewComponent
    {
        private ApplicationDbContext _context;
        private IOptions<StudentListConfig> _studentListConfig;
        private ILogger<StudentListViewComponent> _logger;

        public StudentListViewComponent(ApplicationDbContext context, IOptions<StudentListConfig> studentListConfig, ILogger<StudentListViewComponent> logger)
        {
            _context = context;
            _studentListConfig = studentListConfig;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            _logger.LogInformation($"Invoking {nameof(StudentListViewComponent)}");

            var students = await _context.Students.ToListAsync();

            _logger.LogInformation($"Existing InvokeAsync of {nameof(StudentListViewComponent)}");
            
            //var pageSize = _regConfig.GetValue<int?>("RegistrationList:PageSize")?? 25;
            var pageSize = _studentListConfig.Value.PageSize;

            //return View(students.Take(pageSize));

            return View(students);
        }
    }
}
