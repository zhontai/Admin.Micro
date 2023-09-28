using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain;
using ZhonTai.Module.Admin.HttpApi.Domain.RoleOrg;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class RoleOrgRepository : AdminRepositoryBase<RoleOrgEntity>, IRoleOrgRepository
{
    public RoleOrgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}