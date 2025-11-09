using Microsoft.AspNetCore.Mvc;
using First_AspNetCoreMVC.Models;
using First_AspNetCoreMVC.Data;

namespace First_AspNetCoreMVC.Controllers
{
    public class DemoController : Controller
    {
        private DemoContext _dbContext;

        public IActionResult Index()
        {
            return View();
        }

        public DemoController(DemoContext dbContext)
        {
            this._dbContext = dbContext;
        }

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult Register(int? id)
        {
            if (id.HasValue)
            {
                var reg = _dbContext.Registrations.SingleOrDefault(r => r.Id == id);
                if (reg != null)
                    return View(reg);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                var reg = _dbContext.Registrations.SingleOrDefault(r => r.Id == id);
                if (reg != null)
                {
                    _dbContext.Registrations.Remove(reg);

                    await _dbContext.SaveChangesAsync();
                    return View(nameof(Index));
                }
            }

            return View(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model, string action)
        {
            var reg = _dbContext.Registrations.Find(model.Id);

            switch (action)
            {
                case "Register":
                    // Handle edit logic here
                    if (ModelState.IsValid)
                    {
                        if (model.Id == 0)  // A new record
                        {
                            _dbContext.Registrations.Add(model);
                        }
                        else  // An existing record to update
                        {                            
                            reg.Email = model.Email;
                            reg.FirstName = model.FirstName;
                            reg.LastName = model.LastName;
                        }

                        await _dbContext.SaveChangesAsync();
                        return View(nameof(Index));
                    }
                    break;
                case "Delete":
                    // Handle delete logic here
                    
                    if (reg != null)
                    {
                        _dbContext.Registrations.Remove(reg);

                        await _dbContext.SaveChangesAsync();
                        return View(nameof(Index));
                    }
                    
                    break;
            }
            //return RedirectToAction("Index");



            //if (ModelState.IsValid)
            //{
            //    if (model.Id == 0)  // A new record
            //    {
            //        _dbContext.Registrations.Add(model);
            //    }
            //    else  // An existing record to update
            //    {
            //        var reg = _dbContext.Registrations.Find(model.Id);
            //        reg.Email = model.Email;
            //        reg.FirstName = model.FirstName;
            //        reg.LastName = model.LastName;
            //    }

            //    await _dbContext.SaveChangesAsync();
            //    return View(nameof(Index));
            //}
            return View();
        }

        

        //[HttpDelete]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    var reg = _dbContext.Registrations.Find(id);
        //    if (reg != null)
        //    {
        //        _dbContext.Registrations.Remove(reg);                

        //        await _dbContext.SaveChangesAsync();
        //        return View(nameof(Index));
        //    }

        //    return View();
        //}
    }
}
