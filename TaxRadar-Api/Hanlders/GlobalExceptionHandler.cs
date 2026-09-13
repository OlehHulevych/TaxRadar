using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaxRadar_Application.Exceptions;

namespace TaxRadar_backned.Hanlders;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
  


    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled error occured");
        httpContext.Response.StatusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            ForbiddenAccessException => StatusCodes.Status403Forbidden,
            BadRequestException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError


        };
        var problemDetails = new ProblemDetails
        {
            Type = exception.GetType().Name,
            Title = "An error occured",
            Detail = exception.Message
        };

        if (exception is ValidationException validationException)
            problemDetails.Extensions["errors"] = validationException.Errors;

        await httpContext.Response.WriteAsJsonAsync(problemDetails);
        return true;
    }
}