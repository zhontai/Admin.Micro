using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.TenantPermission;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class TenantPermissionRepository : AdminRepositoryBase<TenantPermissionEntity>, ITenantPermissionRepository
{
    public TenantPermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}