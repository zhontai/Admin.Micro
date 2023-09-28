using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZhonTai.Api.Core;
using ZhonTai.Api.Core.Base;
using ZhonTai.Api.Core.GrpcServices;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using ZhonTai.Api.Rpc.Dtos;
using ZhonTai.Module.Admin.Contracts.Http;
using ZhonTai.Module.App.Api.Core.Consts;
using ZhonTai.Module.App.Api.Domain.Module;
using ZhonTai.Module.App.Api.Domain.Module.Dto;
using ZhonTai.Module.App.Api.Services.Module.Input;
using ZhonTai.Module.App.Api.Services.Module.Output;

namespace ZhonTai.Module.App.Api.Services.Module
{
    /// <summary>
    /// 模块服务
    /// </summary>
    [Order(1010)]
    [DynamicApi(Area = ApiConsts.AreaName)]
    public class ModuleService : BaseService, IModuleService, IDynamicApi
    {
        private IModuleRepository _moduleRepository => LazyGetRequiredService<IModuleRepository>();

        public ModuleService()
        {
        }

        /// <summary>
        /// 查询模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ModuleGetOutput> GetAsync(long id)
        {
            //Grpc测试
            await AppInfo.GetRequiredService<IUserGrpcService>().GetDataPermissionAsync();

            //Http测试
            var res = await AppInfo.GetRequiredService<IUserClientService>().GetPageAsync(
                new PageRequest<UserGetPageFilter>
                {
                    CurrentPage = 1,
                    PageSize = 20
                });

            var result = await _moduleRepository.GetAsync<ModuleGetOutput>(id);

            return result;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResponse<ModuleListOutput>> GetPageAsync(PageRequest<ModuleGetPageDto> input)
        {
            var key = input.Filter?.Name;

            var list = await _moduleRepository.Select
            .WhereIf(key.NotNull(), a => a.Name.Contains(key))
            .Count(out var total)
            .OrderByDescending(true, c => c.Id)
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync<ModuleListOutput>();

            var data = new PageResponse<ModuleListOutput>()
            {
                List = list,
                Total = total
            };

            return data;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<long> AddAsync(ModuleAddInput input)
        {
            var entity = Mapper.Map<ModuleEntity>(input);
            await _moduleRepository.InsertAsync(entity);

            return entity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ModuleUpdateInput input)
        {
            var entity = await _moduleRepository.GetAsync(input.Id);
            if (!(entity?.Id > 0))
            {
                throw Response.Exception("模块不存在");
            }

            Mapper.Map(input, entity);
            await _moduleRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long id)
        {
            await _moduleRepository.DeleteAsync(m => m.Id == id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SoftDeleteAsync(long id)
        {
            await _moduleRepository.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task BatchSoftDeleteAsync(long[] ids)
        {
            await _moduleRepository.SoftDeleteAsync(ids);
        }

    }
}