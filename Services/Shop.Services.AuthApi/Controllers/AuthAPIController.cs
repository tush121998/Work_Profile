using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Services.AuthApi.Models.Dto;
using Shop.Services.AuthApi.Service.IService;

namespace Shop.Services.AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private readonly IAuthService _authService;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new ResponseDto();
        }
        

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto request)
        {
            var user = await _authService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("Registration failed");
            }

            _response.Result = user;
            _response.Message = "User registered successfully";
            _response.IsSuccess = true;
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var loginResponse = await _authService.LoginAsync(request);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Login failed";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            _response.IsSuccess = true;
            _response.Message = "Login successful";
            return Ok(_response);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterationRequestDto request)
        {
            var result = await _authService.AssignRoleAsync(request.Name, request.Role);
            if (!result)
            {
                _response.IsSuccess = false;
                _response.Message = "Role assignment failed";
                return BadRequest(_response);
            }
            _response.IsSuccess = true;
            _response.Message = "Role assigned successfully";
            return Ok(_response);
        }
    }
}
