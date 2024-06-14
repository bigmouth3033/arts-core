using arts_core.Data;
using arts_core.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace arts_core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        void CreateOwner(User owner);
        void UpdateOwner(User owner);
        void DeleteOwner(User owner);

        Task<User> Authenticate(LoginRequest account);

        Task<CustomResult> AdminLogin(LoginRequest account);

        Task<CustomResult> GetAdmin(string email);
    }
    
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly IConfiguration _config;
        public UserRepository(DataContext dataContext, ILogger<UserRepository> logger, IConfiguration configuration) : base(dataContext)
        {
            _logger = logger;
            _config = configuration;
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

        public async Task<User> Authenticate(LoginRequest account)
        {
            try
            {
                var verified = await _context.Users.Include(u => u.RoleType).Where(u => u.Email == account.Email && u.RoleType.Name == "Admin").SingleOrDefaultAsync();

                if(verified != null)
                {
                    if(BCrypt.Net.BCrypt.Verify(account.Password, verified.Password))
                    {
                        return verified;
                    }
                }

                return null;
                
            }catch(Exception ex)
            {
                return null;
            }
        }

        private string CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleType.Name),
                new Claim("Id", user.Id.ToString()),
            };

            var token = new JwtSecurityToken(
                    issuer: _config["JwtSettings:Issuer"],
                    audience: _config["JwtSettings:Audience"],
                    signingCredentials: credentials,
                    claims: claims,
                    expires: DateTime.Now.AddDays(7)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<CustomResult> AdminLogin(LoginRequest account)
        {
            var user = await Authenticate(account);

            if(user == null)
            {
                return new CustomResult(404, "Not Found", null);
            }

            var token = CreateToken(user);

            return new CustomResult(200, "token", token);
        }

        public async Task<CustomResult> GetAdmin(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if(user == null)
            {
                return new CustomResult(400, "Bad Request", null);
            }

            return new CustomResult(200, "Success", user);
        }
    }

}
