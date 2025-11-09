using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;


namespace Default_AspNetCoreWebApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //// In production, the Angular files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Use Method passes to the next one in pipeline, the following doesn't do anything
            app.Use(async (ctx, next) =>
            {
                await next();
            });

            app.UsePingMiddleware();

            // Use Method passes to the next one in pipeline, commented the following out
            // so we can transfer it to a class
            //app.Use(async (ctx, next) =>
            //{
            //    if (ctx.Request.Headers.ContainsKey("Ping"))
            //    {
            //        ctx.Response.Headers["Pong"] = "Returned";
            //        ctx.Response.StatusCode = (int)HttpStatusCode.OK;
            //        return;
            //    }

            //    await next();
            //});

            // Use Map to desviar pipeline
            app.Map("/health", healthApp =>
            {
                // Using anoynymous fucntion
                //healthApp.Run(delegate(HttpContext healthContext) 
                //{
                //    return healthContext.Response.WriteAsJsonAsync("This is by using an anonymous function.");
                //});

                // Using a lambda expression
                healthApp.Run(healthContext =>
                {
                    return healthContext.Response.WriteAsJsonAsync("This is by using a lambda expression.");
                });
            });

            //// Use MapWhen when certain condition is met
            //app.MapWhen(ctx => !ctx.User.Identity.IsAuthenticated, authApp =>
            //{
            //    authApp.Run(authContext =>
            //    {
            //        return authContext.Response.WriteAsJsonAsync("How did you get here?");
            //    });
            //});

            //app.UseStaticFiles();
            ////app.UseMiddleware<CustomMiddleware>();

            //app.UseWelcomePage();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World");
            });

            //app.UseHttpsRedirection();


            //app.UseSpaStaticFiles();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller}/{action=Index}/{id?}");
            //});

            //app.UseSpa(spa =>
            //{
            //    // To learn more about options for serving an Angular SPA from ASP.NET Core,
            //    // see https://go.microsoft.com/fwlink/?linkid=864501

            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseAngularCliServer(npmScript: "start");
            //    }
            //});
        }
    }
}
