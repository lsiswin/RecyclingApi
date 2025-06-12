using AutoMapper;
using RecyclingApi.Application.DTOs.Company;
using RecyclingApi.Application.DTOs.ContentDTOs;
using RecyclingApi.Domain.Entities.Content;

namespace RecyclingApi.Application.Mappings
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile()
        {
            // CompanyProfile 映射
            CreateMap<CompanyProfile, CompanyProfileDto>()
                .ForMember(dest => dest.Advantages, opt => opt.Ignore())
                .ForMember(dest => dest.Milestones, opt => opt.Ignore())
                .ForMember(dest => dest.Certifications, opt => opt.Ignore());
            
            CreateMap<CreateUpdateCompanyProfileDto, CompanyProfile>()
                .ForMember(dest => dest.AdvantagesJson, opt => opt.Ignore())
                .ForMember(dest => dest.MilestonesJson, opt => opt.Ignore())
                .ForMember(dest => dest.CertificationsJson, opt => opt.Ignore());

            

            // 员工简略信息映射
            CreateMap<Domain.Entities.HR.Employee, EmployeeSimpleDto>();
        }
    }
} 