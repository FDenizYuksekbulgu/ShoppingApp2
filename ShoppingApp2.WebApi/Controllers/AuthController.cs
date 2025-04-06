using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp2.Business.Operations.User.Dtos;
using ShoppingApp2.Business.Operations.User;
using ShoppingApp2.WebApi.Models;
using ShoppingApp2.WebApi.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingApp2.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService; 

        public AuthController(IUserService userService) // AuthController sınıfının constructor'ı, IUserService bağımlılığını alır ve servis üzerinden işlemler yapılır.
        {
            _userService = userService;
        }

        [HttpPost("register")] // Burada kayıt işlemi yapılır.
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            

            var addUserDto = new AddUserDto
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,  

            };
            var result = await _userService.AddUser(addUserDto); // Kullanıcıyı ekleme işlemi yapılır.

            if (result.IsSucceed)
                return Ok(); // Başarılıysa 200 OK döndürülür.
            else
                return BadRequest(result.Message); //// Başarısızsa hata mesajı döndürülür
        }


       
        [HttpPost("login")] // Giriş işlemi (Login) burada yapılır.
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
         
            var result = _userService.LoginUser(new LoginUserDto { Email = request.Email, Password = request.Password }); // Kullanıcıyı giriş yapması için doğrulama işlemi

            if (!result.IsSucceed)
                return BadRequest(result.Message); // Giriş başarısızsa hata döndürülür.

            var user = result.Data;

            // JWT Token(Token, bir kullanıcının kimliğini doğrulamak ve yetkilendirmek için kullanılan dijital bir anahtar) oluşturulması kısmı
            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.Role, // Kullanıcı rolü "Admin" ya da "Customer" gibi burada saklanır.
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });


            return Ok(new LoginResponse
            {
                Message = "Giriş başarıyla tamamlandı.",
                Token = token // Token geri döndürülür
            });
        }
        // Kullanıcıyı almak için Token kontrolü gereklidir.
        [HttpGet("me")]
        [Authorize] // Eğer token yoksa, cevap verilmez.
        public IActionResult GetMyUser()
        {
            return Ok();
        }
    }
}
