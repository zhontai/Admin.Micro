using ZhonTai.Api.Core.Consts;
using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Api.Core.Repositories;

namespace ZhonTai.Module.Admin.HttpApi.Repositories
{
    /// <summary>
    /// 权限库基础仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class AdminRepositoryBase<TEntity> : RepositoryBase<TEntity> where TEntity : class
    {
        public AdminRepositoryBase(UnitOfWorkManagerCloud uowm) : base(DbKeys.AppDb, uowm) 
        {
            
        }
    }
}