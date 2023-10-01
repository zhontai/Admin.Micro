using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Utils.Helpers;
using ZhonTai.Module.Admin.HttpApi.Domain.Api;
using ZhonTai.Module.Admin.HttpApi.Domain.PermissionApi;
using ZhonTai.Module.Admin.HttpApi.Domain.Role;
using ZhonTai.Module.Admin.HttpApi.Domain.RolePermission;
using ZhonTai.Module.Admin.HttpApi.Domain.Tenant;
using ZhonTai.Module.Admin.HttpApi.Domain.User;
using ZhonTai.Module.Admin.HttpApi.Domain.UserRole;
using ZhonTai.Api.DynamicApi;
using ZhonTai.Api.DynamicApi.Attributes;
using ZhonTai.Api.Core.Consts;
using ZhonTai.Module.Admin.HttpApi.Domain.UserStaff;
using ZhonTai.Module.Admin.HttpApi.Domain.Org;
using System.Data;
using ZhonTai.Module.Admin.HttpApi.Domain.TenantPermission;
using FreeSql;
using ZhonTai.Module.Admin.HttpApi.Domain.RoleOrg;
using ZhonTai.Module.Admin.HttpApi.Domain.UserOrg;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using ZhonTai.Module.Admin.HttpApi.Domain.PkgPermission;
using ZhonTai.Module.Admin.HttpApi.Domain.TenantPkg;
using ZhonTai.Api.Core;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using ZhonTai.Api.Rpc.Enums;
using ZhonTai.Module.Admin.Contracts.Http;

namespace ZhonTai.Module.Admin.HttpApi.Services;

/// <summary>
/// 用户服务
/// </summary>
[Order(10)]
[DynamicApi(Area = AdminConsts.AreaName)]
public partial class UserService : BaseService, IUserService, IDynamicApi
{
    private readonly Lazy<IRoleRepository> _roleRepository;

    private AppConfig _appConfig => LazyGetRequiredService<AppConfig>();
    private IUserRepository _userRepository => LazyGetRequiredService<IUserRepository>();
    private IOrgRepository _orgRepository => LazyGetRequiredService<IOrgRepository>();
    private ITenantRepository _tenantRepository => LazyGetRequiredService<ITenantRepository>();
    private IApiRepository _apiRepository => LazyGetRequiredService<IApiRepository>();
    private IUserStaffRepository _staffRepository => LazyGetRequiredService<IUserStaffRepository>();
    private IUserRoleRepository _userRoleRepository => LazyGetRequiredService<IUserRoleRepository>();
    private IRoleOrgRepository _roleOrgRepository => LazyGetRequiredService<IRoleOrgRepository>();
    private IUserOrgRepository _userOrgRepository => LazyGetRequiredService<IUserOrgRepository>();
    private IPasswordHasher<UserEntity> _passwordHasher => LazyGetRequiredService<IPasswordHasher<UserEntity>>();
    private IFileService _fileService => LazyGetRequiredService<IFileService>();

