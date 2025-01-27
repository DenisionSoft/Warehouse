using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Warehouse.Business.Exceptions;

namespace Warehouse.Web.Infrastructure;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ProblemDetailsFactory problemDetailsFactory;

    public GlobalExceptionHandler(ProblemDetailsFactory problemDetailsFactory)
    {
        this.problemDetailsFactory = problemDetailsFactory;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = problemDetailsFactory.CreateProblemDetails(
            httpContext,
            title: exception.GetType().Name,
            detail: exception.Message);

        problemDetails.Status = exception switch
        {
            ArgumentOutOfRangeException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            EntityNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
