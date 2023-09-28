using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.File;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class FileRepository : AdminRepositoryBase<FileEntity>, IFileRepository
{
    public FileRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }
}