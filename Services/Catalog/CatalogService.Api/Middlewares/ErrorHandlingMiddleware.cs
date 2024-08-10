using NStore.Shared.Constants;
using NStore.Shared.Exceptions;

namespace Catalog.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Exception exception = GetInnermostExceptionMessage(ex);
            _logger.LogError(exception, exception.Message);

            string message = ErrorMessageConstants.UnexpectedErrorMessage;

            if (exception != null && exception is DomainException) 
            {
                message = exception.Message;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "text/plain";
            }

            await context.Response.WriteAsJsonAsync(new { error = message });
        }
    }

    public static Exception GetInnermostExceptionMessage(Exception ex)
    {
        if (ex == null)
        {
            return null;
        }

        if (ex.InnerException == null)
        {
            return ex;
        }

        return GetInnermostExceptionMessage(ex.InnerException);
    }
}
