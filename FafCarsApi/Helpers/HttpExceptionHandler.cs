using FafCarsApi.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FafCarsApi.Helpers;

public class HttpExceptionHandler(ILogger<HttpExceptionHandler> logger) : IExceptionHandler {
  public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken) {
    if (exception is not HttpException httpException) {
      return false;
    }

    logger.LogError(
      httpException,
      "Exception occurred: {Message}",
      httpException.Message);

    ProblemDetails problemDetails = httpException switch {
      BadRequestException => new() {
        Status = StatusCodes.Status400BadRequest,
        Title = "Bad Request",
        Detail = httpException.Message
      },
      NotFoundException => new() {
        Status = StatusCodes.Status404NotFound,
        Title = "Not Found",
        Detail = httpException.Message
      },
      UnauthorizedException => new() {
        Status = StatusCodes.Status401Unauthorized,
        Title = "Unauthorized",
        Detail = httpException.Message
      },
      _ => throw new ArgumentOutOfRangeException()
    };

    httpContext.Response.StatusCode = problemDetails.Status!.Value;

    await httpContext.Response
      .WriteAsJsonAsync(problemDetails, cancellationToken);

    return true;
  }
}
