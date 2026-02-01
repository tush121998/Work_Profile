using System;
using Shop.Web.Models;

namespace Shop.Web.Service.IService;

public interface IAuthService
{
    Task<ResponseDto> LoginAsync(LoginRequestDto loginRequest);
    Task<ResponseDto> RegisterAsync(RegisterationRequestDto registrationRequest);
    Task<ResponseDto> AssignRoleAsync(RegisterationRequestDto registrationRequest);
}
