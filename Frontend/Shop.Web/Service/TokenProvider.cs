using System;
using Shop.Web.Service.IService;
using Shop.Web.Utility;

namespace Shop.Web.Service;

public class TokenProvider : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public TokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetToken()
    {
        string? token = null;
        bool? hasCookie = _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(SD.TokenCookie, out token);
        return hasCookie == true ? token : null;
    }

    public void SetToken(string token)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Append(SD.TokenCookie, token);
    }

    public void ClearToken()
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(SD.TokenCookie);
    }
}
