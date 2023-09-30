﻿using ZhonTai.Module.Admin.HttpApi.Domain.Org;
using ZhonTai.Api.Core.Consts;
using ZhonTai.Api.DynamicApi.Attributes;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Module.Admin.HttpApi.Domain.RoleOrg;
using ZhonTai.Module.Admin.HttpApi.Domain.UserOrg;
using ZhonTai.Api.Core;
using ZhonTai.Api.Rpc.Enums;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 部门服务
/// </summary>
[Order(30)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class OrgService : BaseService, IOrgService, IDynamicApi
{
    private IOrgRepository _orgRepository => LazyGetRequiredService<IOrgRepository>();
    private IUserOrgRepository _userOrgRepository => LazyGetRequiredService<IUserOrgRepository>();
    private IRoleOrgRepository _roleOrgRepository => LazyGetRequiredService<IRoleOrgRepository>();

    public OrgService()
    {
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<OrgGetResponse> GetAsync(long id)
    {
        var result = await _orgRepository.GetAsync<OrgGetResponse>(id);
        return result;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<List<OrgGetListResponse>> GetListAsync(string key)
    {
        var dataPermission = await AppInfo.GetRequiredService<IUserService>().GetDataPermissionAsync();
        var hasOrg = dataPermission.OrgIds.Count > 0;

        var select = _orgRepository.Select
            .WhereIf(hasOrg, a => dataPermission.OrgIds.Contains(a.Id))
            .WhereIf(dataPermission.DataScope == DataScope.Self, a => a.CreatedUserId == User.Id)
            .WhereIf(key.NotNull(), a => a.Name.Contains(key) || a.Code.Contains(key));

        if (hasOrg)
        {
            select = select.AsTreeCte(up: true);
        }

        var data = await select
            .OrderBy(a => a.ParentId)
            .OrderBy(a => a.Sort)
            .ToListAsync<OrgGetListResponse>();

        return hasOrg ? data.DistinctBy(a => a.Id).OrderBy(a => a.ParentId).ThenBy(a => a.Sort).ToList() : data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(OrgAddRequest request)
    {
        if(request.ParentId == 0)
        {
            throw Response.Exception($"请选择上级部门");
        }

        if (await _orgRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Name == request.Name))
        {
            throw Response.Exception($"此部门已存在");
        }

        if (request.Code.NotNull() && await _orgRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Code == request.Code))
        {
            throw Response.Exception($"此部门编码已存在");
        }

        var entity = Mapper.Map<OrgEntity>(request);

        if (entity.Sort == 0)
        {
            var sort = await _orgRepository.Select.Where(a => a.ParentId == request.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _orgRepository.InsertAsync(entity);
        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task UpdateAsync(OrgUpdateRequest request)
    {
        if (request.ParentId == 0)
        {
            throw Response.Exception($"请选择上级部门");
        }

        var entity = await _orgRepository.GetAsync(request.Id);
        if (!(entity?.Id > 0))
        {
            throw Response.Exception("部门不存在");
        }

        if (request.Id == request.ParentId)
        {
            throw Response.Exception("上级部门不能是本部门");
        }

        if (await _orgRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Id != request.Id && a.Name == request.Name))
        {
            throw Response.Exception($"此部门已存在");
        }

        if (request.Code.NotNull() && await _orgRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Id != request.Id && a.Code == request.Code))
        {
            throw Response.Exception($"此部门编码已存在");
        }

        var childIdList = await _orgRepository.GetChildIdListAsync(request.Id);
        if (childIdList.Contains(request.ParentId))
        {
            throw Response.Exception($"上级部门不能是下级部门");
        }

        Mapper.Map(request, entity);
        await _orgRepository.UpdateAsync(entity);

        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");
    }

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public async Task DeleteAsync(long id)
    {
        //本部门下是否有员工
        if(await _userOrgRepository.HasUser(id))
        {
            throw Response.Exception($"当前部门有员工无法删除");
        }

        var orgIdList = await _orgRepository.GetChildIdListAsync(id);
        //本部门的下级部门下是否有员工
        if (await _userOrgRepository.HasUser(orgIdList))
        {
            throw Response.Exception($"本部门的下级部门有员工无法删除");
        }

        //删除部门角色
        await _roleOrgRepository.DeleteAsync(a => orgIdList.Contains(a.OrgId));

        //删除本部门和下级部门
        await _orgRepository.DeleteAsync(a => orgIdList.Contains(a.Id));

        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public async Task SoftDeleteAsync(long id)
    {
        //本部门下是否有员工
        if (await _userOrgRepository.HasUser(id))
        {
            throw Response.Exception($"当前部门有员工无法删除");
        }

        var orgIdList = await _orgRepository.GetChildIdListAsync(id);
        //本部门的下级部门下是否有员工
        if (await _userOrgRepository.HasUser(orgIdList))
        {
            throw Response.Exception($"本部门的下级部门有员工无法删除");
        }

        //删除部门角色
        await _roleOrgRepository.SoftDeleteAsync(a => orgIdList.Contains(a.OrgId));

        //删除本部门和下级部门
        await _orgRepository.SoftDeleteAsync(a => orgIdList.Contains(a.Id));

        await Cache.DelByPatternAsync(CacheKeys.DataPermission + "*");
    }
}