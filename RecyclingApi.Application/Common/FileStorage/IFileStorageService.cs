using System.Threading.Tasks;

namespace RecyclingApi.Application.Common.FileStorage
{
    /// <summary>
    /// 文件存储服务接口
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="fileData">文件数据</param>
        /// <param name="fileName">文件名</param>
        /// <param name="folderName">文件夹名</param>
        /// <returns>文件URL</returns>
        Task<string> SaveFileAsync(byte[] fileData, string fileName, string folderName);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileUrl">文件URL</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteFileAsync(string fileUrl);
    }
}
