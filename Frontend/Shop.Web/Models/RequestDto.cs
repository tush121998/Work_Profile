using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using static Shop.Web.Utility.SD;

namespace Shop.Web.Models;

public class RequestDto
{
    public APITYPE ApiType { get; set; } = APITYPE.GET;
    public string Url { get; set; }
    public object Data { get; set; }
    public string AccessToken { get; set; }
}
