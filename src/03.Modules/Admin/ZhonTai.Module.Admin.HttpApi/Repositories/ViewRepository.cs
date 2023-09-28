using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.View;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class ViewRepository : AdminRepositoryBase<ViewEntity>, IViewRepository
{
    public ViewRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {
    }
}