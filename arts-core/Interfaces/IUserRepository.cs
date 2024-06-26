using arts_core.Data;
using arts_core.Models;
using arts_core.RequestModels;
using Faker;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace arts_core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        void CreateOwner(User owner);
        void UpdateOwner(User owner);
        void DeleteOwner(User owner);

        Task<User> ManagerAuthenticate(LoginRequest account);

        Task<CustomResult> AdminLogin(LoginRequest account);

        Task<CustomResult> GetAdmin(string email);

        Task<CustomResult> GetEmployees();

        Task<bool> CheckEmailExist(string email);

        Task<bool> CheckPhoneExist(string phone);

        Task<CustomResult> CreateEmployee(CreateEmployee account);

        Task<CustomPaging> GetAllEmployees(int pageNumber, int pageSize);

        Task<CustomResult> CreateCustomer(CreateCustomer account);

        Task<CustomResult> CustomerLogin(LoginRequest account);

        Task<User> CustomerAuthenticate(LoginRequest account);

        Task<CustomResult> GetUser(string email);

        Task<CustomResult> ChangeUserImage(string email, IFormFile image);

        Task<CustomResult> EditUserInfo(string email, UpdateUserRequest info);
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        public UserRepository(DataContext dataContext, ILogger<UserRepository> logger, IConfiguration configuration, IWebHostEnvironment env) : base(dataContext)
        {
            _logger = logger;
            _config = configuration;
            _env = env;
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

        public async Task<User> ManagerAuthenticate(LoginRequest account)
        {
            try
            {
                var verified = await _context.Users.Include(u => u.RoleType).Where(u => u.Email == account.Email && (u.RoleType.Name == "Admin" || u.RoleType.Name == "Employee")).SingleOrDefaultAsync();

                if (verified != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(account.Password, verified.Password))
                    {
                        return verified;
                    }
                }

                return null;

            }
            catch (Exception ex)
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
            var user = await ManagerAuthenticate(account);

            if (user == null)
            {
                return new CustomResult(404, "Not Found", null);
            }

            var token = CreateToken(user);

            return new CustomResult(200, "token", token);
        }

        public async Task<CustomResult> GetAdmin(string email)
        {
            var user = await _context.Users.Include(u => u.RoleType).SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return new CustomResult(400, "Bad Request", null);
            }

            return new CustomResult(200, "Success", user);
        }

        public async Task<CustomResult> GetEmployees()
        {
            try
            {
                var list = await _context.Users.Include(u => u.RoleType).Where(u => u.RoleType.Name == "Employee").ToListAsync();

                return new CustomResult(200, "success", list);
            }
            catch (Exception ex)
            {
                return new CustomResult(400, "Bad request", ex.Message);
            }
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            try
            {
                var verified = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

                if (verified == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CheckPhoneExist(string phone)
        {
            try
            {
                var verified = await _context.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phone);

                if (verified == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<CustomResult> CreateEmployee(CreateEmployee account)
        {
            try
            {
                var verifiedEmail = await CheckEmailExist(account.Email);

                if (verifiedEmail == true)
                {
                    return new CustomResult(400, "Email already exist", null);
                }
                if (account.Phone != null)
                {
                    var verifiedPhone = await CheckPhoneExist(account.Phone);

                    if (verifiedPhone == true)
                    {
                        return new CustomResult(400, "Phone number already exist", null);
                    }

                }

                var employeeRole = await _context.Types.SingleOrDefaultAsync(r => r.NameType == "UserRole" && r.Name == "Employee");

                var employee = new User()
                {
                    Email = account.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(account.Password),
                    Active = true,
                    PhoneNumber = account.Phone,
                    Address = account.Address,
                    RoleType = employeeRole,
                };

                if (account.Avatar != null)
                {
                    var fileName = DateTime.Now.Ticks + account.Avatar.FileName;
                    var uploadPath = Path.Combine(_env.WebRootPath, "images");
                    var filePath = Path.Combine(uploadPath, fileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    account.Avatar.CopyTo(stream);
                    employee.Avatar = fileName;
                }

                _context.Users.Add(employee);

                return new CustomResult(200, "success", employee);
            }
            catch (Exception ex)
            {
                return new CustomResult(400, "failed", ex.Message);
            }

        }

        public async Task<CustomPaging> GetAllEmployees(int pageNumber, int pageSize)
        {
            var list = await _context.Users.Include(u => u.RoleType).Where(u => u.RoleType.Name == "Employee").Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            var total = _context.Users.Include(u => u.RoleType).Where(u => u.RoleType.Name == "Employee").Count();

            var customPaging = new CustomPaging()
            {
                Status = 200,
                Message = "OK",
                CurrentPage = pageNumber,
                TotalPages = (total / pageSize),
                PageSize = pageSize,
                TotalCount = total,
                Data = list
            };

            return customPaging;
        }

        public async Task<CustomResult> CreateCustomer(CreateCustomer account)
        {
            try
            {
                var verifiedEmail = await CheckEmailExist(account.Email);

                if (verifiedEmail == true)
                {
                    return new CustomResult(400, "Email already exist", null);
                }

                var verifiedPhone = await CheckPhoneExist(account.PhoneNumber);

                if (verifiedPhone == true)
                {
                    return new CustomResult(400, "Phone number already exist", null);
                }

                var customerRole = await _context.Types.SingleOrDefaultAsync(r => r.NameType == "UserRole" && r.Name == "Customer");

                var customer = new User()
                {
                    Email = account.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(account.Password),
                    Active = true,
                    PhoneNumber = account.PhoneNumber,
                    Fullname = account.Name,
                    RoleType = customerRole,
                };

                _context.Users.Add(customer);
                return new CustomResult(200, "success", customer);
            }
            catch (Exception ex)
            {
                return new CustomResult(400, "failed", ex.Message);
            }
        }

        public async Task<CustomResult> CustomerLogin(LoginRequest account)
        {
            var user = await CustomerAuthenticate(account);

            if (user == null)
            {
                return new CustomResult(404, "Not Found", null);
            }

            var token = CreateToken(user);

            return new CustomResult(200, "token", token);
        }

        public async Task<User> CustomerAuthenticate(LoginRequest account)
        {
            try
            {
                var verified = await _context.Users.Include(u => u.RoleType).Where(u => u.Email == account.Email && (u.RoleType.Name == "Customer")).SingleOrDefaultAsync();

                if (verified != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(account.Password, verified.Password))
                    {
                        return verified;
                    }
                }

                return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<CustomResult> GetUser(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return new CustomResult(400, "Bad Request", null);
            }

            return new CustomResult(200, "Success", user);
        }

        public async Task<CustomResult> ChangeUserImage(string email, IFormFile image)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
                var fileName = DateTime.Now.Ticks + image.FileName;
                var uploadPath = Path.Combine(_env.WebRootPath, "images");
                var filePath = Path.Combine(uploadPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                image.CopyTo(stream);
                user.Avatar = fileName;

                _context.Users.Update(user);

                await _context.SaveChangesAsync();

                return new CustomResult(200, "Success", user);
            }
            catch(Exception ex)
            {
                return new CustomResult(400, "Failed", ex.Message);
            }


        }

        public async Task<CustomResult> EditUserInfo(string email, UpdateUserRequest info)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

                user.Fullname = info.FullName;
                user.Dob = DateTime.Parse(info.Dob);
                user.Gender = info.Gender;

                if (user.PhoneNumber != info.PhoneNumber)
                {
                    var verified = await CheckPhoneExist(info.PhoneNumber);
                    if (verified)
                    {
                        return new CustomResult(400, "Phone number exist", null);
                    }

                    user.PhoneNumber = info.PhoneNumber;
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return new CustomResult(200, "Success", user);
            }
            catch(Exception ex)
            {
                return new CustomResult(400, "Failed", ex.Message);
            }



        }
    }

}
