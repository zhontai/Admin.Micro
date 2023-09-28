using Microsoft.AspNetCore.Mvc;
using ZhonTai.Module.Admin.HttpApi.Domain.Doc;
using ZhonTai.Module.Admin.HttpApi.Domain.DocImage;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using ZhonTai.Api.Core.Consts;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 文档服务
/// </summary>
[Order(180)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class DocService : BaseService, IDocService, IDynamicApi
{
    private readonly IDocRepository _docRepository;
    private readonly IDocImageRepository _docImageRepository;

    public DocService(
        IDocRepository DocRepository,
        IDocImageRepository docImageRepository
    )
    {
        _docRepository = DocRepository;
        _docImageRepository = docImageRepository;
    }

    /// <summary>
    /// 查询分组
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DocGetGroupResponse> GetGroupAsync(long id)
    {
        var result = await _docRepository.GetAsync<DocGetGroupResponse>(id);
        return result;
    }

    /// <summary>
    /// 查询菜单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DocGetMenuResponse> GetMenuAsync(long id)
    {
        var result = await _docRepository.GetAsync<DocGetMenuResponse>(id);
        return result;
    }

    /// <summary>
    /// 查询文档内容
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DocGetContentResponse> GetContentAsync(long id)
    {
        var result = await _docRepository.GetAsync<DocGetContentResponse>(id);
        return result;
    }

    /// <summary>
    /// 查询文档列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public async Task<List<DocListResponse>> GetListAsync(string key, DateTime? start, DateTime? end)
    {
        if (end.HasValue)
        {
            end = end.Value.AddDays(1);
        }

        var data = await _docRepository
            .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Label.Contains(key))
            .WhereIf(start.HasValue && end.HasValue, a => a.CreatedTime.Value.BetweenEnd(start.Value, end.Value))
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync<DocListResponse>();

        return data;
    }

    /// <summary>
    /// 查询图片列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<List<string>> GetImageListAsync(long id)
    {
        var result = await _docImageRepository.Select
            .Where(a => a.DocId == id)
            .OrderByDescending(a => a.Id)
            .ToListAsync(a => a.Url);

        return result;
    }

    /// <summary>
    /// 新增分组
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<long> AddGroupAsync(DocAddGroupRequest request)
    {
        var entity = Mapper.Map<DocEntity>(request);
        await _docRepository.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 新增菜单
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<long> AddMenuAsync(DocAddMenuRequest request)
    {
        var entity = Mapper.Map<DocEntity>(request);
        await _docRepository.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 新增图片
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<long> AddImageAsync(DocAddImageRequest request)
    {
        var entity = Mapper.Map<DocImageEntity>(request);
        await _docImageRepository.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改分组
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task UpdateGroupAsync(DocUpdateGroupRequest request)
    {
        var entity = await _docRepository.GetAsync(request.Id);
        entity = Mapper.Map(request, entity);
        await _docRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改菜单
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task UpdateMenuAsync(DocUpdateMenuRequest request)
    {
        var entity = await _docRepository.GetAsync(request.Id);
        entity = Mapper.Map(request, entity);
        await _docRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改文档内容
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task UpdateContentAsync(DocUpdateContentRequest request)
    {
        var entity = await _docRepository.GetAsync(request.Id);
        entity = Mapper.Map(request, entity);
        await _docRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除文档
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task DeleteAsync(long id)
    {
        await _docRepository.DeleteAsync(m => m.Id == id);
    }

    /// <summary>
    /// 彻底删除图片
    /// </summary>
    /// <param name="docId"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task DeleteImageAsync(long docId, string url)
    {
        await _docImageRepository.DeleteAsync(m => m.DocId == docId && m.Url == url);
    }

    /// <summary>
    /// 删除文档
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task SoftDeleteAsync(long id)
    {
        await _docRepository.SoftDeleteAsync(id);
    }

    /// <summary>
    /// 查询精简文档列表
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<dynamic>> GetPlainListAsync()
    {
        var docs = await _docRepository.Select
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync(a => new { a.Id, a.ParentId, a.Label, a.Type, a.Opened });

        var menus = docs
            .Where(a => (new[] { DocType.Group, DocType.Markdown }).Contains(a.Type))
            .Select(a => new
            {
                a.Id,
                a.ParentId,
                a.Label,
                a.Type,
                a.Opened
            });

        return menus;
    }

    /// <summary>
    /// 上传文档图片
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> UploadImage([FromForm] DocUploadImageRequest request)
    {
        var fileService = LazyGetRequiredService<FileService>();
        var file = await fileService.UploadFileAsync(request.File);
        //保存文档图片
        await AddImageAsync(new DocAddImageRequest
        {
            DocId = request.Id,
            Url = file.LinkUrl
        });
        return file.LinkUrl;
    }
}