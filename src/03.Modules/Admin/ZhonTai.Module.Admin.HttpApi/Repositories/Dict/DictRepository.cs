using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.Dict;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class DictRepository : AdminRepositoryBase<DictEntity>, IDictRepository
{
    public DictRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}