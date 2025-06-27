using AutoMapper;
using Core.IRepo;
using Core.IServices.UserService;
using Repository.Models;
using Core.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.UserService
{
	public class UserService<TEntity,TDto> : IUserService<TEntity,TDto> 
		where TEntity : class
		where TDto : class
	{
		private readonly IGenericrepo<TEntity> _userRepository;
		private readonly IMapper _mapper; 
        public UserService(IGenericrepo<TEntity> userRepository, IMapper mapper)
        {
			_userRepository = userRepository;
			_mapper = mapper;

		}

		public async Task<IEnumerable<TDto>> GetAllUser()
		{
			var result = await _userRepository.GetAllAsync();
			return _mapper.Map<IEnumerable<TDto>>(result);

		}

		public async Task<TDto> GetUserById(int id)
		{
			var result = await _userRepository.GetByIdAsync(id);
			if (result == null)
			{
				return null;
			}

			return _mapper.Map<TDto>(result);
		}

		public async Task<TDto> CreateUser(TDto userDto)
		{
			var mapData = _mapper.Map<TEntity>(userDto);
			var resultEntity = await _userRepository.AddAsync(mapData);
			var resultDto = _mapper.Map<TDto>(resultEntity);
			return resultDto;

		}

		public async Task<TDto> updateUser(TDto userDto)
		{
			var mapData =  _mapper.Map<TEntity>(userDto);
			  await _userRepository.UpdateAsync(mapData);
			return userDto;



		}
		public async Task<TDto> DeleteUser(int id)
		{
			var result = await _userRepository.DeleteAsync(id);
			var mapData = _mapper.Map<TDto>(result);

			return mapData;
		}

	


	}
}
