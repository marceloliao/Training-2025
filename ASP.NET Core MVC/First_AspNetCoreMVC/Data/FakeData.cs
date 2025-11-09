using First_AspNetCoreMVC.Models;

namespace First_AspNetCoreMVC.Data
{
    public class FakeData: IDataService
    {
        private IEnumerable<RegistrationModel> Registrations;

        public FakeData()
        {
            LoadFakeDate();
        }

        private void LoadFakeDate() => Registrations = new List<RegistrationModel>
            {
              new RegistrationModel {FirstName = "Mary1", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary2", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary3", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary4", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary5", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary6", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary7", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary8", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary9", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary10", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary11", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary12", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary13", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary14", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary15", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary16", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary17", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary18", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary19", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary20", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary21", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary22", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary23", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary24", LastName="Johnson", Email="mjohnson@hotmail.com" },
              new RegistrationModel {FirstName = "Mary25", LastName="Johnson", Email="mjohnson@hotmail.com" },
            };

        public Task<IEnumerable<RegistrationModel>> GetRegistrations() => Task.FromResult(Registrations);
    }
}
