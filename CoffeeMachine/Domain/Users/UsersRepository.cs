using Domain.Common.Dal;

namespace Domain.Users
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
