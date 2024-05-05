using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using StackOverflow.Domain.Exceptions;

namespace StackOverflow.Web.Middlewires;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
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

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var error = context.Features.Get<IExceptionHandlerFeature>();
        if (error != null)
        {
            // Log the exception
            Log.Error(ex, "An unhandled exception occurred");

            // Set the status code and content type
            context.Response.ContentType = "application/json";
            if (ex is ArgumentException)
            {
                context.Response.StatusCode = 400;
            }
            else if (ex is NotFoundException)
            {
                context.Response.StatusCode = 404;
            }
            else
            {
                context.Response.StatusCode = 500;
            }

            // Write a consistent error response
            var errorResponse = new
            {
                message = ex.Message,
                type = ex.GetType().Name
            };
            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}