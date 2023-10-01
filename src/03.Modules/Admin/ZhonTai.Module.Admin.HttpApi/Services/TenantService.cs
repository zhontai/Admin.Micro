using Microsoft.AspNetCore.Mvc;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Utils.Helpers;
using ZhonTai.Module.Admin.HttpApi.Domain.Role;
using ZhonTai.Module.Admin.HttpApi.Domain.RolePermission;
using ZhonTai.Module.Admin.HttpApi.Domain.Tenant;
using ZhonTai.Module.Admin.HttpApi.Domain.User;
using ZhonTai.Module.Admin.HttpApi.Domain.UserRole;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using ZhonTai.Api.Core.Consts;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Module.Admin.HttpApi.Domain.Org;
using ZhonTai.Module.Admin.HttpApi.Domain.UserStaff;
using ZhonTai.Module.Admin.HttpApi.Domain.UserOrg;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using ZhonTai.Module.Admin.HttpApi.Domain.Pkg;
using ZhonTai.Module.Admin.HttpApi.Domain.TenantPkg;
using ZhonTai.Module.Admin.HttpApi.Services.Pkg;
using ZhonTai.Api.Core.Enums;
using ZhonTai.Api.Core.IdGenerator;
using ZhonTai.Api.Rpc.Enums;

namespace ZhonTai.Module.Admin.HttpApi.Services.Tenant;

