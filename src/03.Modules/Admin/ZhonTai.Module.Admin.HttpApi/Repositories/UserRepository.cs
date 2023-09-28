using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.User;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class UserRepository : AdminRepositoryBase<UserEntity>, IUserRepository
{
    public UserRepository(UnitOfWorkManagerCloud muowm) : base(muowm)
    {

    }
}