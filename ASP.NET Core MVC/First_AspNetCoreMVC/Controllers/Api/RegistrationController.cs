using First_AspNetCoreMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace First_AspNetCoreMVC.Controllers.Api
{
    [Route("api/[controller]")]
    public class RegistrationController : Controller
    {
        private readonly DemoContext _dbContext;

        public RegistrationController(DemoContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Retrieve()
        {
            //var fake = new FakeData();

            //return Ok(fake.GetRegistrations());


            return Ok(_dbContext.Registrations);
        }

        [HttpGet("byCount")]
        public IActionResult Retrieve([FromQuery] int count)
        {
            //var fake = new FakeData();

            //return Ok(fake.GetRegistrations().Take(count));
            return Ok(_dbContext.Registrations.Take(count));
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var reg = _dbContext.Registrations.FirstOrDefault(m => m.Id == Id);
            if (reg != null)
            {
                _dbContext.Registrations.Remove(reg);
                _dbContext.SaveChanges();
                return Ok($"The item {Id} was successfully deleted.");
            }
            else
                return NotFound("The item was not found.");
        }
    }
}
