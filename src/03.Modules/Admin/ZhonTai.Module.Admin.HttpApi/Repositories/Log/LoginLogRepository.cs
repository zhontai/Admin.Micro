using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.LoginLog;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class LoginLogRepository : AdminRepositoryBase<LoginLogEntity>, ILoginLogRepository
{
    public LoginLogRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}