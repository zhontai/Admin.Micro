using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.TenantPkg;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class TenantPkgRepository : AdminRepositoryBase<TenantPkgEntity>, ITenantPkgRepository
{
    public TenantPkgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}