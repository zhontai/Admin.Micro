using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.UserRole;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class UserRoleRepository : AdminRepositoryBase<UserRoleEntity>, IUserRoleRepository
{
    public UserRoleRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}