using arts_core.Data;
using arts_core.Models;
using Microsoft.EntityFrameworkCore;

namespace arts_core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        void CreateOwner(User owner);
        void UpdateOwner(User owner);
        void DeleteOwner(User owner);
    }
    
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(DataContext dataContext, ILogger<UserRepository> logger) : base(dataContext)
        {
            _logger = logger;
        }

        public void CreateOwner(User owner)
        {
            Add(owner);
        }

        public void DeleteOwner(User owner)
        {
            Delete(owner);
        }
        public void UpdateOwner(User owner)
        {
            Update(owner);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int ownerId)
        {
            return await _context.Users.FindAsync(ownerId);
        }


    }

}
