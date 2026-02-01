using System;

namespace Shop.Services.AuthApi.Models.Dto;

public class ResponseDto
{
    public object? Result { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}
