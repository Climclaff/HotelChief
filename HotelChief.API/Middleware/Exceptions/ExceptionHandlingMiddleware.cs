namespace HotelChief.API.Middleware.Exceptions
{
    using Serilog;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Log.Error("An exception has occured => {@exception}", exception);
            var exceptionType = exception.GetType().ToString();
            context.Response.Redirect("/Home/Error?message=" + Uri.EscapeDataString(exceptionType));
        }
    }
}
