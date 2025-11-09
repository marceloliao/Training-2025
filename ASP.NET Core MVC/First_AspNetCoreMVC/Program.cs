using First_AspNetCoreMVC;
using First_AspNetCoreMVC.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure appsetting in a variable named configuration
var Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

// Add services to the container.
builder.Services.AddSingleton<IDataService, FakeData>();

// Add DBContext as a service
builder.Services.AddDbContext<DemoContext>(options => options.UseSqlite("Data Source=demo.db"));

// Registrating my configuration as a service, if we change in appsetting to a class, this is not needed
//builder.Services.AddSingleton<IConfigurationRoot> (configuration);
builder.Services.Configure<RegistrationConfig>(Configuration.GetSection("RegistrationList"));

builder.Services.AddControllersWithViews();
builder.Services.AddMvc();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Create a scope to access the DbContext
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DemoContext>();
    dbContext.Seed().Wait();
}

app.UseHttpsRedirection();

// The following block directs the default file to be used when the user accesses the root URL
//DefaultFilesOptions options = new DefaultFilesOptions();
//options.DefaultFileNames.Clear();
//options.DefaultFileNames.Add("student.html");
//app.UseDefaultFiles(options);

//app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

//app.UseMyMiddleware();

//app.UseWelcomePage();

//app.UseDefaultFiles();


//app.Run(async (context) =>
//{
//    await context.Response.WriteAsync("Hello World!");
//});


app.Run();
