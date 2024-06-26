using arts_core.Data;
using arts_core.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace arts_core.Interfaces
{
    public interface IAddressRepository : IRepository<Address>
    {

        Task<CustomResult> CreateNewAddress(string email,Address address);

        Task<CustomResult> GetUserAddress(string email);

    }
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private readonly ILogger<AddressRepository> _logger;
        public AddressRepository(ILogger<AddressRepository> logger, DataContext dataContext) : base(dataContext)
        {
            _logger = logger;
        }

        public async Task<CustomResult> CreateNewAddress(string email, Address address)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

                var total = _context.Addresses.Where(a => a.UserId == user.Id).Count();

                var newAddress = new Address()
                {
                    FullName = address.FullName,
                    PhoneNumber = address.PhoneNumber,
                    Ward = address.Ward,
                    Province = address.Province,
                    District = address.District,
                    AddressDetail = address.AddressDetail,
                    IsDefault = address.IsDefault,
                    UserId = user.Id,
                };

                if (total == 0)
                {
                    newAddress.IsDefault = true;
                }

                if (newAddress.IsDefault == true && total != 0)
                {
                    var oldDefaultAddress = await _context.Addresses.SingleOrDefaultAsync(a => a.UserId == user.Id && a.IsDefault == true);

                    if (oldDefaultAddress != null)
                    {
                        oldDefaultAddress.IsDefault = false;
                        _context.Addresses.Update(newAddress);
                    }
                }

                _context.Addresses.Add(newAddress);
                await _context.SaveChangesAsync();

                return new CustomResult(200, "Success", newAddress);
            }catch(Exception ex)
            {
                return new CustomResult(400, "Failed", ex.Message);
            }

        }

        public async Task<CustomResult> GetUserAddress(string email)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

                var listAddress = await _context.Addresses.Where(a => a.UserId == user.Id).OrderByDescending(a => a.Id).ToListAsync();

                return new CustomResult(200, "Success", listAddress);
            } catch (Exception ex)
            {
                return new CustomResult(400, "Failed", ex.Message);
            }

        }
    }
}
