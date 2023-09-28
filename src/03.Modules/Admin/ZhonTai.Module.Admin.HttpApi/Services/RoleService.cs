using ZhonTai.Module.Admin.HttpApi.Domain.Role;
using ZhonTai.Module.Admin.HttpApi.Domain.RolePermission;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Api.Core.Consts;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Module.Admin.HttpApi.Domain.UserRole;
using ZhonTai.Module.Admin.HttpApi.Domain.User;
using ZhonTai.Module.Admin.HttpApi.Domain;
using ZhonTai.Module.Admin.HttpApi.Domain.Org;
using ZhonTai.Module.Admin.HttpApi.Domain.RoleOrg;
using ZhonTai.Api.Core.Enums;
using ZhonTai.Api.Rpc.Enums;

namespace ZhonTai.Module.Admin.HttpApi.Services.Role;

/// <summary>
/// 角色服务
/// </summary>
[Order(20)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class RoleService : BaseService, IRoleService, IDynamicApi
{
    private IRoleRepository _roleRepository => LazyGetRequiredService<IRoleRepository>();
    private IUserRepository _userRepository => LazyGetRequiredService<IUserRepository>();
    private IUserRoleRepository _userRoleRepository => LazyGetRequiredService<IUserRoleRepository>();
    private IRolePermissionRepository _rolePermissionRepository => LazyGetRequiredService<IRolePermissionRepository>();
    private IRoleOrgRepository _roleOrgRepository => LazyGetRequiredService<IRoleOrgRepository>();

    public RoleService()
    {
    }

    /// <summary>
    /// 添加角色部门
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="orgIds"></param>
    /// <returns></returns>
    private async Task AddRoleOrgAsync(long roleId, List<long> orgIds)
    {
        if (orgIds != null && orgIds.Any())
        {
            var roleOrgs = orgIds.Select(orgId => new RoleOrgEntity
            {
                RoleId = roleId,
                OrgId = orgId
            }).ToList();
            await _roleOrgRepository.InsertAsync(roleOrgs);
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<RoleGetResponse> GetAsync(long id)
    {
        var roleGetResponse = await _roleRepository.Select
        .WhereDynamic(id)
        .ToOneAsync<RoleGetResponse>();

        roleGetResponse.OrgIds = await _roleOrgRepository.Select.Where(a=>a.RoleId == id).ToListAsync(a=>a.OrgId);

        return roleGetResponse;
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<List<RoleGetListResponse>> GetListAsync([FromQuery]RoleGetListRequest request)
    {
        var list = await _roleRepository.Select
        .WhereIf(request.Name.NotNull(), a => a.Name.Contains(request.Name))
        .OrderBy(a => new { a.ParentId, a.Sort })
        .ToListAsync<RoleGetListResponse>();

        return list;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResponse<RoleGetPageResponse>> GetPageAsync(PageRequest<RoleGetPageFilter> request)
    {
        var key = request.Filter?.Name;

        var list = await _roleRepository.Select
        .WhereDynamicFilter(request.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(request.CurrentPage, request.PageSize)
        .ToListAsync<RoleGetPageResponse>();

        var data = new PageResponse<RoleGetPageResponse>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 查询角色用户列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<List<RoleGetRoleUserListResponse>> GetRoleUserListAsync([FromQuery] RoleGetRoleUserListRequest request)
    {
        var list = await _userRepository.Select.From<UserRoleEntity>()
            .InnerJoin(a => a.t2.UserId == a.t1.Id)
            .Where(a => a.t2.RoleId == request.RoleId)
            .WhereIf(request.Name.NotNull(), a => a.t1.Name.Contains(request.Name))
            .OrderByDescending(a => a.t1.Id)
            .ToListAsync<RoleGetRoleUserListResponse>();

        return list;
    }

    /// <summary>
    /// 添加角色用户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task AddRoleUserAsync(RoleAddRoleUserListRequest request)
    {
        var roleId = request.RoleId;
        var userIds = await _userRoleRepository.Select.Where(a => a.RoleId == roleId).ToListAsync(a => a.UserId);
        var insertUserIds = request.UserIds.Except(userIds);
        if (insertUserIds != null && insertUserIds.Any())
        {
            var userRoleList = insertUserIds.Select(userId => new UserRoleEntity 
            { 
                UserId = userId, 
                RoleId = roleId 
            }).ToList();
            await _userRoleRepository.InsertAsync(userRoleList);
        }

        var clearUserIds = userIds.Concat(request.UserIds).Distinct();
        foreach (var userId in clearUserIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 移除角色用户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task RemoveRoleUserAsync(RoleAddRoleUserListRequest request)
    {
        var userIds = request.UserIds;
        if (userIds != null && userIds.Any())
        {
            await _userRoleRepository.Where(a => a.RoleId == request.RoleId && request.UserIds.Contains(a.UserId)).ToDelete().ExecuteAffrowsAsync();
        }

        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(RoleAddRequest request)
    {
        if (await _roleRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Name == request.Name))
        {
            throw Response.Exception($"此{(request.Type == RoleType.Group ? "分组" : "角色")}已存在");
        }

        if (request.Code.NotNull() && await _roleRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Code == request.Code))
        {
            throw Response.Exception($"此{(request.Type == RoleType.Group ? "分组" : "角色")}编码已存在");
        }

        var entity = Mapper.Map<RoleEntity>(request);
        if (entity.Sort == 0)
        {
            var sort = await _roleRepository.Select.Where(a => a.ParentId == request.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _roleRepository.InsertAsync(entity);
        if (request.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, request.OrgIds);
        }

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task UpdateAsync(RoleUpdateRequest request)
    {
        var entity = await _roleRepository.GetAsync(request.Id);
        if (!(entity?.Id > 0))
        {
            throw Response.Exception("角色不存在");
        }

        if (await _roleRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Id != request.Id && a.Name == request.Name))
        {
            throw Response.Exception($"此{(request.Type == RoleType.Group ? "分组" : "角色")}已存在");
        }

        if (request.Code.NotNull() && await _roleRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Id != request.Id && a.Code == request.Code))
        {
            throw Response.Exception($"此{(request.Type == RoleType.Group ? "分组" : "角色")}编码已存在");
        }

        Mapper.Map(request, entity);
        await _roleRepository.UpdateAsync(entity);
        await _roleOrgRepository.DeleteAsync(a => a.RoleId == entity.Id);
        if (request.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, request.OrgIds);
        }

        var userIds = await _userRoleRepository.Select.Where(a => a.RoleId == entity.Id).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }    

    /// <summary>
    /// 彻底删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var roleIdList = await _roleRepository.GetChildIdListAsync(id);
        var userIds = await _userRoleRepository.Select.Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);

        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => a.UserId == id);
        //删除角色权限
        await _rolePermissionRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //删除角色
        await _roleRepository.DeleteAsync(a => roleIdList.Contains(a.Id));
        
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var roleIdList = await _roleRepository.GetChildIdListAsync(ids);
        var userIds = await _userRoleRepository.Select.Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);

        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //删除角色权限
        await _rolePermissionRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        //删除角色
        await _roleRepository.Where(a => roleIdList.Contains(a.Id)).AsTreeCte().ToDelete().ExecuteAffrowsAsync();

        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        var roleIdList = await _roleRepository.GetChildIdListAsync(id);
        var userIds = await _userRoleRepository.Select.Where(a => roleIdList.Contains(a.RoleId)).ToListAsync(a => a.UserId);
        await _userRoleRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _rolePermissionRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _roleRepository.SoftDeleteRecursiveAsync(a => roleIdList.Contains(a.Id));
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var roleIdList = await _roleRepository.GetChildIdListAsync(ids);
        var userIds = await _userRoleRepository.Select.Where(a => ids.Contains(a.RoleId)).ToListAsync(a => a.UserId);
        await _userRoleRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _rolePermissionRepository.DeleteAsync(a => roleIdList.Contains(a.RoleId));
        await _roleRepository.SoftDeleteRecursiveAsync(a => roleIdList.Contains(a.Id));
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 设置数据权限
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task SetDataScopeAsync(RoleSetDataScopeRequest request)
    {
        var entity = await _roleRepository.GetAsync(request.RoleId);
        if (!(entity?.Id > 0))
        {
            throw Response.Exception("角色不存在");
        }

        Mapper.Map(request, entity);
        await _roleRepository.UpdateAsync(entity);
        await _roleOrgRepository.DeleteAsync(a => a.RoleId == entity.Id);
        if (request.DataScope == DataScope.Custom)
        {
            await AddRoleOrgAsync(entity.Id, request.OrgIds);
        }

        var userIds = await _userRoleRepository.Select.Where(a => a.RoleId == entity.Id).ToListAsync(a => a.UserId);
        foreach (var userId in userIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }
}