using System.Net;
using System.Text.Json;

namespace ShoppingApp2.WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            } //Bu örnekte, ExceptionHandlingMiddleware sınıfı bir middleware olarak yapılandırılmıştır. Gelen her istekte, oluşabilecek hatalar try-catch bloğuyla yakalanır ve yönetilir. Eğer bir hata meydana gelirse, hata loglanır ve kullanıcıya genel bir hata mesajı döndürülür.
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An unexpected error occurred. Please try again later.",
                Details = exception.Message // Detayları istemciye göstermek istemiyorsak bu satırı kaldırabiliriz.
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
