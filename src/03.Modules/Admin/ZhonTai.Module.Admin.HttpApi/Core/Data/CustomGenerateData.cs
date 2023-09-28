using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Api.Core.Db;
using ZhonTai.Module.Admin.HttpApi.Domain.DictType;
using ZhonTai.Module.Admin.HttpApi.Domain.Dict;
using ZhonTai.Module.Admin.HttpApi.Domain.Api;
using ZhonTai.Module.Admin.HttpApi.Domain.Permission;
using ZhonTai.Module.Admin.HttpApi.Domain.User;
using ZhonTai.Module.Admin.HttpApi.Domain.Role;
using ZhonTai.Module.Admin.HttpApi.Domain.UserRole;
using ZhonTai.Module.Admin.HttpApi.Domain.RolePermission;
using ZhonTai.Module.Admin.HttpApi.Domain.Tenant;
using ZhonTai.Module.Admin.HttpApi.Domain.TenantPermission;
using ZhonTai.Module.Admin.HttpApi.Domain.PermissionApi;
using ZhonTai.Module.Admin.HttpApi.Domain.View;
using ZhonTai.Api.Core.Configs;
using ZhonTai.Utils.Extensions;
using ZhonTai.Module.Admin.HttpApi.Domain.UserStaff;
using ZhonTai.Module.Admin.HttpApi.Domain.Org;
using ZhonTai.Api.Core.Db.Data;
using FreeSql;
using ZhonTai.Module.Admin.HttpApi.Domain.UserOrg;
using System.Reflection;
using ZhonTai.Api.Core.Attributes;
using ZhonTai.Api.Core.Helpers;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

/// <summary>
/// 生成数据
/// </summary>
public class CustomGenerateData : GenerateData, IGenerateData
{
    public virtual async Task GenerateDataAsync(IFreeSql db, AppConfig appConfig)
    {
        #region 读取数据

        //admin
        #region 数据字典

        var dictionaryTypes = await db.Queryable<DictTypeEntity>().ToListAsync();

        var dictionaries = await db.Queryable<DictEntity>().ToListAsync();
        #endregion

        #region 接口

        var apis = await db.Queryable<ApiEntity>().ToListAsync();
        var apiTree = apis.Clone().ToTree((r, c) =>
        {
            return c.ParentId == 0;
        },
        (r, c) =>
        {
            return r.Id == c.ParentId;
        },
        (r, datalist) =>
        {
            r.Childs ??= new List<ApiEntity>();
            r.Childs.AddRange(datalist);
        });

        #endregion

        #region 视图

        var views = await db.Queryable<ViewEntity>().ToListAsync();
        var viewTree = views.Clone().ToTree((r, c) =>
        {
            return c.ParentId == 0;
        },
       (r, c) =>
       {
           return r.Id == c.ParentId;
       },
       (r, datalist) =>
       {
           r.Childs ??= new List<ViewEntity>();
           r.Childs.AddRange(datalist);
       });

        #endregion

        #region 权限

        var permissions = await db.Queryable<PermissionEntity>().ToListAsync();
        var permissionTree = permissions.Clone().ToTree((r, c) =>
        {
            return c.ParentId == 0;
        },
       (r, c) =>
       {
           return r.Id == c.ParentId;
       },
       (r, datalist) =>
       {
           r.Childs ??= new List<PermissionEntity>();
           r.Childs.AddRange(datalist);
       });

        #endregion

        #region 用户

        var users = await db.Queryable<UserEntity>().ToListAsync();

        #endregion

        #region 员工

        var staffs = await db.Queryable<UserStaffEntity>().ToListAsync();

        #endregion

        #region 部门

        var orgs = await db.Queryable<OrgEntity>().ToListAsync();
        var orgTree = orgs.Clone().ToTree((r, c) =>
        {
            return c.ParentId == 0;
        },
        (r, c) =>
        {
            return r.Id == c.ParentId;
        },
        (r, datalist) =>
        {
            r.Childs ??= new List<OrgEntity>();
            r.Childs.AddRange(datalist);
        });

        #endregion

        #region 角色

        var roles = await db.Queryable<RoleEntity>().ToListAsync();

        #endregion

        #region 用户角色

        var userRoles = await db.Queryable<UserRoleEntity>().ToListAsync();

        #endregion

        #region 用户部门

        var userOrgs = await db.Queryable<UserOrgEntity>().ToListAsync();

        #endregion

        #region 角色权限

        var rolePermissions = await db.Queryable<RolePermissionEntity>().ToListAsync();

        #endregion

        #region 租户

        var tenants = await db.Queryable<TenantEntity>().ToListAsync();

        #endregion

        #region 租户权限

        var tenantPermissions = await db.Queryable<TenantPermissionEntity>().ToListAsync();

        #endregion

        #region 权限接口

        var permissionApis = await db.Queryable<PermissionApiEntity>().ToListAsync();

        #endregion

        #endregion

        #region 生成数据

        var isTenant = appConfig.Tenant;

        if (isTenant)
        {
            var tenantIds = tenants?.Select(a => a.Id)?.ToList();
            SaveDataToJsonFile<UserEntity>(users.Where(a => tenantIds.Contains(a.TenantId.Value)));
            SaveDataToJsonFile<RoleEntity>(roles.Where(a => tenantIds.Contains(a.TenantId.Value)));
            orgTree = orgs.Clone().Where(a => tenantIds.Contains(a.TenantId.Value)).ToList().ToTree((r, c) =>
            {
                return c.ParentId == 0;
            },
            (r, c) =>
            {
                return r.Id == c.ParentId;
            },
            (r, datalist) =>
            {
                r.Childs ??= new List<OrgEntity>();
                r.Childs.AddRange(datalist);
            });
            SaveDataToJsonFile<OrgEntity>(orgTree);
            SaveDataToJsonFile<UserStaffEntity>(staffs.Where(a => tenantIds.Contains(a.TenantId.Value)));
        }

        SaveDataToJsonFile<UserEntity>(users, isTenant);
        SaveDataToJsonFile<RoleEntity>(roles, isTenant);
        SaveDataToJsonFile<OrgEntity>(orgTree, isTenant);
        SaveDataToJsonFile<UserStaffEntity>(staffs, isTenant);
        
        SaveDataToJsonFile<DictEntity>(dictionaries);
        SaveDataToJsonFile<DictTypeEntity>(dictionaryTypes);
        SaveDataToJsonFile<UserRoleEntity>(userRoles);
        SaveDataToJsonFile<UserOrgEntity>(userOrgs);
        SaveDataToJsonFile<ApiEntity>(apiTree);
        SaveDataToJsonFile<ViewEntity>(viewTree);
        SaveDataToJsonFile<PermissionEntity>(permissionTree);
        SaveDataToJsonFile<PermissionApiEntity>(permissionApis);
        SaveDataToJsonFile<RolePermissionEntity>(rolePermissions);
        SaveDataToJsonFile<TenantEntity>(tenants);
        SaveDataToJsonFile<TenantPermissionEntity>(tenantPermissions);
        #endregion
    }
}
