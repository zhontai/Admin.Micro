using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 文件接口
/// </summary>
public interface IFileService
{
    Task<PageResponse<FileGetPageResponse>> GetPageAsync(PageRequest<FileGetPageFilter> request);

    Task DeleteAsync(FileDeleteRequest request);

    Task<FileUploadFileResponse> UploadFileAsync(IFormFile file, string fileDirectory = "", bool fileReName = true);

    Task<List<FileUploadFileResponse>> UploadFilesAsync([Required] IFormFileCollection files, string fileDirectory = "", bool fileReName = true);
}