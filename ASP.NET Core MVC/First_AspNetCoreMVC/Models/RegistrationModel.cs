using System.ComponentModel.DataAnnotations;

namespace First_AspNetCoreMVC.Models
{
    public class RegistrationModel
    {
        public int Id { get; set; }
       
        public string FirstName { get; set; }

       
        public string LastName { get; set; }

       
        public string Email { get; set; }        
    }
}
