using Repository.Models;
using Core.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices.UserService
{
	public interface IUserService<TEntity, TDto> 
		where TEntity : class
		where TDto : class
	{
		public Task<IEnumerable<TDto>> GetAllUser();
		public Task<TDto> GetUserById(int id);
		public Task<TDto> CreateUser(TDto instructorDto);
		public Task<TDto> updateUser(TDto instructorDto);
		public Task<TDto> DeleteUser(int id);
	}
}
