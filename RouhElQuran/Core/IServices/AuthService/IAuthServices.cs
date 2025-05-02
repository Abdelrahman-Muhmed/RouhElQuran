using Repository.Models;
using Core.Dto_s;

namespace RouhElQuran.AccountService
{
    public interface IAuthServices
    {
        Task<object> LoginUser(LoginDto login);

        Task<string> CreateToken(AppUser user);
    }
}