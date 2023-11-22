using System.Threading.Tasks;
using ZhonTai.Api.Rpc.Dtos;
using ZhonTai.Module.App.Api.Domain.Module.Dto;
using ZhonTai.Module.App.Api.Services.Module.Input;
using ZhonTai.Module.App.Api.Services.Module.Output;

namespace ZhonTai.Module.App.Api.Services.Module
{
    /// <summary>
    /// 模块接口
    /// </summary>
    public interface IModuleService
    {
        Task<ModuleGetOutput> GetAsync(long id);

        Task<PageResponse<ModuleListOutput>> GetPageAsync(PageRequest<ModuleGetPageDto> input);

        Task<long> AddAsync(ModuleAddInput input);

        Task UpdateAsync(ModuleUpdateInput input);

        Task DeleteAsync(long id);

        Task SoftDeleteAsync(long id);

        Task BatchSoftDeleteAsync(long[] ids);
    }
}