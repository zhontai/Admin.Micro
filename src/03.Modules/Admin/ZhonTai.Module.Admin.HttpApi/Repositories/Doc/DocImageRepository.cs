using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.DocImage;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class DocImageRepository : AdminRepositoryBase<DocImageEntity>, IDocImageRepository
{
    public DocImageRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}