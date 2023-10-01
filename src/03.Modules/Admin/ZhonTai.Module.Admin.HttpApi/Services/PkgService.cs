using ZhonTai.Module.Admin.HttpApi.Domain.Pkg;
using ZhonTai.Module.Admin.HttpApi.Domain.PkgPermission;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using ZhonTai.Api.Core.Consts;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Module.Admin.HttpApi.Domain.TenantPkg;
using ZhonTai.Module.Admin.HttpApi.Domain.Tenant;
using ZhonTai.Module.Admin.HttpApi.Domain.RolePermission;
using ZhonTai.Module.Admin.HttpApi.Domain.User;
using ZhonTai.Module.Admin.HttpApi.Domain.Org;

namespace ZhonTai.Module.Admin.HttpApi.Services.Pkg;

/// <summary>
/// 套餐服务
/// </summary>
[Order(51)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class PkgService : BaseService, IDynamicApi
{
    private IPkgRepository _pkgRepository => LazyGetRequiredService<IPkgRepository>();
    private ITenantRepository _tenantRepository => LazyGetRequiredService<ITenantRepository>();
    private ITenantPkgRepository _tenantPkgRepository => LazyGetRequiredService<ITenantPkgRepository>();
    private IPkgPermissionRepository _pkgPermissionRepository => LazyGetRequiredService<IPkgPermissionRepository>();
    private IRolePermissionRepository _rolePermissionRepository => LazyGetRequiredService<IRolePermissionRepository>();
    private IUserRepository _userRepository => LazyGetRequiredService<IUserRepository>();

    public PkgService()
    {
    }

    /// <summary>
    /// 清除租户下所有用户权限缓存
    /// </summary>
    /// <param name="tenantIds"></param>
    [NonAction]
    public async Task ClearUserPermissionsAsync(List<long> tenantIds)
    {
        using var _ = _userRepository.DataFilter.Disable(FilterNames.Tenant);
        var userIds = await _userRepository.Select.Where(a => tenantIds.Contains(a.TenantId.Value)).ToListAsync(a => a.Id);
        if (userIds.Any())
        {
            foreach (var userId in userIds)
            {
                await Cache.DelAsync(CacheKeys.UserPermissions + userId);
            }
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<PkgGetResponse> GetAsync(long id)
    {
        return await _pkgRepository.Select
        .WhereDynamic(id)
        .ToOneAsync<PkgGetResponse>();
    }

    /// <summary>
    /// 查询列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<List<PkgGetListResponse>> GetListAsync([FromQuery]PkgGetListRequest request)
    {
        var list = await _pkgRepository.Select
        .WhereIf(request.Name.NotNull(), a => a.Name.Contains(request.Name))
        .OrderBy(a => new {a.ParentId, a.Sort})
        .ToListAsync<PkgGetListResponse>();

        return list;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResponse<PkgGetPageResponse>> GetPageAsync(PageRequest<PkgGetPageFilterRequest> request)
    {
        var key = request.Filter?.Name;

        var list = await _pkgRepository.Select
        .WhereDynamicFilter(request.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Name.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, c => c.Id)
        .Page(request.CurrentPage, request.PageSize)
        .ToListAsync<PkgGetPageResponse>();

        var data = new PageResponse<PkgGetPageResponse>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 查询套餐租户列表
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<List<PkgGetPkgTenantListResponse>> GetPkgTenantListAsync([FromQuery] PkgGetPkgTenantListRequest request)
    {
        using var _ = _tenantRepository.DataFilter.Disable(FilterNames.Tenant);

        var list = await _tenantRepository.Select.From<TenantPkgEntity, OrgEntity>((s, b, c) => s
            .InnerJoin(a => a.Id == b.TenantId)
            .InnerJoin(a => a.OrgId == c.Id))
            .Where((a, b, c) => b.PkgId == request.PkgId)
            .WhereIf(request.TenantName.NotNull(), (a, b, c) => c.Name.Contains(request.TenantName))
            .OrderByDescending((a, b, c) => b.Id)
            .ToListAsync((a, b, c) => new PkgGetPkgTenantListResponse { Id = a.Id, Name = c.Name, Code = c.Code });

        return list;
    }

    /// <summary>
    /// 查询套餐租户分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResponse<PkgGetPkgTenantListResponse>> GetPkgTenantPageAsync(PageRequest<PkgGetPkgTenantListRequest> request)
    {
        using var _ = _tenantRepository.DataFilter.Disable(FilterNames.Tenant);

        var list = await _tenantRepository.Select.From<TenantPkgEntity, OrgEntity>((s, b, c) => s
            .InnerJoin(a => a.Id == b.TenantId)
            .InnerJoin(a => a.OrgId == c.Id))
            .Where((a, b, c) => b.PkgId == request.Filter.PkgId)
            .WhereIf(request.Filter.TenantName.NotNull(), (a, b, c) => c.Name.Contains(request.Filter.TenantName))
            .Count(out var total)
            .OrderByDescending((a, b, c) => b.Id)
            .Page(request.CurrentPage, request.PageSize)
            .ToListAsync((a, b, c) => new PkgGetPkgTenantListResponse { Id = a.Id, Name = c.Name, Code = c.Code });

        var data = new PageResponse<PkgGetPkgTenantListResponse>()
        {
            List = list,
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 查询套餐权限列表
    /// </summary>
    /// <param name="pkgId">套餐编号</param>
    /// <returns></returns>
    public async Task<List<long>> GetPkgPermissionListAsync(long pkgId)
    {
        var permissionIds = await _pkgPermissionRepository
            .Select.Where(d => d.PkgId == pkgId)
            .ToListAsync(a => a.PermissionId);

        return permissionIds;
    }

    /// <summary>
    /// 设置套餐权限
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SetPkgPermissionsAsync(PkgSetPkgPermissionsRequest request)
    {
        //查询套餐权限
        var permissionIds = await _pkgPermissionRepository.Select.Where(d => d.PkgId == request.PkgId).ToListAsync(m => m.PermissionId);

        //批量删除套餐权限
        var deleteIds = permissionIds.Where(d => !request.PermissionIds.Contains(d));
        if (deleteIds.Any())
        {
            //删除套餐权限
            await _pkgPermissionRepository.DeleteAsync(m => m.PkgId == request.PkgId && deleteIds.Contains(m.PermissionId));
            //删除套餐下关联的角色权限
            await _rolePermissionRepository.DeleteAsync(a => deleteIds.Contains(a.PermissionId));
        }

        //批量插入套餐权限
        var pkgPermissions = new List<PkgPermissionEntity>();
        var insertPermissionIds = request.PermissionIds.Where(d => !permissionIds.Contains(d));
        if (insertPermissionIds.Any())
        {
            foreach (var permissionId in insertPermissionIds)
            {
                pkgPermissions.Add(new PkgPermissionEntity()
                {
                    PkgId = request.PkgId,
                    PermissionId = permissionId,
                });
            }
            await _pkgPermissionRepository.InsertAsync(pkgPermissions);
        }

        
        var tenantIds = await _tenantPkgRepository.Select.Where(a => a.PkgId == request.PkgId).ToListAsync(a => a.TenantId);
        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 添加套餐租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task AddPkgTenantAsync(PkgAddPkgTenantListRequest input)
    {
        var pkgId = input.PkgId;
        var tenantIds = await _tenantPkgRepository.Select.Where(a => a.PkgId == pkgId).ToListAsync(a => a.TenantId);
        var insertTenantIds = input.TenantIds.Except(tenantIds);
        if (insertTenantIds != null && insertTenantIds.Any())
        {
            var tenantPkgList = insertTenantIds.Select(tenantId => new TenantPkgEntity 
            { 
                TenantId = tenantId, 
                PkgId = pkgId 
            }).ToList();
            await _tenantPkgRepository.InsertAsync(tenantPkgList);

            //清除租户下所有用户权限缓存
            await ClearUserPermissionsAsync(insertTenantIds.ToList());
        }
    }

    /// <summary>
    /// 移除套餐租户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task RemovePkgTenantAsync(PkgAddPkgTenantListRequest request)
    {
        var tenantIds = request.TenantIds;
        if (tenantIds != null && tenantIds.Any())
        {
            await _tenantPkgRepository.Where(a => a.PkgId == request.PkgId && request.TenantIds.Contains(a.TenantId)).ToDelete().ExecuteAffrowsAsync();

            //清除租户下所有用户权限缓存
            await ClearUserPermissionsAsync(tenantIds.ToList());
        }
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(PkgAddRequest request)
    {
        if (await _pkgRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Name == request.Name))
        {
            throw Response.Exception($"此套餐名已存在");
        }

        if (request.Code.NotNull() && await _pkgRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Code == request.Code))
        {
            throw Response.Exception($"此套餐编码已存在");
        }

        var entity = Mapper.Map<PkgEntity>(request);
        if (entity.Sort == 0)
        {
            var sort = await _pkgRepository.Select.Where(a => a.ParentId == request.ParentId).MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }

        await _pkgRepository.InsertAsync(entity);

        return entity.Id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task UpdateAsync(PkgUpdateRequest request)
    {
        var entity = await _pkgRepository.GetAsync(request.Id);
        if (!(entity?.Id > 0))
        {
            throw Response.Exception("套餐不存在");
        }

        if (await _pkgRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Id != request.Id && a.Name == request.Name))
        {
            throw Response.Exception($"此套餐名已存在");
        }

        if (request.Code.NotNull() && await _pkgRepository.Select.AnyAsync(a => a.ParentId == request.ParentId && a.Id != request.Id && a.Code == request.Code))
        {
            throw Response.Exception($"此套餐编码已存在");
        }

        Mapper.Map(request, entity);
        await _pkgRepository.UpdateAsync(entity);

        var tenantIds = await _tenantPkgRepository.Select.Where(a => a.PkgId == entity.Id).ToListAsync(a => a.TenantId);
        foreach (var tenantId in tenantIds)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + tenantId);
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
        var pkgIdList = await _pkgRepository.GetChildIdListAsync(id);
        var tenantIds = await _tenantPkgRepository.Select.Where(a => pkgIdList.Contains(a.PkgId)).ToListAsync(a => a.TenantId);

        //删除租户套餐
        await _tenantPkgRepository.DeleteAsync(a => a.TenantId == id);
        //删除套餐权限
        await _pkgPermissionRepository.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        //删除套餐
        await _pkgRepository.DeleteAsync(a => pkgIdList.Contains(a.Id));

        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 批量彻底删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var pkgIdList = await _pkgRepository.GetChildIdListAsync(ids);
        var tenantIds = await _tenantPkgRepository.Select.Where(a => pkgIdList.Contains(a.PkgId)).ToListAsync(a => a.TenantId);

        //删除租户套餐
        await _tenantPkgRepository.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        //删除套餐权限
        await _pkgPermissionRepository.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        //删除套餐
        await _pkgRepository.Where(a => pkgIdList.Contains(a.Id)).AsTreeCte().ToDelete().ExecuteAffrowsAsync();

        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        var pkgIdList = await _pkgRepository.GetChildIdListAsync(id);
        var tenantIds = await _tenantPkgRepository.Select.Where(a => pkgIdList.Contains(a.PkgId)).ToListAsync(a => a.TenantId);
        await _tenantPkgRepository.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        await _pkgPermissionRepository.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        await _pkgRepository.SoftDeleteRecursiveAsync(a => pkgIdList.Contains(a.Id));

        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var pkgIdList = await _pkgRepository.GetChildIdListAsync(ids);
        var tenantIds = await _tenantPkgRepository.Select.Where(a => ids.Contains(a.PkgId)).ToListAsync(a => a.TenantId);
        await _tenantPkgRepository.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        await _pkgPermissionRepository.DeleteAsync(a => pkgIdList.Contains(a.PkgId));
        await _pkgRepository.SoftDeleteRecursiveAsync(a => pkgIdList.Contains(a.Id));

        //清除租户下所有用户权限缓存
        await ClearUserPermissionsAsync(tenantIds);
    }
}