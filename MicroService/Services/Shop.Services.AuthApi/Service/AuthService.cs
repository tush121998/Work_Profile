using System;
using Microsoft.AspNetCore.Identity;
using Services.AuthApi.Data;
using Shop.Services.AuthApi.Models;
using Shop.Services.AuthApi.Models.Dto;
using Shop.Services.AuthApi.Service.IService;

namespace Shop.Services.AuthApi.Service;

public class AuthService  : IAuthService
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext context, UserManager<ApplicationUser> userManager,
         RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<UserDto> RegisterAsync(RegisterationRequestDto registrationRequest)
    {
        var user = new ApplicationUser
        {
            UserName = registrationRequest.Email,
            Email = registrationRequest.Email,
            NormalizedEmail = registrationRequest.Email.ToUpper(),
            Name = registrationRequest.Name,
            PhoneNumber = registrationRequest.PhoneNumber
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registrationRequest.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User registration failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        catch (Exception)
        {
            throw;
        }

        var userToReturn = _context.ApplicationUsers.First(u => u.UserName == registrationRequest.Email);
        return new UserDto
        {
            Id = userToReturn.Id,
            Email = userToReturn.Email,
            Name = userToReturn.Name,
            PhoneNumber = userToReturn.PhoneNumber
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequest)
    {
       var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == loginRequest.Username);

       bool isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
        if (user == null || !isPasswordValid)
        {
            return new LoginResponseDto
            {
                User = null,
                Token = string.Empty,
            };
        }
        // If user is found and password is valid, generate token.
        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenGenerator.GenerateToken(user, roles);

        UserDto userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            PhoneNumber = user.PhoneNumber
        };

        return new LoginResponseDto
        {
            User = userDto,
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(3)
        };
    }

    public Task<bool> AssignRoleAsync(string name, string roleName)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.Name == name);
        if (user == null)
        {
            return Task.FromResult(false);
        }
        var roleExists = _roleManager.RoleExistsAsync(roleName).Result;
        if (!roleExists)
        {
            var roleResult = _roleManager.CreateAsync(new IdentityRole(roleName)).Result;
            if (!roleResult.Succeeded)
            {
                return Task.FromResult(false);
            }
        }
        var result = _userManager.AddToRoleAsync(user, roleName).Result;
        return Task.FromResult(result.Succeeded);
    }
}