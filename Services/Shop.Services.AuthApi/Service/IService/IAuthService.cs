using System;
using Shop.Services.AuthApi.Models.Dto;

namespace Shop.Services.AuthApi.Service.IService;

public interface IAuthService
{
    Task<UserDto> RegisterAsync(RegisterationRequestDto registrationRequest);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest);
    Task<bool> AssignRoleAsync(string name, string roleName);
}
