using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.PkgPermission;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class PkgPermissionRepository : AdminRepositoryBase<PkgPermissionEntity>, IPkgPermissionRepository
{
    public PkgPermissionRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}