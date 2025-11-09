using First_AspNetCoreMVC.Models;

namespace First_AspNetCoreMVC.Data
{
    public interface IDataService
    {
        Task<IEnumerable<RegistrationModel>> GetRegistrations();
    }
}
