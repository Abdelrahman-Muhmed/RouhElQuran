

namespace Core.IServices.UserService
{
    public interface IUserService<TEntity, TDto> where TEntity : class where TDto : class
    {
        public Task<IEnumerable<TDto>> GetAllUser();
        public Task<TDto> GetUserById(int id);
        public Task<TDto> CreateUser(TDto userDto);
        public TDto UpdateUser(TDto userDto);
        public Task<bool> DeleteUser(int id);
    }
}
