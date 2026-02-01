using System;

namespace Shop.Services.AuthApi.Models.Dto;

public class LoginRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}
