using AuthenticationService.DbSets;

namespace AuthenticationService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _dbContext;

        public UserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(User user)
        {
            await _dbContext.AddAsync<User>(user);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
