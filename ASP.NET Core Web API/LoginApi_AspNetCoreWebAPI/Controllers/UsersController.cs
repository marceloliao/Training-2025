using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoginApi_AspNetCoreWebAPI.Data;
using LoginApi_AspNetCoreWebAPI.Models;

namespace LoginApi_AspNetCoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly LoginApiDbContext _dbContext;

        public UsersController(LoginApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Register(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check whether a user with the same email already exists
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == userDTO.Email);

            if (existingUser != null)
            {
                return BadRequest("User with this email already exists.");
            }
            else
            {
                _dbContext.Users.Add(new User
                {
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    Email = userDTO.Email,
                    Password = userDTO.Password
                }); 

                _dbContext.SaveChanges();

                return Ok("User registered successfully.");
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult actionResult(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check whether a user with the same email and password exists
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == loginDTO.Email && u.Password == loginDTO.Password);
            
            if (existingUser != null)
            {
                return Ok("Login successful.");
            }
            else
            {
                return Unauthorized("Invalid email or password.");
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _dbContext.Users.ToList();
            return Ok(users);
        }

        [HttpGet]
        //[Route("GetUserById")]  This one will need GetUserById?id=3
        [Route("GetUserById/{id}")]  // This one will use GetUserById/3
        public IActionResult GetUserById(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == id);
            
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(user);
        }
    }
}
