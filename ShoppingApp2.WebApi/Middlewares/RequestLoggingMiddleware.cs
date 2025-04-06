namespace ShoppingApp2.WebApi.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var user = context.User.Identity?.Name ?? "Anonymous";

            _logger.LogInformation("Request Information: URL: {Url}, Time: {Time}, User: {User}",
                request.Path, DateTime.Now, user);

            await _next(context);
        }
    }
}
