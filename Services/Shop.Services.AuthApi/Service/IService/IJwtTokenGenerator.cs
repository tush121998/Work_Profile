using System;
using Shop.Services.AuthApi.Models;

namespace Shop.Services.AuthApi.Service.IService;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser user, IEnumerable<string> roles);
}
