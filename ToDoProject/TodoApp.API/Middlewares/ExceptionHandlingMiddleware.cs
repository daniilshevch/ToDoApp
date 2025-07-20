using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ToDoApp.API.Middlewares
{
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
                await _next.Invoke(context);
            }
            catch(ValidationException ex)
            {
                context.Response.StatusCode = 400;
                var errors = ex.Errors.Select(error => new
                {
                    Property = error.PropertyName,
                    Error = error.ErrorMessage
                });
                var response = new
                {
                    Message = "Model validation failed",
                    Errors = errors
                };
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                var response = new
                {
                    Message = "Internal Server Error",
                    Detail = ex.Message
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
