using System;

namespace Shop.Web.Models;

public class LoginResponseDto
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public UserDto User { get; set; }
}
