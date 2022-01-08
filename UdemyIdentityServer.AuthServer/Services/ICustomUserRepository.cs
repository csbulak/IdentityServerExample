using System.Threading.Tasks;
using UdemyIdentityServer.AuthServer.Models;

namespace UdemyIdentityServer.AuthServer.Services
{
    public interface ICustomUserRepository
    {
        Task<bool> Validate(string email, string password);
        Task<CustomUser> FindById(int id);
        Task<CustomUser> FindByEmail(string email);
    }
}