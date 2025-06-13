using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;
using AutoMapper;
using RecyclingApi.Application.DTOs;
using RecyclingApi.Application.DTOs.HR;
using RecyclingApi.Application.Repositories;
using RecyclingApi.Domain.Entities.HR;
using RecyclingApi.Application.Common.Exceptions;
using RecyclingApi.Application.Common.FileStorage;

namespace RecyclingApi.Application.Services.HR
{
    /// <summary>
    /// 简历服务实现
    /// </summary>
    public class ResumeService : IResumeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ResumeService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// 获取简历列表（分页）
        /// </summary>
        public async Task<PagedResponseDto<ResumeDTO>> GetResumesAsync(ResumeFilterDTO filter)
        {
            var repository = _unitOfWork.GetRepository<Resume>();
            Expression<Func<Resume, bool>> predicate = r => true;

            // 构建查询条件
            if (!string.IsNullOrEmpty(filter.Title))
            {
                predicate = CombineExpressions(predicate, r => r.Title.Contains(filter.Title));
            }

            if (!string.IsNullOrEmpty(filter.UserName))
            {
                predicate = CombineExpressions(predicate, r => 
                    (r.User != null && r.User.UserName.Contains(filter.UserName)) ||
                    (r.User != null && r.User.RealName.Contains(filter.UserName)));
            }

            if (!string.IsNullOrEmpty(filter.Skills))
            {
                predicate = CombineExpressions(predicate, r => r.Skills.Contains(filter.Skills));
            }

            if (filter.UploadDateFrom.HasValue)
            {
                predicate = CombineExpressions(predicate, r => r.UploadDate >= filter.UploadDateFrom.Value);
            }

            if (filter.UploadDateTo.HasValue)
            {
                predicate = CombineExpressions(predicate, r => r.UploadDate <= filter.UploadDateTo.Value);
            }

            if (filter.IsActive.HasValue)
            {
                predicate = CombineExpressions(predicate, r => r.IsActive == filter.IsActive.Value);
            }

            // 执行查询
            var result = await repository.GetPagedListAsync(
                filter.PageIndex,
                filter.PageSize,
                predicate,
                r => r.UploadDate,
                true,
                "User");

            // 映射为DTO
            var dtos = _mapper.Map<List<ResumeDTO>>(result.Items);

            return new PagedResponseDto<ResumeDTO>
            {
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = (int)Math.Ceiling(result.TotalCount / (double)filter.PageSize),
                Items = dtos
            };
        }

        /// <summary>
        /// 根据ID获取简历
        /// </summary>
        public async Task<ResumeDTO> GetResumeByIdAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Resume>();
            var resume = await repository.GetByIdAsync(id, "User");

            if (resume == null)
                throw new NotFoundException($"简历ID {id} 未找到");

            return _mapper.Map<ResumeDTO>(resume);
        }

        /// <summary>
        /// 获取用户的所有简历
        /// </summary>
        public async Task<List<ResumeDTO>> GetResumesByUserIdAsync(string userId)
        {
            var repository = _unitOfWork.GetRepository<Resume>();
            var resumes = await repository.GetAsync(
                r => r.UserId == userId,
                "User");

            var orderedResumes = resumes.OrderByDescending(r => r.UploadDate);
            return _mapper.Map<List<ResumeDTO>>(orderedResumes);
        }

        /// <summary>
        /// 创建简历
        /// </summary>
        public async Task<int> CreateResumeAsync(CreateResumeDTO dto, string userId)
        {
            // 处理文件
            var (fileUrl, fileSize) = await ProcessResumeFile(dto.ResumeFileBase64, dto.FileName);
            
            var repository = _unitOfWork.GetRepository<Resume>();
            var resume = new Resume
            {
                Title = dto.Title,
                FileUrl = fileUrl,
                FileName = dto.FileName,
                FileSize = fileSize,
                FileType = Path.GetExtension(dto.FileName).TrimStart('.'),
                Skills = dto.Skills,
                Education = dto.Education,
                WorkExperience = dto.WorkExperience,
                AdditionalInfo = dto.AdditionalInfo,
                UploadDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                IsActive = true,
                UserId = userId
            };

            repository.Add(resume);
            await _unitOfWork.SaveChangesAsync();

            return resume.Id;
        }

        /// <summary>
        /// 更新简历
        /// </summary>
        public async Task<bool> UpdateResumeAsync(UpdateResumeDTO dto)
        {
            var repository = _unitOfWork.GetRepository<Resume>();
            var resume = await repository.GetByIdAsync(dto.Id);
            
            if (resume == null)
                throw new NotFoundException($"简历ID {dto.Id} 未找到");

            resume.Title = dto.Title;
            resume.Skills = dto.Skills;
            resume.Education = dto.Education;
            resume.WorkExperience = dto.WorkExperience;
            resume.AdditionalInfo = dto.AdditionalInfo;
            resume.IsActive = dto.IsActive;
            resume.LastUpdatedDate = DateTime.Now;

            // 如果上传了新文件
            if (!string.IsNullOrEmpty(dto.ResumeFileBase64))
            {
                // 删除旧文件
                await _fileStorageService.DeleteFileAsync(resume.FileUrl);

                // 处理新文件
                var (fileUrl, fileSize) = await ProcessResumeFile(dto.ResumeFileBase64, dto.FileName);

                // 更新文件信息
                resume.FileUrl = fileUrl;
                resume.FileName = dto.FileName;
                resume.FileSize = fileSize;
                resume.FileType = Path.GetExtension(dto.FileName).TrimStart('.');
            }

            repository.Update(resume);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 删除简历
        /// </summary>
        public async Task<bool> DeleteResumeAsync(int id)
        {
            var repository = _unitOfWork.GetRepository<Resume>();
            var resume = await repository.GetByIdAsync(id);
            
            if (resume == null)
                throw new NotFoundException($"简历ID {id} 未找到");

            // 删除文件
            await _fileStorageService.DeleteFileAsync(resume.FileUrl);

            repository.Delete(resume);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 更改简历状态
        /// </summary>
        public async Task<bool> ChangeResumeStatusAsync(int id, bool isActive)
        {
            var repository = _unitOfWork.GetRepository<Resume>();
            var resume = await repository.GetByIdAsync(id);
            
            if (resume == null)
                throw new NotFoundException($"简历ID {id} 未找到");

            resume.IsActive = isActive;
            resume.LastUpdatedDate = DateTime.Now;

            repository.Update(resume);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        #region 私有辅助方法

        /// <summary>
        /// 处理简历文件
        /// </summary>
        private async Task<(string fileUrl, long fileSize)> ProcessResumeFile(string base64File, string fileName)
        {
            var fileData = Convert.FromBase64String(base64File.Split(',')[1]);
            var fileUrl = await _fileStorageService.SaveFileAsync(fileData, fileName, "resumes");
            return (fileUrl, fileData.Length);
        }

        /// <summary>
        /// 合并两个表达式树
        /// </summary>
        private static Expression<Func<T, bool>> CombineExpressions<T>(
            Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left!, right!), parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression? Visit(Expression? node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }

        #endregion
    }
}