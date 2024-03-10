using AuthenticationService.DbSets;

namespace AuthenticationService.Repositories
{
    public interface IUserRepository
    {
        Task Add(User user);
        Task Save();
    }
}
