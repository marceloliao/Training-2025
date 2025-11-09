using Microsoft.AspNetCore.Mvc;

namespace First_AspNetCoreMVC.Controllers
{
    public class StudentController : Controller
    {
        public String Index()
        {
            return "This is Student page";
        }
    }
}
