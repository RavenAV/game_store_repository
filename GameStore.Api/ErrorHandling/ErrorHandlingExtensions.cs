using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.ErrorHandling;

public static class ErrorHandlingExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
                                .CreateLogger("Error handling");

            var exceptionDetails = context.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionDetails?.Error;

            logger.LogError(exception, "Could not process a request on machine {Machine}. TraceId: {TraceId}",
            Environment.MachineName,
            Activity.Current?.TraceId);

            var problem = new ProblemDetails
            {
                Title = "We made a mistake, but we're working on it!",
                Status = StatusCodes.Status500InternalServerError,
                Extensions =
                {
                    { "traceId", Activity.Current?.TraceId.ToString() }
                }
            };

            var enviroment = context.RequestServices.GetRequiredService<IHostEnvironment>();

            if (enviroment.IsDevelopment())
            {
                problem.Detail = exception?.ToString();
            }

            await Results.Problem(problem).ExecuteAsync(context);
        });
    }
}