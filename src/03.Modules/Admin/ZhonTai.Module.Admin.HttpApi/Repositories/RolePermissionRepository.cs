using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.RolePermission;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class RolePermissionRepository : AdminRepositoryBase<RolePermissionEntity>, IRolePermissionRepository
{
    public RolePermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}