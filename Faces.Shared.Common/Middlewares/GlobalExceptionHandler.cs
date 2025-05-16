namespace Faces.Shared.Common.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;

    public class GlobalExceptionHandler(RequestDelegate _next, ILogger<GlobalExceptionHandler> _logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error"
                };

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                var json = JsonConvert.SerializeObject(problemDetails);
                await context.Response.WriteAsync(json);
            }
        }
    }
}