using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.DictType;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class DictTypeRepository : AdminRepositoryBase<DictTypeEntity>, IDictTypeRepository
{
    public DictTypeRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}