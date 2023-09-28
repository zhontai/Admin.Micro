using ZhonTai.Api.Core.Db.Transaction;
using ZhonTai.Module.Admin.HttpApi.Domain.UserStaff;

namespace ZhonTai.Module.Admin.HttpApi.Repositories;

public class UserStaffRepository : AdminRepositoryBase<UserStaffEntity>, IUserStaffRepository
{
    public UserStaffRepository(UnitOfWorkManagerCloud uowm) : base(uowm)
    {

    }
}