using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.PermissionApi;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class PermissionApiRepository : AdminRepositoryBase<PermissionApiEntity>, IPermissionApiRepository
{
    public PermissionApiRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}