using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.Tenant;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class TenantRepository : AdminRepositoryBase<TenantEntity>, ITenantRepository
{
    public TenantRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {
    }
}