using System;

namespace Shop.Services.AuthApi.Models.Dto;

public class LoginResponseDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public UserDto User { get; set; }
}