    public UserService(Lazy<IRoleRepository> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    /// <summary>
    /// 查询用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserGetResponse> GetAsync(long id)
    {
        var userEntity = await _userRepository.Select
        .WhereDynamic(id)
        .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Id = b.Id, Name = b.Name }))
        .IncludeMany(a => a.Orgs.Select(b => new OrgEntity { Id = b.Id, Name = b.Name }))
        .ToOneAsync(a => new
        {
            a.Id,
            a.UserName,
            a.Name,
            a.Mobile,
            a.Email,
            a.Roles,
            a.Orgs,
            a.OrgId,
            a.ManagerUserId,
            ManagerUserName = a.ManagerUser.Name,
            Staff = new
            {
                a.Staff.JobNumber,
                a.Staff.Sex,
                a.Staff.Position,
                a.Staff.Introduce
            }
        });

        var output = Mapper.Map<UserGetResponse>(userEntity);

        return output;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageResponse<UserGetPageResponse>> GetPageAsync(PageRequest<UserGetPageFilterRequest> request)
    {
        var dataPermission = await AppInfo.GetRequiredService<IUserService>().GetDataPermissionAsync();

        var orgId = request.Filter?.OrgId;
        var list = await _userRepository.Select
        .WhereIf(dataPermission.OrgIds.Count > 0, a => _userOrgRepository.Where(b => b.UserId == a.Id && dataPermission.OrgIds.Contains(b.OrgId)).Any())
        .WhereIf(dataPermission.DataScope == DataScope.Self, a => a.CreatedUserId == User.Id)
        .WhereIf(orgId.HasValue && orgId > 0, a => _userOrgRepository.Where(b => b.UserId == a.Id && b.OrgId == orgId).Any())
        .Where(a=>a.Type != UserType.Member)
        .WhereDynamicFilter(request.DynamicFilter)
        .Count(out var total)
        .OrderByDescending(true, a => a.Id)
        .Page(request.CurrentPage, request.PageSize)
        .ToListAsync(a => new UserGetPageResponse 
        {
            RoleNames = _roleRepository.Value.Select
            .InnerJoin<UserRoleEntity>((b, c) => b.Id == c.RoleId && c.UserId == a.Id)
            .ToList(b => b.Name)
        });

        if(orgId.HasValue && orgId > 0)
        {
            var managerUserIds = await _userOrgRepository.Select
                .Where(a => a.OrgId == orgId && a.IsManager == true).ToListAsync(a => a.UserId);

            if (managerUserIds.Any())
            {
                var managerUsers = list.Where(a => managerUserIds.Contains(a.Id));
                foreach (var managerUser in managerUsers)
                {
                    managerUser.IsManager = true;
                }
            }
        }

        var data = new PageResponse<UserGetPageResponse>()
        {
            List = Mapper.Map<List<UserGetPageResponse>>(list),
            Total = total
        };

        return data;
    }

    /// <summary>
    /// 获得数据权限
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task<DataPermissionResponse> GetDataPermissionAsync()
    {
        if (!(User?.Id > 0))
        {
            return null;
        }

        var key = CacheKeys.DataPermission + User.Id;
        return await Cache.GetOrSetAsync(key, async () =>
        {
            using (_userRepository.DataFilter.Disable(FilterNames.Self, FilterNames.Data))
            {
                var user = await _userRepository.Select
                .IncludeMany(a => a.Roles.Select(b => new RoleEntity
                {
                    Id = b.Id,
                    DataScope = b.DataScope
                }))
                .WhereDynamic(User.Id)
                .ToOneAsync(a => new
                {
                    a.OrgId,
                    a.Roles
                });

                if (user == null)
                    return null;

                //数据范围
                DataScope dataScope = DataScope.Self;
                var customRoleIds = new List<long>();
                user.Roles?.ToList().ForEach(role =>
                {
                    if (role.DataScope == DataScope.Custom)
                    {
                        customRoleIds.Add(role.Id);
                    }
                    else if (role.DataScope <= dataScope)
                    {
                        dataScope = role.DataScope;
                    }
                });

                //部门列表
                var orgIds = new List<long>();
                if (dataScope != DataScope.All)
                {
                    //本部门
                    if (dataScope == DataScope.Dept)
                    {
                        orgIds.Add(user.OrgId);
                    }
                    //本部门和下级部门
                    else if (dataScope == DataScope.DeptWithChild)
                    {
                        orgIds = await _orgRepository
                        .Where(a => a.Id == user.OrgId)
                        .AsTreeCte()
                        .ToListAsync(a => a.Id);
                    }

                    //指定部门
                    if (customRoleIds.Count > 0)
                    {
                        if(dataScope == DataScope.Self)
                        {
                            dataScope = DataScope.Custom;
                        }
                        var customRoleOrgIds = await _roleOrgRepository.Select.Where(a => customRoleIds.Contains(a.RoleId)).ToListAsync(a => a.OrgId);
                        orgIds = orgIds.Concat(customRoleOrgIds).ToList();
                    }
                }

                return new DataPermissionResponse
                {
                    OrgId = user.OrgId,
                    OrgIds = orgIds.Distinct().ToList(),
                    DataScope = (User.PlatformAdmin || User.TenantAdmin) ? DataScope.All : dataScope
                };
            }
        });
    }

    /// <summary>
    /// 查询用户基本信息
    /// </summary>
    /// <returns></returns>
    [Login]
    public async Task<UserGetBasicResponse> GetBasicAsync()
    {
        if (!(User?.Id > 0))
        {
            throw Response.Exception("未登录！");
        }

        var data = await _userRepository.GetAsync<UserGetBasicResponse>(User.Id);
        data.Mobile = DataMaskHelper.PhoneMask(data.Mobile);
        data.Email = DataMaskHelper.EmailMask(data.Email);
        return data;
    }

    /// <summary>
    /// 查询用户权限信息
    /// </summary>
    /// <returns></returns>
    public async Task<IList<UserPermissionsResponse>> GetPermissionsAsync()
    {
        var key = CacheKeys.UserPermissions + User.Id;
        var result = await Cache.GetOrSetAsync(key, async () =>
        {
            if (User.TenantAdmin)
            {
                var cloud = LazyGetRequiredService<FreeSqlCloud>();
                var db = cloud.Use(DbKeys.AppDb);

                var tenantPermissions = await db.Select<ApiEntity>()
                .Where(a => db.Select<TenantPermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c) => b.PermissionId == c.PermissionId && b.TenantId == User.TenantId)
                .Where((b, c) => c.ApiId == a.Id).Any())
                .ToListAsync<UserPermissionsResponse>();

                var pkgPermissions = await db.Select<ApiEntity>()
                .Where(a => db.Select<TenantPkgEntity, PkgPermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c, d) => b.PkgId == c.PkgId && c.PermissionId == d.PermissionId && b.TenantId == User.TenantId)
                .Where((b, c, d) => d.ApiId == a.Id).Any())
                .ToListAsync<UserPermissionsResponse>();

                return tenantPermissions.Union(pkgPermissions).Distinct().ToList();
            }

            return await _apiRepository
            .Where(a => _apiRepository.Orm.Select<UserRoleEntity, RolePermissionEntity, PermissionApiEntity>()
            .InnerJoin((b, c, d) => b.RoleId == c.RoleId && b.UserId == User.Id)
            .InnerJoin((b, c, d) => c.PermissionId == d.PermissionId)
            .Where((b, c, d) => d.ApiId == a.Id).Any())
            .ToListAsync<UserPermissionsResponse>();
        });
        return result;
    }

    /// <summary>
    /// 新增用户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task<long> AddAsync(UserAddRequest request)
    {
        Expression<Func<UserEntity, bool>> where = (a => a.UserName == request.UserName);
        where = where.Or(request.Mobile.NotNull(), a => a.Mobile == request.Mobile)
            .Or(request.Email.NotNull(), a => a.Email == request.Email);

        var existsUser = await _userRepository.Select.Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == request.UserName)
            {
                throw Response.Exception($"账号已存在");
            }

            if (request.Mobile.NotNull() && existsUser.Mobile == request.Mobile)
            {
                throw Response.Exception($"手机号已存在");
            }

            if (request.Email.NotNull() && existsUser.Email == request.Email)
            {
                throw Response.Exception($"邮箱已存在");
            }
        }

        // 用户信息
        if (request.Password.IsNull())
        {
            request.Password = _appConfig.DefaultPassword;
        }

        var entity = Mapper.Map<UserEntity>(request);
        entity.Type = UserType.DefaultUser;
        if (_appConfig.PasswordHasher)
        {
            entity.Password = _passwordHasher.HashPassword(entity, request.Password);
            entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(request.Password);
            entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        var user = await _userRepository.InsertAsync(entity);
        var userId = user.Id;

        //用户角色
        if (request.RoleIds != null && request.RoleIds.Any())
        {
            var roles = request.RoleIds.Select(roleId => new UserRoleEntity
            { 
                UserId = userId,
                RoleId = roleId
            }).ToList();
            await _userRoleRepository.InsertAsync(roles);
        }

        // 员工信息
        var staff = request.Staff == null ? new UserStaffEntity() : Mapper.Map<UserStaffEntity>(request.Staff);
        staff.Id = userId;
        await _staffRepository.InsertAsync(staff);

        //所属部门
        if (request.OrgIds != null && request.OrgIds.Any())
        {
            var orgs = request.OrgIds.Select(orgId => new UserOrgEntity
            {
                UserId = userId,
                OrgId = orgId
            }).ToList();
            await _userOrgRepository.InsertAsync(orgs);
        }

        return userId;
    }

    /// <summary>
    /// 修改用户
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateAsync(UserUpdateRequest request)
    {
        if (request.Id == request.ManagerUserId)
        {
            throw Response.Exception("直属主管不能是自己");
        }

        Expression<Func<UserEntity, bool>> where = (a => a.UserName == request.UserName);
        where = where.Or(request.Mobile.NotNull(), a => a.Mobile == request.Mobile)
            .Or(request.Email.NotNull(), a => a.Email == request.Email);

        var existsUser = await _userRepository.Select.Where(a => a.Id != request.Id).Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == request.UserName)
            {
                throw Response.Exception($"账号已存在");
            }

            if (request.Mobile.NotNull() && existsUser.Mobile == request.Mobile)
            {
                throw Response.Exception($"手机号已存在");
            }

            if (request.Email.NotNull() && existsUser.Email == request.Email)
            {
                throw Response.Exception($"邮箱已存在");
            }
        }

        var user = await _userRepository.GetAsync(request.Id);
        if (!(user?.Id > 0))
        {
            throw Response.Exception("用户不存在");
        }

        Mapper.Map(request, user);
        await _userRepository.UpdateAsync(user);

        var userId = user.Id;

        // 用户角色
        await _userRoleRepository.DeleteAsync(a => a.UserId == userId);
        if (request.RoleIds != null && request.RoleIds.Any())
        {
            var roles = request.RoleIds.Select(roleId => new UserRoleEntity
            {
                UserId = userId,
                RoleId = roleId
            }).ToList();
            await _userRoleRepository.InsertAsync(roles);
        }

        // 员工信息
        var staff = await _staffRepository.GetAsync(userId);
        staff ??= new UserStaffEntity();
        Mapper.Map(request.Staff, staff);
        staff.Id = userId;
        await _staffRepository.InsertOrUpdateAsync(staff);

        //所属部门
        var orgIds = await _userOrgRepository.Select.Where(a => a.UserId == userId).ToListAsync(a => a.OrgId);
        var insertOrgIds = request.OrgIds.Except(orgIds);

        var deleteOrgIds = orgIds.Except(request.OrgIds);
        if (deleteOrgIds != null && deleteOrgIds.Any())
        {
            await _userOrgRepository.DeleteAsync(a => a.UserId == userId && deleteOrgIds.Contains(a.OrgId));
        }
            
        if (insertOrgIds != null && insertOrgIds.Any())
        {
            var orgs = insertOrgIds.Select(orgId => new UserOrgEntity
            {
                UserId = userId,
                OrgId = orgId
            }).ToList();
            await _userOrgRepository.InsertAsync(orgs);
        }

        await Cache.DelAsync(CacheKeys.DataPermission + user.Id);
    }

    /// <summary>
    /// 新增会员
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public virtual async Task<long> AddMemberAsync(UserAddMemberRequest request)
    {
        using (_userRepository.DataFilter.DisableAll())
        {
            Expression<Func<UserEntity, bool>> where = (a => a.UserName == request.UserName);
            where = where.Or(request.Mobile.NotNull(), a => a.Mobile == request.Mobile)
                .Or(request.Email.NotNull(), a => a.Email == request.Email);

            var existsUser = await _userRepository.Select.Where(where)
                .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

            if (existsUser != null)
            {
                if (existsUser.UserName == request.UserName)
                {
                    throw Response.Exception($"账号已存在");
                }

                if (request.Mobile.NotNull() && existsUser.Mobile == request.Mobile)
                {
                    throw Response.Exception($"手机号已存在");
                }

                if (request.Email.NotNull() && existsUser.Email == request.Email)
                {
                    throw Response.Exception($"邮箱已存在");
                }
            }

            // 用户信息
            if (request.Password.IsNull())
            {
                request.Password = _appConfig.DefaultPassword;
            }

            var entity = Mapper.Map<UserEntity>(request);
            entity.Type = UserType.Member;
            if (_appConfig.PasswordHasher)
            {
                entity.Password = _passwordHasher.HashPassword(entity, request.Password);
                entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
            }
            else
            {
                entity.Password = MD5Encrypt.Encrypt32(request.Password);
                entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
            }
            var user = await _userRepository.InsertAsync(entity);

            return user.Id;
        }
    }

    /// <summary>
    /// 修改会员
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task UpdateMemberAsync(UserUpdateMemberRequest request)
    {
        Expression<Func<UserEntity, bool>> where = (a => a.UserName == request.UserName);
        where = where.Or(request.Mobile.NotNull(), a => a.Mobile == request.Mobile)
            .Or(request.Email.NotNull(), a => a.Email == request.Email);

        var existsUser = await _userRepository.Select.Where(a => a.Id != request.Id).Where(where)
            .FirstAsync(a => new { a.UserName, a.Mobile, a.Email });

        if (existsUser != null)
        {
            if (existsUser.UserName == request.UserName)
            {
                throw Response.Exception($"账号已存在");
            }

            if (request.Mobile.NotNull() && existsUser.Mobile == request.Mobile)
            {
                throw Response.Exception($"手机号已存在");
            }

            if (request.Email.NotNull() && existsUser.Email == request.Email)
            {
                throw Response.Exception($"邮箱已存在");
            }
        }

        var user = await _userRepository.GetAsync(request.Id);
        if (!(user?.Id > 0))
        {
            throw Response.Exception("用户不存在");
        }

        Mapper.Map(request, user);
        await _userRepository.UpdateAsync(user);
    }

    /// <summary>
    /// 更新用户基本信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Login]
    public async Task UpdateBasicAsync(UserUpdateBasicRequest request)
    {
        var entity = await _userRepository.GetAsync(User.Id);
        entity = Mapper.Map(request, entity);
        await _userRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 修改用户密码
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Login]
    public async Task ChangePasswordAsync(UserChangePasswordRequest request)
    {
        if (request.ConfirmPassword != request.NewPassword)
        {
            throw Response.Exception("新密码和确认密码不一致");
        }

        var entity = await _userRepository.GetAsync(User.Id);
        var oldPassword = MD5Encrypt.Encrypt32(request.OldPassword);
        if (oldPassword != entity.Password)
        {
            throw Response.Exception("旧密码不正确");
        }

        entity.Password = MD5Encrypt.Encrypt32(request.NewPassword);
        await _userRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<string> ResetPasswordAsync(UserResetPasswordRequest request)
    {
        var entity = await _userRepository.GetAsync(request.Id);
        var password = request.Password;
        if (password.IsNull())
        {
            password = _appConfig.DefaultPassword;
        }
        if (password.IsNull())
        {
            password = "111111";
        }
        if (_appConfig.PasswordHasher)
        {
            entity.Password = _passwordHasher.HashPassword(entity, password);
            entity.PasswordEncryptType = PasswordEncryptType.PasswordHasher;
        }
        else
        {
            entity.Password = MD5Encrypt.Encrypt32(password);
            entity.PasswordEncryptType = PasswordEncryptType.MD5Encrypt32;
        }
        await _userRepository.UpdateAsync(entity);
        return password;
    }

    /// <summary>
    /// 设置主管
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task SetManagerAsync(UserSetManagerRequest request)
    {
        var entity = await _userOrgRepository.Where(a => a.UserId == request.UserId && a.OrgId == request.OrgId).FirstAsync();
        entity.IsManager = request.IsManager;
        await _userOrgRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 设置启用
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task SetEnableAsync(UserSetEnableRequest request)
    {
        var entity = await _userRepository.GetAsync(request.UserId);
        if (entity.Type == UserType.PlatformAdmin)
        {
            throw Response.Exception("平台管理员禁止禁用");
        }
        if (entity.Type == UserType.TenantAdmin)
        {
            throw Response.Exception("企业管理员禁止禁用");
        }
        entity.Enabled = request.Enabled;
        await _userRepository.UpdateAsync(entity);
    }

    /// <summary>
    /// 彻底删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task DeleteAsync(long id)
    {
        var user = await _userRepository.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
        if (user == null)
        {
            throw Response.Exception("用户不存在");
        }

        if(user.Type == UserType.PlatformAdmin)
        {
            throw Response.Exception($"平台管理员禁止删除");
        }

        if (user.Type == UserType.TenantAdmin)
        {
            throw Response.Exception($"企业管理员禁止删除");
        }

        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => a.UserId == id);
        //删除用户所属部门
        await _userOrgRepository.DeleteAsync(a => a.UserId == id);
        //删除员工
        await _staffRepository.DeleteAsync(a => a.Id == id);
        //删除用户
        await _userRepository.DeleteAsync(a => a.Id == id);

        //删除用户数据权限缓存
        await Cache.DelAsync(CacheKeys.DataPermission + id);
    }

    /// <summary>
    /// 批量彻底删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchDeleteAsync(long[] ids)
    {
        var admin = await _userRepository.Select.Where(a => ids.Contains(a.Id) && 
        (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

        if (admin)
        {
            throw Response.Exception("平台管理员禁止删除");
        }
       
        //删除用户角色
        await _userRoleRepository.DeleteAsync(a => ids.Contains(a.UserId));
        //删除用户所属部门
        await _userOrgRepository.DeleteAsync(a => ids.Contains(a.UserId));
        //删除员工
        await _staffRepository.DeleteAsync(a => ids.Contains(a.Id));
        //删除用户
        await _userRepository.DeleteAsync(a => ids.Contains(a.Id));

        foreach (var userId in ids)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task SoftDeleteAsync(long id)
    {
        var user = await _userRepository.Select.WhereDynamic(id).ToOneAsync(a => new { a.Type });
        if (user == null)
        {
            throw Response.Exception("用户不存在");
        }

        if (user.Type == UserType.PlatformAdmin || user.Type == UserType.TenantAdmin)
        {
            throw Response.Exception("平台管理员禁止删除");
        }

        await _userRoleRepository.DeleteAsync(a => a.UserId == id);
        await _userOrgRepository.DeleteAsync(a => a.UserId == id);
        await _staffRepository.SoftDeleteAsync(a => a.Id == id);
        await _userRepository.SoftDeleteAsync(id);

        await Cache.DelAsync(CacheKeys.DataPermission + id);
    }

    /// <summary>
    /// 批量删除用户
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [AdminTransaction]
    public virtual async Task BatchSoftDeleteAsync(long[] ids)
    {
        var admin = await _userRepository.Select.Where(a => ids.Contains(a.Id) && 
        (a.Type == UserType.PlatformAdmin || a.Type == UserType.TenantAdmin)).AnyAsync();

        if (admin)
        {
            throw Response.Exception("平台管理员禁止删除");
        }

        await _userRoleRepository.DeleteAsync(a => ids.Contains(a.UserId));
        await _userOrgRepository.DeleteAsync(a => ids.Contains(a.UserId));
        await _staffRepository.SoftDeleteAsync(a => ids.Contains(a.Id));
        await _userRepository.SoftDeleteAsync(ids);

        foreach (var userId in ids)
        {
            await Cache.DelAsync(CacheKeys.DataPermission + userId);
        }
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="file"></param>
    /// <param name="autoUpdate"></param>
    /// <returns></returns>
    [HttpPost]
    [Login]
    public async Task<string> AvatarUpload([FromForm] IFormFile file, bool autoUpdate = false)
    {
        var fileInfo = await _fileService.UploadFileAsync(file);
        if (autoUpdate)
        {
            var entity = await _userRepository.GetAsync(User.Id);
            entity.Avatar = fileInfo.LinkUrl;
            await _userRepository.UpdateAsync(entity);
        }
        return fileInfo.LinkUrl;
    }

    /// <summary>
    /// 一键登录用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<dynamic> OneClickLoginAsync([Required]string userName)
    {
        if (userName.IsNull())
        {
            throw Response.Exception("请选择用户");
        }

        using var _ = _userRepository.DataFilter.DisableAll();

        var authGetTokenRequest = await _userRepository.Select.Where(a => a.UserName == userName).ToOneAsync<AuthGetTokenRequest>();

        if(authGetTokenRequest == null)
        {
            throw Response.Exception("用户不存在");
        }

        if (_appConfig.Tenant)
        {
            var tenant = await _tenantRepository.Select.WhereDynamic(authGetTokenRequest.TenantId).ToOneAsync<AuthGetTokenRequest.Models.TenantModel>();
            authGetTokenRequest.Tenant = tenant;
        }

        string token = AppInfo.GetRequiredService<IAuthService>().GetToken(authGetTokenRequest);

        return new { token }; 
    }
}