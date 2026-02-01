using System;
using Microsoft.AspNetCore.Identity;

namespace Shop.Services.AuthApi.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}
