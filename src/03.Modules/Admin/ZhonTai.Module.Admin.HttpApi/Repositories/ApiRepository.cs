using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.Api;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class ApiRepository : AdminRepositoryBase<ApiEntity>, IApiRepository
{
    public ApiRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}