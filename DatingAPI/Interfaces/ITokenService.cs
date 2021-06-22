using System.Threading.Tasks;
using DatingAPI.Entities;

namespace DatingAPI.Interfaces
{
    public interface ITokenService
    {
       Task<string> CreateToken(AppUser appUser);
    }
}