using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Shop.Web.Models;
using static Shop.Web.Utility.SD;

namespace Shop.Web.Service.IService;

public class BaseService : IBaseService
{
    private IHttpClientFactory IHttpClientFactory { get; set; }
    private ITokenProvider ITokenProvider { get; set; }
    public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
    {
        IHttpClientFactory = httpClientFactory;
        ITokenProvider = tokenProvider;
    }
    public async Task<ResponseDto> SendAsync<T>(RequestDto requestDto, bool withToken = true)
    {
        try
        {
            HttpClient client = IHttpClientFactory.CreateClient("AuthAPI");
            HttpRequestMessage request = new HttpRequestMessage();

            request.Headers.Add("Accept", "application/json");
            request.RequestUri = new Uri(requestDto.Url);
            if (requestDto.Data != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
            }

            request.Method = requestDto.ApiType switch
            {
                APITYPE.GET => HttpMethod.Get,
                APITYPE.POST => HttpMethod.Post,
                APITYPE.PUT => HttpMethod.Put,
                APITYPE.DELETE => HttpMethod.Delete,
                _ => HttpMethod.Get,
            };
            if (withToken)
            {
                var token = ITokenProvider.GetToken();
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);

            switch(response.StatusCode)
            {
                case System.Net.HttpStatusCode.Forbidden:
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Forbidden"
                    };
                case System.Net.HttpStatusCode.NotFound:
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Not Found"
                    };
                case System.Net.HttpStatusCode.InternalServerError:
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Internal Server Error"
                    };
                case System.Net.HttpStatusCode.BadRequest:
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Bad Request"
                    };
                    case System.Net.HttpStatusCode.Unauthorized:
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Unauthorized"
                    };
                default:
                    var result = await response.Content.ReadAsStringAsync();
                    var responseDto = JsonConvert.DeserializeObject<ResponseDto>(result);
                    responseDto.IsSuccess = true;
                    return responseDto;
            }
           
        }
        catch (Exception ex)
        {
            return new ResponseDto
            {
                IsSuccess = false,
                Message = ex.Message
            };
        }
      
    }
}
