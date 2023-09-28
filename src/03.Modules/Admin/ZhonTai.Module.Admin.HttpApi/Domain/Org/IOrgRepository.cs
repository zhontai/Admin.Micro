using System.Collections.Generic;
using System.Threading.Tasks;
using ZhonTai.Api.Core.Repositories;

namespace ZhonTai.Module.Admin.HttpApi.Domain.Org;

public interface IOrgRepository : IRepositoryBase<OrgEntity>
{
    /// <summary>
    /// 获得本部门和下级部门Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<long>> GetChildIdListAsync(long id);
}