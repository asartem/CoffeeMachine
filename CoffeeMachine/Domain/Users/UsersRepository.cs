using Cm.Domain.Common.Dal;

namespace Cm.Domain.Users
{

    public interface IUsersRepository: IRepository<User>
    {
    }
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
