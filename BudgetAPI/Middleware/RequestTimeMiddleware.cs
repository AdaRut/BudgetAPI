using System.Diagnostics;

namespace BudgetAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            await next.Invoke(context);
            stopwatch.Stop();
            long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            bool elapsedMoreThan3Seconds = (elapsedMilliseconds / 1000) > 3;
            if (elapsedMoreThan3Seconds) {
                _logger.LogError("Request Method " + context.Request.Method+ " at " + context.Request.Path
                    + $" took {elapsedMilliseconds}  milliseconds");
            }
        }
    }
}
