using System;
using AutoMapper;
using Services.Coupon.API.Models.Dto;

namespace Services.Coupon.API;

public class MappingConfiguration
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            // Add your mappings here
            // Example: config.CreateMap<Source, Destination>();
            config.CreateMap<Models.Coupon, CouponDto>();
            config.CreateMap<CouponDto, Models.Coupon>();
        });

        return mappingConfig;
    }
}
