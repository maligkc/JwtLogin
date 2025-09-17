using Business.Abstract;
using Business.Auth;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly JwtHelper _jwtHelper;

        public AuthController(IAuthService authService, JwtHelper jwtHelper)
        {
            _authService = authService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            _authService.Register(dto.UserName, dto.Password);
            return Ok("Kayıt başarılı!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = _authService.Login(dto.UserName, dto.Password);
            if(user == null)
                return NotFound("Böyle bir kullanıcı yok");

            var token = _jwtHelper.GenerateToken(user);
            return Ok(new { Token = token });
        }
        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = _authService.GetUsers();
            if (users == null)
                return NotFound("Sistemde hiç kullanıcı yok.");

            return Ok(users);
        }

       
    }
}
