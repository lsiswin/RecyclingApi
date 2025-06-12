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
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Employee.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EmployeeId));

            CreateMap<CreateUpdateTeamMemberDto, TeamMember>();

            // TeamMemberType 映射
            CreateMap<TeamMemberType, TeamMemberTypeDto>();
            CreateMap<CreateUpdateTeamMemberTypeDto, TeamMemberType>();

            // Banner 映射
            CreateMap<Banner, BannerDto>();
            CreateMap<CreateUpdateBannerDto, Banner>();

            // Case 映射
            CreateMap<Case, CaseDto>();
            CreateMap<CreateUpdateCaseDto, Case>();
        }
    }
} 