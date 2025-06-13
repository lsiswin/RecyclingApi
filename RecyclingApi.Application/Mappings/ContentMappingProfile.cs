using AutoMapper;
using RecyclingApi.Application.DTOs.CaseDTOs;
using RecyclingApi.Application.DTOs.Company;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Domain.Entities.Content;

namespace RecyclingApi.Application.Mappings
{
    public class ContentMappingProfile : Profile
    {
        public ContentMappingProfile()
        {
            // CompanyInfo 映射
            CreateMap<CompanyInfo, CompanyInfoDto>();
            CreateMap<UpdateCompanyInfoDto, CompanyInfo>();

            // CompanyAdvantage 映射
            CreateMap<CompanyAdvantage, CompanyAdvantageDto>();
            CreateMap<CreateUpdateAdvantageDto, CompanyAdvantage>();

            // CompanyMilestone 映射
            CreateMap<CompanyMilestone, CompanyMilestoneDto>();
            CreateMap<CreateUpdateMilestoneDto, CompanyMilestone>();
            // TeamMember 映射
            CreateMap<TeamMember, TeamMemberDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeId));

            CreateMap<CreateUpdateTeamMemberDto, TeamMember>();

            // TeamMemberType 映射
            CreateMap<TeamMemberType, TeamMemberTypeDto>();
            CreateMap<CreateUpdateTeamMemberTypeDto, TeamMemberType>();

            // Banner 映射
            CreateMap<Banner, BannerDto>();
            CreateMap<CreateUpdateBannerDto, Banner>();

            // Case 映射
            // Case → CaseDto
            CreateMap<Case, CaseDto>()
                // 处理逗号分隔的标签字符串 → List<string>
                .ForMember(dest => dest.Tags, opt => opt.ConvertUsing(new TagsConverter()))

                // 处理分号分隔的服务亮点 → List<string>
                .ForMember(dest => dest.Highlights, opt => opt.ConvertUsing(new HighlightsConverter()))

                // 忽略 DTO 中不存在的 UpdatedAt 字段
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // CaseDto → Case (反向映射)
            CreateMap<CaseDto, Case>()
                // 处理 List<string> → 逗号分隔的标签字符串
                .ForMember(dest => dest.Tags, opt => opt.ConvertUsing(new TagsReverseConverter()))

                // 处理 List<string> → 分号分隔的服务亮点
                .ForMember(dest => dest.Highlights, opt => opt.ConvertUsing(new HighlightsReverseConverter()))

                // 自动设置更新时间（当通过 DTO 更新时）
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
           CreateMap<CreateUpdateCaseDto, Case>()
                // 处理 List<string> → 逗号分隔的标签字符串
                .ForMember(dest => dest.Tags, opt => opt.ConvertUsing(new TagsReverseConverter()))

                // 处理 List<string> → 分号分隔的服务亮点
                .ForMember(dest => dest.Highlights, opt => opt.ConvertUsing(new HighlightsReverseConverter()))

                // 自动设置更新时间（当通过 DTO 更新时）
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }

        #region 自定义值转换器
        // Tags 字符串 ↔ 列表转换
        private class TagsConverter : IValueConverter<string, List<string>>
        {
            public List<string> Convert(string source, ResolutionContext context)
                => source?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(s => s.Trim())
                         .ToList() ?? new List<string>();
        }

        private class TagsReverseConverter : IValueConverter<List<string>, string>
        {
            public string Convert(List<string> source, ResolutionContext context)
                => source != null ? string.Join(",", source) : null;
        }

        // Highlights 字符串 ↔ 列表转换
        private class HighlightsConverter : IValueConverter<string, List<string>>
        {
            public List<string> Convert(string source, ResolutionContext context)
                => source?.Split(';', StringSplitOptions.RemoveEmptyEntries)
                         .Select(s => s.Trim())
                         .ToList() ?? new List<string>();
        }

        private class HighlightsReverseConverter : IValueConverter<List<string>, string>
        {
            public string Convert(List<string> source, ResolutionContext context)
                => source != null ? string.Join(";", source) : null;
        }
        #endregion
    }

} 