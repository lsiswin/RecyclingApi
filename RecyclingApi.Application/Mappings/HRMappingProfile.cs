using AutoMapper;
using RecyclingApi.Application.DTOs.HR;
using RecyclingApi.Domain.Entities.HR;

namespace RecyclingApi.Application.Mappings
{
    /// <summary>
    /// HR模块的AutoMapper映射配置
    /// </summary>
    public class HRMappingProfile : Profile
    {
        /// <summary>
        /// 构造函数，配置映射关系
        /// </summary>
        public HRMappingProfile()
        {
            // JobPosition 映射
            CreateMap<JobPosition, JobPositionDTO>()
                .ForMember(dest => dest.ApplicationCount, opt => opt.MapFrom(src => src.Applications != null ? src.Applications.Count : 0));
            
            CreateMap<CreateJobPositionDTO, JobPosition>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PostedDate, opt => opt.MapFrom(_ => System.DateTime.Now))
                .ForMember(dest => dest.Applications, opt => opt.Ignore());
            
            CreateMap<UpdateJobPositionDTO, JobPosition>()
                .ForMember(dest => dest.PostedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Applications, opt => opt.Ignore());

            // JobApplication 映射
            CreateMap<JobApplication, JobApplicationDTO>()
                .ForMember(dest => dest.JobPositionTitle, opt => opt.MapFrom(src => src.JobPosition != null ? src.JobPosition.Title : null))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()));
            
            CreateMap<CreateJobApplicationDTO, JobApplication>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => ApplicationStatus.New))
                .ForMember(dest => dest.AppliedDate, opt => opt.MapFrom(_ => System.DateTime.Now))
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(_ => System.DateTime.Now))
                .ForMember(dest => dest.JobPosition, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            
            CreateMap<UpdateJobApplicationStatusDTO, JobApplication>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.Phone, opt => opt.Ignore())
                .ForMember(dest => dest.ResumeUrl, opt => opt.Ignore())
                .ForMember(dest => dest.CoverLetter, opt => opt.Ignore())
                .ForMember(dest => dest.JobPositionId, opt => opt.Ignore())
                .ForMember(dest => dest.JobPosition, opt => opt.Ignore())
                .ForMember(dest => dest.AppliedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(_ => System.DateTime.Now))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // Resume 映射
            CreateMap<Resume, ResumeDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : null));
            
            CreateMap<CreateResumeDTO, Resume>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FileUrl, opt => opt.Ignore()) // 由服务处理文件上传
                .ForMember(dest => dest.FileSize, opt => opt.Ignore()) // 由服务计算文件大小
                .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => System.IO.Path.GetExtension(src.FileName).TrimStart('.')))
                .ForMember(dest => dest.UploadDate, opt => opt.MapFrom(_ => System.DateTime.Now))
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(_ => System.DateTime.Now))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            
            CreateMap<UpdateResumeDTO, Resume>()
                .ForMember(dest => dest.FileUrl, opt => opt.Ignore()) // 由服务处理文件上传
                .ForMember(dest => dest.FileSize, opt => opt.Ignore()) // 由服务计算文件大小
                .ForMember(dest => dest.FileType, opt => opt.Ignore()) // 由服务处理文件类型
                .ForMember(dest => dest.UploadDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedDate, opt => opt.MapFrom(_ => System.DateTime.Now))
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
} 