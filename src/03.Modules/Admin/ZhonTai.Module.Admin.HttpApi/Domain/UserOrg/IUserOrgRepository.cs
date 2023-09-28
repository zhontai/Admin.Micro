﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ZhonTai.Api.Core.Repositories;

namespace ZhonTai.Module.Admin.HttpApi.Domain.UserOrg;

public interface IUserOrgRepository : IRepositoryBase<UserOrgEntity>
{
    /// <summary>
    /// 本部门下是否有员工
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> HasUser(long id);

    /// <summary>
    /// 部门列表下是否有员工
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    Task<bool> HasUser(List<long> idList);
}