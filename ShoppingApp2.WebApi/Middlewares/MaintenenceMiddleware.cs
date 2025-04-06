using ShoppingApp2.Business.Operations.Setting;

namespace ShoppingApp2.WebApi.Middlewares
{
    public class MaintenenceMiddleware
    {
        private readonly RequestDelegate _next;

        public MaintenenceMiddleware(RequestDelegate next) //Bir sonraki middleware'e geçişi sağlayacak olan delegate.
        {
            _next = next;  //Bir sonraki middleware'e yönlendirme için kullanılır.
        }  // Constructor, bir sonraki middleware'i almak için kullanılır.

        public async Task Invoke(HttpContext context)
        { // ISettingService üzerinden bakım modu durumunu almak için gerekli servis.
            var settingService = context.RequestServices.GetRequiredService<ISettingService>();
            bool maintenenceMode = settingService.GetMaintenenceState();
            // Bakım modunun aktif olup olmadığını kontrol ediyor.
            if (context.Request.Path.StartsWithSegments("/api/auth/login") ||  // Eğer istek "/api/auth/login" veya "/api/settings" yoluna yöneliyorsa, bakım modu kontrol edilmez.
                context.Request.Path.StartsWithSegments("/api/settings"))
            {
                await _next(context); // Bakım modu kontrolü yapılmadan, istek bir sonraki middleware'e yönlendirilir.
                return;
            }

            if (maintenenceMode) // Eğer bakım modu aktifse, kullanıcıya bakım mesajı döndürülür.
            {
                await context.Response.WriteAsync("Bakım çalışması sebebiyle şu anda hizmet verememekteyiz.");
            }
            else
            {
                await _next(context);  // Bakım modu aktif değilse, istek bir sonraki middleware'e yönlendirilir.
            }
        }
    }
}
