using AutoMapper;
using Core.IRepo;
using Core.IServices.UserService;
using Core.IUnitOfWork;

namespace Service.Services.UserService
{
    public class UserService<TEntity, TDto> : ServiceBase, IUserService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IGenericRepository<TEntity> _UserRepository;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IGenericRepository<TEntity> userRepository, IMapper mapper) : base(unitOfWork)
        {
            _UserRepository = userRepository;
            _mapper = mapper;

        }

        public async Task<IEnumerable<TDto>> GetAllUser()
        {
            var result = await _UserRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(result);

        }

        public async Task<TDto> GetUserById(int id)
        {
            var result = await _UserRepository.GetByIdAsync(id);
            if (result == null)
            {
                return null;
            }

            return _mapper.Map<TDto>(result);
        }

        public async Task<TDto> CreateUser(TDto userDto)
        {
            var mapData = _mapper.Map<TEntity>(userDto);
            await _UserRepository.AddAsync(mapData);
            var resultDto = _mapper.Map<TDto>(mapData);
            return resultDto;

        }

        public TDto UpdateUser(TDto userDto)
        {
            var mapData = _mapper.Map<TEntity>(userDto);
            _UserRepository.Update(mapData);
            return userDto;



        }

        // TODO: there is no save changes here, should be handled
        public async Task<bool> DeleteUser(int id)
        {
            var entity = await _UserRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return false;
            }
             _UserRepository.Remove(entity);

            return true;
        }




    }
}
