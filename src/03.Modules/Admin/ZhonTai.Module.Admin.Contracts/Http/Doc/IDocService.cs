using Microsoft.AspNetCore.Mvc;

namespace ZhonTai.Module.Admin.Contracts.Http;

/// <summary>
/// 文档接口
/// </summary>
public partial interface IDocService
{
    Task<List<string>> GetImageListAsync(long id);

    Task<DocGetGroupResponse> GetGroupAsync(long id);

    Task<DocGetMenuResponse> GetMenuAsync(long id);

    Task<DocGetContentResponse> GetContentAsync(long id);

    Task<IEnumerable<dynamic>> GetPlainListAsync();

    Task<List<DocListResponse>> GetListAsync(string key, DateTime? start, DateTime? end);

    Task<long> AddGroupAsync(DocAddGroupRequest input);

    Task<long> AddMenuAsync(DocAddMenuRequest input);

    Task<long> AddImageAsync(DocAddImageRequest input);

    Task UpdateGroupAsync(DocUpdateGroupRequest input);

    Task UpdateMenuAsync(DocUpdateMenuRequest input);

    Task UpdateContentAsync(DocUpdateContentRequest input);

    Task DeleteAsync(long id);

    Task DeleteImageAsync(long documentId, string url);

    Task SoftDeleteAsync(long id);

    Task<string> UploadImage([FromForm] DocUploadImageRequest input);
}