using System;
using Shop.Web.Models;

namespace Shop.Web.Service.IService;

public interface IBaseService
{
    Task<ResponseDto> SendAsync<T>(RequestDto requestDto, bool withToken = true);
}

