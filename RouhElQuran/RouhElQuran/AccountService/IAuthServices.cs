using Repository.Models;
using RouhElQuran.Dto_s;

namespace RouhElQuran.AccountService
{
    public interface IAuthServices
    {
        Task<object> LoginUser(LoginDto login);

        Task<string> CreateToken(AppUser user);
    }
}