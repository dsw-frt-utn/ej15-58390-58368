using System.Net;
using System.Threading.Tasks;
using Dsw2026Ej15.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Dsw2026Ej15.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(ValidationException ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(ex.Message);
            }
            catch (Exception)
            {
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An error ocurred while processing your request.",
                    Detail = "Internal Server Error."
                });
            }
        }
    }
}
