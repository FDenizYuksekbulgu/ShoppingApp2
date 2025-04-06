namespace ShoppingApp2.WebApi.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger; // Loglama işlemi için logger.

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        // Constructor, middleware'i başlatırken gerekli olan logger ve next delegate'i alır.
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context) // InvokeAsync metodu, gelen HTTP isteğini işlemek için çağrılır.
        {
            var request = context.Request; // İstek nesnesini alır.
            var user = context.User.Identity?.Name ?? "Anonymous"; // Kimlik doğrulaması yapılmışsa kullanıcı adını alır, aksi takdirde "Anonymous" döner.

            _logger.LogInformation("Request Information: URL: {Url}, Time: {Time}, User: {User}",  // Gelen isteğin URL'si, zaman damgası ve kullanıcı kimliği ile loglama yapılır.
                request.Path, DateTime.Now, user);

            await _next(context);
        }
    }
}
