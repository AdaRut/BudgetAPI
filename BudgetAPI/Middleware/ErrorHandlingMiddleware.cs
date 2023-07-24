using BudgetAPI.Exceptions;

namespace BudgetAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            } catch (NotFoundException nfex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync(nfex.Message);

            } catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("Something went wrong...");
            }
        }
    }
}
