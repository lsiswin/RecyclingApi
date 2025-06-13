using System;
using System.IO;
using System.Threading.Tasks;

namespace RecyclingApi.Application.Common.FileStorage
{
    /// <summary>
    /// 本地文件存储服务实现
    /// </summary>
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _rootPath;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LocalFileStorageService()
        {
            _rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
        }

        /// <summary>
        /// 保存文件到本地
        /// </summary>
        public async Task<string> SaveFileAsync(byte[] fileData, string fileName, string folderName)
        {
            // 生成文件URL（使用GUID避免文件名冲突）
            var fileExt = Path.GetExtension(fileName);
            var fileUrl = $"/uploads/{folderName}/{Guid.NewGuid()}{fileExt}";
            
            // 确保目录存在
            var uploadDir = Path.Combine(_rootPath, "uploads", folderName);
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            // 保存文件
            var filePath = Path.Combine(_rootPath, fileUrl.TrimStart('/'));
            await File.WriteAllBytesAsync(filePath, fileData);

            return fileUrl;
        }

        /// <summary>
        /// 从本地删除文件
        /// </summary>
        public Task<bool> DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return Task.FromResult(false);

            var filePath = Path.Combine(_rootPath, fileUrl.TrimStart('/'));
            if (!File.Exists(filePath))
                return Task.FromResult(false);

            try
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}
