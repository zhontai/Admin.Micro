using ZhonTai.Api.Core.Db.Transaction;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class OprationLogRepository : AdminRepositoryBase<OprationLogEntity>, IOprationLogRepository
{
    public OprationLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}