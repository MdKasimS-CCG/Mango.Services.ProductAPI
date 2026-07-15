using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;

namespace Mango.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var loggerFactory = LoggerFactory.Create(builder => {
                builder.AddConsole();
            });

            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
                //config.CreateMap<Product, ProductDto>();
            }, loggerFactory);
            return mappingConfig;
        }
    }
}