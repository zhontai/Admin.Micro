using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Api.Core.Repositories;
using ZhonTai.Module.App.Api.Core.Consts;

namespace ZhonTai.Module.App.Api.Core.Repositories
{
    public class AppRepositoryBase<TEntity> : RepositoryBase<TEntity> where TEntity : class
    {
        public AppRepositoryBase(UnitOfWorkManagerCloud uowm) : base(DbKeys.AppDb, uowm)
        {

        }
    }
}