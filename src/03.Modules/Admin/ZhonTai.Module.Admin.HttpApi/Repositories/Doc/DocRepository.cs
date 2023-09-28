using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.Doc;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class DocRepository : AdminRepositoryBase<DocEntity>, IDocRepository
{
    public DocRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}