

namespace Core.IServices.UserService
{
	public interface IUserService<TEntity, TDto> 
		where TEntity : class
		where TDto : class
	{
		public Task<IEnumerable<TDto>> GetAllUser();
		public Task<TDto> GetUserById(int id);
		public Task<TDto> CreateUser(TDto userDto);
		public Task<TDto> updateUser(TDto userDto);
		public Task<TDto> DeleteUser(int id);
	}
}
