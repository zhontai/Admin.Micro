using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.App.Api.Core.Repositories;
using ZhonTai.Module.App.Api.Domain.Module;

namespace ZhonTai.Module.App.Api.Repositories
{
    /// <summary>
    /// Ä£¿é²Ö´¢
    /// </summary>
    public class ModuleRepository : AppRepositoryBase<ModuleEntity>, IModuleRepository
    {
        public ModuleRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
        {
        }
    }
}