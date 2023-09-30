using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.OprationLog;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class OprationLogRepository : AdminRepositoryBase<OprationLogEntity>, IOprationLogRepository
{
    public OprationLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}