using System.Collections.Generic;
using System.Threading.Tasks;
using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.Org;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class OrgRepository : AdminRepositoryBase<OrgEntity>, IOrgRepository
{
    public OrgRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {
    }

    /// <summary>
    /// 获得本部门和下级部门Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<List<long>> GetChildIdListAsync(long id)
    {
        return await Select
        .Where(a => a.Id == id)
        .AsTreeCte()
        .ToListAsync(a => a.Id);
    }
}