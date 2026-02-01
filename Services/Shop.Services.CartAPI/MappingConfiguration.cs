using System;
using AutoMapper;

namespace Shop.Services.CartAPI;

public class MappingConfiguration
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            // Add your mappings here
            config.CreateMap<Models.CartHeader, Models.Dto.CartHeaderDto>().ReverseMap();
            config.CreateMap<Models.CartDetails, Models.Dto.CartDetailsDto>().ReverseMap();
        });
        return mappingConfig;
    }
}
