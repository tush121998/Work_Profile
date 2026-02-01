using System;
using AutoMapper;

namespace Shop.Services.ProductAPI;

public class MappingConfiguration
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            // Add your mappings here
            config.CreateMap<Models.Product, Models.Dto.ProductDto>().ReverseMap();
        });
        return mappingConfig;
    }
}
