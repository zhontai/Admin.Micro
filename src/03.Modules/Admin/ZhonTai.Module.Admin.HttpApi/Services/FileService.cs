using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Api.Core.Consts;
using ZhonTai.Module.Admin.HttpApi.Domain.File;
using Microsoft.AspNetCore.Http;
using OnceMi.AspNetCore.OSS;
using Microsoft.Extensions.Options;
using ZhonTai.Utils.Files;
using ZhonTai.Utils.Helpers;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Hosting;
using ZhonTai.Module.Admin.HttpApi.Core.Configs;
using Mapster;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 文件服务
/// </summary>
[Order(110)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class FileService : BaseService, IFileService, IDynamicApi
{
    private IFileRepository _fileRepository => LazyGetRequiredService<IFileRepository>();
    private IOSSServiceFactory _oSSServiceFactory => LazyGetRequiredService<IOSSServiceFactory>();
    private OSSConfig _oSSConfig => LazyGetRequiredService<IOptions<OSSConfig>>().Value;
    private IHttpContextAccessor _httpContextAccessor => LazyGetRequiredService<IHttpContextAccessor>();

    public FileService()
    {
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResponse<FileGetPageResponse>> GetPageAsync(PageRequest<FileGetPageFilter> request)
    {
        var fileName = request.Filter?.FileName;

        var list = await _fileRepository.Select
        .WhereDynamicFilter(request.DynamicFilter)
        .WhereIf(fileName.NotNull(), a => a.FileName.Contains(fileName))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(request.CurrentPage, request.PageSize)
        .ToListAsync(a => new FileGetPageResponse { ProviderName = a.Provider.ToString() });

        var data = new PageResponse<FileGetPageResponse>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task DeleteAsync(FileDeleteRequest request)
    {
        var file = await _fileRepository.GetAsync(request.Id);
        if (file == null)
        {
            return;
        }

        var shareFile = await _fileRepository.Where(a=>a.Id != request.Id && a.LinkUrl == file.LinkUrl).AnyAsync();
        if (!shareFile)
        {
            if(file.Provider.HasValue)
            {
                var oSSService = _oSSServiceFactory.Create(file.Provider.ToString());
                var oSSOptions = _oSSConfig.OSSConfigs.Where(a => a.Enable && a.Provider == file.Provider).FirstOrDefault();
                var enableOss = oSSOptions != null && oSSOptions.Enable;
                if (enableOss)
                {
                    var filePath = Path.Combine(file.FileDirectory, file.SaveFileName + file.Extension).ToPath();
                    await oSSService.RemoveObjectAsync(file.BucketName, filePath);
                }
            }
            else
            {
                var env = LazyGetRequiredService<IWebHostEnvironment>();
                var filePath = Path.Combine(env.WebRootPath, file.FileDirectory, file.SaveFileName + file.Extension).ToPath();
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        await _fileRepository.DeleteAsync(file.Id);
    }

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="file">文件</param>
    /// <param name="fileDirectory">文件目录</param>
    /// <param name="fileReName">文件重命名</param>
    /// <returns></returns>
    public async Task<FileUploadFileResponse> UploadFileAsync([Required] IFormFile file, string fileDirectory = "", bool fileReName = true)
    {
        var localUploadConfig = _oSSConfig.LocalUploadConfig;

        var extention = Path.GetExtension(file.FileName).ToLower();
        var hasIncludeExtension = localUploadConfig.IncludeExtension?.Length > 0;
        if(hasIncludeExtension && !localUploadConfig.IncludeExtension.Contains(extention))
        {
            throw new Exception($"不允许上传{extention}文件格式");
        }
        var hasExcludeExtension = localUploadConfig.ExcludeExtension?.Length > 0;
        if (hasExcludeExtension && localUploadConfig.ExcludeExtension.Contains(extention))
        {
            throw new Exception($"不允许上传{extention}文件格式");
        }

        var fileLenth = file.Length;
        if(fileLenth > localUploadConfig.MaxSize) 
        {
            throw new Exception($"文件大小不能超过{new FileSize(localUploadConfig.MaxSize)}");
        }
       
        var oSSOptions = _oSSConfig.OSSConfigs.Where(a => a.Enable && a.Provider == _oSSConfig.Provider).FirstOrDefault();
        var enableOss = oSSOptions != null && oSSOptions.Enable;
        var enableMd5 = enableOss ? oSSOptions.Md5 : localUploadConfig.Md5;
        var md5 = string.Empty;
        if (enableMd5)
        {
            md5 = MD5Encrypt.GetHash(file.OpenReadStream());
            var md5FileEntity = await _fileRepository.WhereIf(enableOss, a => a.Provider == oSSOptions.Provider).Where(a => a.Md5 == md5).FirstAsync();
            if (md5FileEntity != null)
            {
                var sameFileEntity = new FileEntity
                {
                    Provider = md5FileEntity.Provider,
                    BucketName = md5FileEntity.BucketName,
                    FileGuid = FreeUtil.NewMongodbId(),
                    SaveFileName = md5FileEntity.SaveFileName,
                    FileName = Path.GetFileNameWithoutExtension(file.FileName),
                    Extension = extention,
                    FileDirectory = md5FileEntity.FileDirectory,
                    Size = md5FileEntity.Size,
                    SizeFormat = md5FileEntity.SizeFormat,
                    LinkUrl = md5FileEntity.LinkUrl,
                    Md5 = md5,
                };
                sameFileEntity = await _fileRepository.InsertAsync(sameFileEntity);
                return sameFileEntity.Adapt<FileUploadFileResponse>();
            }
        }

        if (fileDirectory.IsNull())
        {
            fileDirectory = localUploadConfig.Directory;
            if (localUploadConfig.DateTimeDirectory.NotNull())
            {
                fileDirectory = Path.Combine(fileDirectory, DateTime.Now.ToString(localUploadConfig.DateTimeDirectory)).ToPath();
            }
        }

        var fileSize = new FileSize(fileLenth);
        var fileEntity = new FileEntity
        {
            Provider = oSSOptions?.Provider,
            BucketName = oSSOptions?.BucketName,
            FileGuid = FreeUtil.NewMongodbId(),
            FileName = Path.GetFileNameWithoutExtension(file.FileName),
            Extension = extention,
            FileDirectory = fileDirectory,
            Size = fileSize.Size,
            SizeFormat = fileSize.ToString(),
            Md5 = md5
        };
        fileEntity.SaveFileName = fileReName ? fileEntity.FileGuid.ToString() : fileEntity.FileName;

        var filePath = Path.Combine(fileDirectory, fileEntity.SaveFileName + fileEntity.Extension).ToPath();
        var url = string.Empty;
        if (enableOss)
        {
            url = oSSOptions.Url;
            if (url.IsNull())
            {
                url = oSSOptions.Provider switch
                {
                    OSSProvider.Minio => $"{oSSOptions.Endpoint}/{oSSOptions.BucketName}",
                    OSSProvider.Aliyun => $"{oSSOptions.BucketName}.{oSSOptions.Endpoint}",
                    OSSProvider.QCloud => $"{oSSOptions.BucketName}-{oSSOptions.Endpoint}.cos.{oSSOptions.Region}.myqcloud.com",
                    OSSProvider.Qiniu => $"{oSSOptions.BucketName}.{oSSOptions.Region}.qiniucs.com",
                    OSSProvider.HuaweiCloud => $"{oSSOptions.BucketName}.{oSSOptions.Endpoint}",
                    _ => ""
                };

                if (url.IsNull())
                {
                    throw Response.Exception($"请配置{oSSOptions.Provider}的Url参数");
                }

                var urlProtocol = oSSOptions.IsEnableHttps ? "https" : "http";
                fileEntity.LinkUrl = $"{urlProtocol}://{url}/{filePath}";
            }
            else
            {
                fileEntity.LinkUrl = $"{url}/{filePath}";
            }
        }
        else
        {
            fileEntity.LinkUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/{filePath}";
        }

        if (enableOss)
        {
            var oSSService = _oSSServiceFactory.Create(_oSSConfig.Provider.ToString());
            await oSSService.PutObjectAsync(oSSOptions.BucketName, filePath, file.OpenReadStream());
        }
        else
        {
            var uploadHelper = base.LazyGetRequiredService<ZhonTai.Api.Core.Helpers.FileHelper>();
            var env = LazyGetRequiredService<IWebHostEnvironment>();
            fileDirectory = Path.Combine(env.WebRootPath, fileDirectory).ToPath();
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            filePath = Path.Combine(env.WebRootPath, filePath).ToPath();
            await uploadHelper.SaveAsync(file, filePath);
        }
       
        fileEntity = await _fileRepository.InsertAsync(fileEntity);

        return fileEntity.Adapt<FileUploadFileResponse>();
    }

    /// <summary>
    /// 上传多文件
    /// </summary>
    /// <param name="files">文件列表</param>
    /// <param name="fileDirectory">文件目录</param>
    /// <param name="fileReName">文件重命名</param>
    /// <returns></returns>
    public async Task<List<FileUploadFileResponse>> UploadFilesAsync([Required] IFormFileCollection files, string fileDirectory = "", bool fileReName = true)
    {
        var fileList = new List<FileUploadFileResponse>();
        foreach (var file in files)
        {
            fileList.Add(await UploadFileAsync(file, fileDirectory, fileReName));
        }
        return fileList;
    }
}