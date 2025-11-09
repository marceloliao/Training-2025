using First_AspNetCoreMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Win32;

namespace First_AspNetCoreMVC.Components
{
    public class RegistrationListViewComponent: ViewComponent
    {

        //private IDataService _dataService;
        //private IConfigurationRoot _config;
        private IOptions<RegistrationConfig> _regConfig;
        private ILogger<RegistrationListViewComponent> _logger;
        private DemoContext _dbContext;

        // Changed IDataService to DemoContext
        public RegistrationListViewComponent(DemoContext dbContext, IOptions<RegistrationConfig> regConfig, ILogger<RegistrationListViewComponent> logger)
        {
            this._dbContext = dbContext;
            //this._dataService = dataService;
            this._regConfig = regConfig;
            this._logger = logger;
        }

        //public RegistrationListViewComponent(IDataService dataService, IOptions<RegistrationConfig> regConfig, ILogger<RegistrationListViewComponent> logger)
        //{
        //    this._dataService = dataService;
        //    this._regConfig = regConfig;
        //    this._logger = logger;
        //}

        //public RegistrationListViewComponent(IDataService dataService)
        //{
        //    this._dataService = dataService;

        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            this._logger.LogInformation($"Invoking {nameof(RegistrationListViewComponent)}");

            var registrations = await this._dbContext.Registrations.ToListAsync();
            //var registrations = await this._dataService.GetRegistrations();

            //var pageSize = _regConfig.GetValue<int?>("RegistrationList:PageSize")?? 25;
            var pageSize = _regConfig.Value.PageSize;

            this._logger.LogInformation($"Exiting InvokeAsync of {nameof(RegistrationListViewComponent)}");

            return View(registrations.Take(pageSize));

            //return View(registrations);
        }    
    }
}
