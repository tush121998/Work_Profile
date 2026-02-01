using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.IService;
using Shop.Web.Utility;

namespace Shop.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequest = new LoginRequestDto();
            return View(loginRequest);
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            ResponseDto response = await _authService.LoginAsync(loginRequest);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDto loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));
                TempData["Success"] = "Login Successful";
                await SignInUser(loginResponse);
                _tokenProvider.SetToken(loginResponse.Token);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = response.Message;
                return View(loginRequest);
            }
        }

         [HttpGet]
        public async Task<IActionResult> Register()
        {
            var rolelist = new List<SelectListItem>()
            {
                new SelectListItem(){Text = SD.Role.Admin.ToString(), Value = SD.Role.Admin.ToString()},
                new SelectListItem(){Text = SD.Role.Customer.ToString(), Value = SD.Role.Customer.ToString()}
            };
            ViewBag.RoleList = rolelist;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDto registerationRequest)
        {
            ResponseDto result = await _authService.RegisterAsync(registerationRequest);
            ResponseDto assignRole;
            if (result != null && result.IsSuccess)
            {
                if(string.IsNullOrEmpty(registerationRequest.Role))
                {
                    registerationRequest.Role = SD.Role.Customer.ToString();
                }
                assignRole = await _authService.AssignRoleAsync(registerationRequest);
                if(assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                TempData["Error"] = result.Message;
            }
            var rolelist = new List<SelectListItem>()
            {
                new SelectListItem(){Text = SD.Role.Admin.ToString(), Value = SD.Role.Admin.ToString()},
                new SelectListItem(){Text = SD.Role.Customer.ToString(), Value = SD.Role.Customer.ToString()}
            };
            ViewBag.RoleList = rolelist;

            return View(registerationRequest);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto loginResponse)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(loginResponse.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name, jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwtToken.Claims.First(c => c.Type == "role").Value));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }
    }
}
