using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Default_AspNetCoreWebApplication
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class PingMiddleware
    {
        private readonly RequestDelegate _next;

        public PingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.ContainsKey("Ping"))
            {
                httpContext.Response.Headers["Pong"] = "Returned";
                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                return Task.CompletedTask;
            }

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class PingMiddlewareExtensions
    {
        public static IApplicationBuilder UsePingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PingMiddleware>();
        }
    }
}
