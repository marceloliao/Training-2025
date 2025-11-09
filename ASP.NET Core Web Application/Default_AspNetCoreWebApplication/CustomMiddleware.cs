namespace Default_AspNetCoreWebApplication
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Custom logic before the next middleware
            await context.Response.WriteAsync("Hello from Custom Middleware!");

            // Call the next middleware in the pipeline
            await _next(context);

            // Custom logic after the next middleware
        }
    }
}
