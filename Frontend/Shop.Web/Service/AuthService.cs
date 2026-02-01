using System;
using Shop.Web.Models;
using Shop.Web.Service.IService;
using Shop.Web.Utility;

namespace Shop.Web.Service;

public class AuthService : IAuthService
{
    private readonly IBaseService _baseService;

    public AuthService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    public Task<ResponseDto> LoginAsync(LoginRequestDto loginRequest)
    {
        RequestDto dto = new()
        {
            Url = SD.AuthApiBase + "/AuthAPI/login",
            ApiType = SD.APITYPE.POST,
            Data = loginRequest
        };
        return _baseService.SendAsync<ResponseDto>(dto, false);
    }

    public Task<ResponseDto> RegisterAsync(RegisterationRequestDto registrationRequest)
    {
        RequestDto dto = new()
        {
            Url = SD.AuthApiBase + "/AuthAPI/register",
            ApiType = SD.APITYPE.POST,
            Data = registrationRequest
        };
        return _baseService.SendAsync<ResponseDto>(dto, false);
    }

    public async Task<ResponseDto> AssignRoleAsync(RegisterationRequestDto registrationRequest)
    {
       RequestDto dto = new()
        {
            Url = SD.AuthApiBase + "/AuthAPI/assign-role",
            ApiType = SD.APITYPE.POST,
            Data = registrationRequest
        };
        return await _baseService.SendAsync<ResponseDto>(dto, true);
    }
}
