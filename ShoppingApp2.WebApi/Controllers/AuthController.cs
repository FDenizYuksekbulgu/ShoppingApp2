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

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // TODO: İleride Action Filter olarak kodlanacak.

            var addUserDto = new AddUserDto
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,  

            };
            var result = await _userService.AddUser(addUserDto);

            if (result.IsSucceed)
                return Ok();
            else
                return BadRequest(result.Message);
        }


        // HttpGet -> Veri URL üzerinden taşınır - query string
        // Firewall ve benzeri uygulamalarınız URL'i loglar, böyle bir durumda şifreyi'de loglamış olur.
        // GÜVENLİK AÇIĞI
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // TODO: İleride Action Filter olarak kodlanacak.

            /*
            var loginUserDto = new LoginUserDto
            {
                Email = request.Email,
                Password = request.Password,
            };

            var result = _userService.LoginUser(loginUserDto);
            */
            var result = _userService.LoginUser(new LoginUserDto { Email = request.Email, Password = request.Password });

            if (!result.IsSucceed)
                return BadRequest(result.Message);
            
            var user = result.Data;

            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.Role,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });


            return Ok(new LoginResponse
            {
                Message = "Giriş başarıyla tamamlandı.",
                Token = token
            });
        }
        [HttpGet("me")]
        [Authorize] // Token yoksa cevap yok
        public IActionResult GetMyUser()
        {
            return Ok();
        }
    }
}
