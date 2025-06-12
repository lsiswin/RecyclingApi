using AutoMapper;
using RecyclingApi.Application.DTOs.ProductDTOs;
using RecyclingApi.Domain.Entities.Products;

namespace RecyclingApi.Application.Mappings;

/// <summary>
/// 产品相关的AutoMapper映射配置文件
/// 定义了实体类与DTO之间的映射关系
/// </summary>
public class ProductMappingProfile : Profile
{
    /// <summary>
    /// 初始化映射配置
    /// </summary>
    public ProductMappingProfile()
    {
        // Product实体与DTO的映射配置
        CreateMap<Product, ProductDto>();
        // CreateProductDto到Product实体的映射配置
        CreateMap<CreateUpdateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.ProcessSteps, opt => opt.Ignore());

        // UpdateProductDto到Product实体的映射配置（仅映射非空值）
        CreateMap<CreateUpdateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.ProcessSteps, opt => opt.Ignore())
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // ProductCategory实体与DTO的映射配置
        CreateMap<ProductCategory, ProductCategoryDto>()
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count));

        // ProcessStep实体与DTO的映射配置
        CreateMap<ProcessStep, ProcessStepDto>();
    }
} 