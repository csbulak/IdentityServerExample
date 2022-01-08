using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UdemyIdentityServer.AuthServer.Models;

namespace UdemyIdentityServer.AuthServer.Repository
{
    public class CustomUserRepository : ICustomUserRepository
    {
        private readonly CustomDbContext _context;

        public CustomUserRepository(CustomDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Validate(string email, string password)
        {
            return await _context.CustomUsers.AnyAsync(x => x.Email == email && x.Password == password);
        }

        public async Task<CustomUser> FindById(int id)
        {
            return await _context.CustomUsers.FindAsync(id);
        }

        public async Task<CustomUser> FindByEmail(string email)
        {
            return await _context.CustomUsers.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}