/// <summary>
/// 租户服务
/// </summary>
[Order(50)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class TenantService : BaseService, ITenantService, IDynamicApi
{
    private readonly Lazy<IPkgRepository> _pkgRepository;
    private AppConfig _appConfig => LazyGetRequiredService<AppConfig>();
    private ITenantRepository _tenantRepository => LazyGetRequiredService<ITenantRepository>();
    private IRoleRepository _roleRepository => LazyGetRequiredService<IRoleRepository>();
    private IUserRepository _userRepository => LazyGetRequiredService<IUserRepository>();
    private IOrgRepository _orgRepository => LazyGetRequiredService<IOrgRepository>();
    private IUserRoleRepository _userRoleRepository => LazyGetRequiredService<IUserRoleRepository>();
    private IRolePermissionRepository _rolePermissionRepository => LazyGetRequiredService<IRolePermissionRepository>();
    private IUserStaffRepository _userStaffRepository => LazyGetRequiredService<IUserStaffRepository>();
    private IUserOrgRepository _userOrgRepository => LazyGetRequiredService<IUserOrgRepository>();
    private IPasswordHasher<UserEntity> _passwordHasher => LazyGetRequiredService<IPasswordHasher<UserEntity>>();
    private ITenantPkgRepository _tenantPkgRepository => LazyGetRequiredService<ITenantPkgRepository>();

    public TenantService(Lazy<IPkgRepository> pkgRepository)
    {
        _pkgRepository = pkgRepository;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TenantGetResponse> GetAsync(long id)
    {
        using var _ = _tenantRepository.DataFilter.Disable(FilterNames.Tenant);

        var tenant = await _tenantRepository.Select
        .WhereDynamic(id)
        .FirstAsync(a => new TenantGetResponse
        {
            Name = a.Org.Name,
            Code = a.Org.Code,
            UserName = a.User.UserName,
            RealName = a.User.Name,
            Phone = a.User.Mobile,
            Email = a.User.Email,
            PkgIds = _pkgRepository.Value.Select
            .InnerJoin<TenantPkgEntity>((b, c) => b.Id == c.PkgId && c.TenantId == a.Id)
            .ToList(b => b.Id)
        });
        return tenant;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResponse<TenantGetPageResponse>> GetPageAsync(PageRequest<TenantGetPageFilterRequest> request)
    {
        using var _ = _tenantRepository.DataFilter.Disable(FilterNames.Tenant);

        var key = request.Filter?.Name;

        var list = await _tenantRepository.Select
        .WhereDynamicFilter(request.DynamicFilter)
        .WhereIf(key.NotNull(), a => a.Org.Name.Contains(key))
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .Page(request.CurrentPage, request.PageSize)
        .ToListAsync(a => new TenantGetPageResponse
        {
            Name = a.Org.Name,
            Code = a.Org.Code,
            UserName = a.User.UserName,
            RealName = a.User.Name,
            Phone = a.User.Mobile,
            Email = a.User.Email,
            PkgNames = _pkgRepository.Value.Select
            .InnerJoin<TenantPkgEntity>((b, c) => b.Id == c.PkgId && c.TenantId == a.Id)
            .ToList(b => b.Name)
        });

        var data = new PageResponse<TenantGetPageResponse>()
        {
            List = Mapper.Map<List<TenantGetPageResponse>>(list),
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task<long> AddAsync(TenantAddRequest request)
    {
        using (_tenantRepository.DataFilter.Disable(FilterNames.Tenant))
        {
            var existsOrg = await _orgRepository.Select
            .Where(a => (a.Name == request.Name || a.Code == request.Code) && a.ParentId == 0)
            .FirstAsync(a => new { a.Name, a.Code });

            if (existsOrg != null)
            {
                if (existsOrg.Name == request.Name)
                {
                    throw Response.Exception($"企业名称已存在");
                }

                if (existsOrg.Code == request.Code)
                {
                    throw Response.Exception($"企业编码已存在");
                }
            }

            Expression<Func<UserEntity, bool>> where = (a => a.UserName == request.UserName);
            where = where.Or(request.Phone.NotNull(), a => a.Mobile == request.Phone)
                .Or(request.Email.NotNull(), a => a.Email == request.Email);

            var existsUser = await _userRepository.Select.Where(where)
                .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

            if (existsUser != null)
            {
                if (existsUser.UserName == request.UserName)
                {
                    throw Response.Exception($"企业账号已存在");
                }

                if (request.Phone.NotNull() && existsUser.Mobile == request.Phone)
                {
                    throw Response.Exception($"企业手机号已存在");
                }

                if (request.Email.NotNull() && existsUser.Email == request.Email)
                {
                    throw Response.Exception($"企业邮箱已存在");
                }
            }

            //添加租户
            TenantEntity entity = Mapper.Map<TenantEntity>(request);
            TenantEntity tenant = await _tenantRepository.InsertAsync(entity);
            long tenantId = tenant.Id;

            //添加租户套餐
            if (request.PkgIds != null && request.PkgIds.Any())
            {
                var pkgs = request.PkgIds.Select(pkgId => new TenantPkgEntity
                {
                    TenantId = tenantId,
                    PkgId = pkgId
                }).ToList();

                await _tenantPkgRepository.InsertAsync(pkgs);
            }

            //添加部门
            var org = new OrgEntity
            {
                TenantId = tenantId,
                Name = request.Name,
                Code = request.Code,
                ParentId = 0,
                MemberCount = 1,
                Sort = 1,
                Enabled = true
            };
            await _orgRepository.InsertAsync(org);

            //添加用户
            if (request.Password.IsNull())
            {
                request.Password = _appConfig.DefaultPassword;
            }
            var user = new UserEntity
            {
                TenantId = tenantId,
                UserName = request.UserName,
                Name = request.RealName,
                Mobile = request.Phone,
                Email = request.Email,
                Type = UserType.TenantAdmin,
                OrgId = org.Id,
                Enabled = true
            };
            if (_appConfig.PasswordHasher)
            {
                user.Password = _passwordHasher.HashPassword(user, request.Password);
                user.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
            }
            else
            {
                user.Password = MD5Encrypt.Encrypt32(request.Password);
                user.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
            }
            await _userRepository.InsertAsync(user);

            long userId = user.Id;

            //添加用户员工
            var emp = new UserStaffEntity
            {
                Id = userId,
                TenantId = tenantId
            };
            await _userStaffRepository.InsertAsync(emp);

            //添加用户部门
            var userOrg = new UserOrgEntity
            {
                UserId = userId,
                OrgId = org.Id
            };
            await _userOrgRepository.InsertAsync(userOrg);

            //添加角色分组和角色
            var roleGroupId = IdHelper.GetNextId();
            var roleId = IdHelper.GetNextId();
            var jobGroupId = IdHelper.GetNextId();
            var roles = new List<RoleEntity>{
                new RoleEntity
                {
                    Id = roleGroupId,
                    ParentId = 0,
                    TenantId = tenantId,
                    Type = RoleType.Group,
                    Name = "系统默认",
                    Sort = 1
                },
                new RoleEntity
                {
                    Id = roleId,
                    TenantId = tenantId,
                    Type = RoleType.Role,
                    Name = "主管理员",
                    Code = "main-admin",
                    ParentId = roleGroupId,
                    DataScope = DataScope.All,
                    Sort = 1
                },
                new RoleEntity
                {
                    Id= jobGroupId,
                    ParentId = 0,
                    TenantId = tenantId,
                    Type = RoleType.Group,
                    Name = "岗位",
                    Sort = 2
                },
                new RoleEntity
                {
                    TenantId = tenantId,
                    Type = RoleType.Role,
                    Name = "普通员工",
                    Code = "emp",
                    ParentId = jobGroupId,
                    DataScope = DataScope.Self,
                    Sort = 1
                }
            };
            await _roleRepository.InsertAsync(roles);

            //添加用户角色
            var userRole = new UserRoleEntity()
            {
                UserId = userId,
                RoleId = roleId
            };
            await _userRoleRepository.InsertAsync(userRole);

            //更新租户的用户和部门
            tenant.UserId = userId;
            tenant.OrgId = org.Id;
            await _tenantRepository.UpdateAsync(tenant);

            return tenant.Id;
        }
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task UpdateAsync(TenantUpdateRequest request)
    {
        using (_tenantRepository.DataFilter.Disable(FilterNames.Tenant))
        {
            var tenant = await _tenantRepository.GetAsync(request.Id);
            if (!(tenant?.Id > 0))
            {
                throw Response.Exception("租户不存在");
            }

            var existsOrg = await _orgRepository.Select
                .Where(a => a.Id != tenant.OrgId && (a.Name == request.Name || a.Code == request.Code))
                .FirstAsync(a => new { a.Name, a.Code });

            if (existsOrg != null)
            {
                if (existsOrg.Name == request.Name)
                {
                    throw Response.Exception($"企业名称已存在");
                }

                if (existsOrg.Code == request.Code)
                {
                    throw Response.Exception($"企业编码已存在");
                }
            }

            Expression<Func<UserEntity, bool>> where = (a => a.UserName == request.UserName);
            where = where.Or(request.Phone.NotNull(), a => a.Mobile == request.Phone)
                .Or(request.Email.NotNull(), a => a.Email == request.Email);

            var existsUser = await _userRepository.Select.Where(a => a.Id != tenant.UserId).Where(where)
                .FirstAsync(a => new { a.Id, a.Name, a.UserName, a.Mobile, a.Email });

            if (existsUser != null)
            {
                if (existsUser.UserName == request.UserName)
                {
                    throw Response.Exception($"企业账号已存在");
                }

                if (request.Phone.NotNull() && existsUser.Mobile == request.Phone)
                {
                    throw Response.Exception($"企业手机号已存在");
                }

                if (request.Email.NotNull() && existsUser.Email == request.Email)
                {
                    throw Response.Exception($"企业邮箱已存在");
                }
            }

            //更新用户
            await _userRepository.UpdateDiy.DisableGlobalFilter(FilterNames.Tenant).SetSource(
            new UserEntity()
            {
                Id = tenant.UserId,
                Name = request.RealName,
                UserName = request.UserName,
                Mobile = request.Phone,
                Email = request.Email
            })
            .UpdateColumns(a => new { a.Name, a.UserName, a.Mobile, a.Email, a.ModifiedTime }).ExecuteAffrowsAsync();

            //更新部门
            await _orgRepository.UpdateDiy.DisableGlobalFilter(FilterNames.Tenant).SetSource(
            new OrgEntity()
            {
                Id = tenant.OrgId,
                Name = request.Name,
                Code = request.Code
            })
            .UpdateColumns(a => new { a.Name, a.Code, a.ModifiedTime }).ExecuteAffrowsAsync();

            //更新租户
            await _tenantRepository.UpdateDiy.DisableGlobalFilter(FilterNames.Tenant).SetSource(
            new TenantEntity()
            {
                Id = tenant.Id,
                Description = request.Description,
            })
            .UpdateColumns(a => new { a.Description, a.ModifiedTime }).ExecuteAffrowsAsync();

            //更新租户套餐
            await _tenantPkgRepository.DeleteAsync(a => a.TenantId == tenant.Id);
            if (request.PkgIds != null && request.PkgIds.Any())
            {
                var pkgs = request.PkgIds.Select(pkgId => new TenantPkgEntity
                {
                    TenantId = tenant.Id,
                    PkgId = pkgId
                }).ToList();

                await _tenantPkgRepository.InsertAsync(pkgs);

                //清除租户下所有用户权限缓存
                await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(new List<long> { tenant.Id });
            }
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
        using (_tenantRepository.DataFilter.Disable(FilterNames.Tenant))
        {
            var tenantType = await _tenantRepository.Select.WhereDynamic(id).ToOneAsync(a => a.TenantType);
            if (tenantType == TenantType.Platform)
            {
                throw Response.Exception("平台租户禁止删除");
            }

            //删除角色权限
            await _rolePermissionRepository.Where(a => a.Role.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //删除用户角色
            await _userRoleRepository.Where(a => a.User.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //删除员工
            await _userStaffRepository.Where(a => a.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //删除用户部门
            await _userOrgRepository.Where(a => a.User.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //删除部门
            await _orgRepository.Where(a => a.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //删除用户
            await _userRepository.Where(a => a.TenantId == id && a.Type != UserType.Member).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //删除角色
            await _roleRepository.Where(a => a.TenantId == id).DisableGlobalFilter(FilterNames.Tenant).ToDelete().ExecuteAffrowsAsync();

            //删除租户套餐
            await _tenantPkgRepository.DeleteAsync(a => a.TenantId == id);

            //删除租户
            await _tenantRepository.DeleteAsync(id);

            //清除租户下所有用户权限缓存
            await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(new List<long> { id });
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
        using (_tenantRepository.DataFilter.Disable(FilterNames.Tenant))
        {
            var tenantType = await _tenantRepository.Select.WhereDynamic(id).ToOneAsync(a => a.TenantType);
            if (tenantType == TenantType.Platform)
            {
                throw Response.Exception("平台租户禁止删除");
            }

            //删除部门
            await _orgRepository.SoftDeleteAsync(a => a.TenantId == id, FilterNames.Tenant);

            //删除用户
            await _userRepository.SoftDeleteAsync(a => a.TenantId == id && a.Type != UserType.Member, FilterNames.Tenant);

            //删除角色
            await _roleRepository.SoftDeleteAsync(a => a.TenantId == id, FilterNames.Tenant);

            //删除租户
            var result = await _tenantRepository.SoftDeleteAsync(id);

            //清除租户下所有用户权限缓存
            await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(new List<long> { id });
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
        using (_tenantRepository.DataFilter.Disable(FilterNames.Tenant))
        {
            var tenantType = await _tenantRepository.Select.WhereDynamic(ids).ToOneAsync(a => a.TenantType);
            if (tenantType == TenantType.Platform)
            {
                throw Response.Exception("平台租户禁止删除");
            }

            //删除部门
            await _orgRepository.SoftDeleteAsync(a => ids.Contains(a.TenantId.Value), FilterNames.Tenant);

            //删除用户
            await _userRepository.SoftDeleteAsync(a => ids.Contains(a.TenantId.Value) && a.Type != UserType.Member, FilterNames.Tenant);

            //删除角色
            await _roleRepository.SoftDeleteAsync(a => ids.Contains(a.TenantId.Value), FilterNames.Tenant);

            //删除租户
            var result = await _tenantRepository.SoftDeleteAsync(ids);

            //清除租户下所有用户权限缓存
            await LazyGetRequiredService<PkgService>().ClearUserPermissionsAsync(ids.ToList());
        }
    }

    /// <summary>
    /// 设置启用
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(TenantSetEnableRequest request)
    {
        var entity = await _tenantRepository.GetAsync(request.TenantId);
        if (entity.TenantType == TenantType.Platform)
        {
            throw Response.Exception("平台租户禁止禁用");
        }
        entity.Enabled = request.Enabled;
        await _tenantRepository.UpdateAsync(entity);
    }
}