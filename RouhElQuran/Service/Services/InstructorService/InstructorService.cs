using AutoMapper;
using Core.Dto_s;
using Core.IRepo;
using Core.IServices.InstructorService;
using Repository.Models;
using Service.Helper.CalculatHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.InstructorService
{
    public class InstructorService : IInstructorService
    {
        private readonly IGenericrepo<Instructor> _Genericrepo;
        private readonly IMapper _mapper;
        public InstructorService(IGenericrepo<Instructor> Genericrepo, IMapper mapper)
        {
            _Genericrepo = Genericrepo;
            _mapper = mapper;

        }

        public async Task<IEnumerable<InstructorDto>> GetAllInstructor()
        {
            var result = await _Genericrepo.GetAllAsync();
            var resultMap = _mapper.Map<IEnumerable<InstructorDto>>(result);

            return resultMap;

        }

        public async Task<InstructorDto> GetInstructorById(int id)
        {
            var result = await _Genericrepo.GetByIdAsync(id);
            var resultMap = _mapper.Map<InstructorDto>(result);
            return resultMap;

        }
        public async Task<Instructor> CreateInstructor(InstructorDto instructorDto)
        {
            var resultMap = _mapper.Map<Instructor>(instructorDto);

            resultMap.YearsOfExperience = CalculatHelper.calculatYearsOfExperience(resultMap.WorkExperienceTo , resultMap.WorkExperienceFrom);
          
            var result = await _Genericrepo.AddAsync(resultMap);
            return result;  
        }
        public async Task<Instructor> updateInstructor(InstructorDto instructorDto)
        {
            var resultMap = _mapper.Map<Instructor>(instructorDto);
            var result = await _Genericrepo.UpdateAsync(resultMap);
            return result;
        }
        public async Task<Instructor> DeleteInstructor(int id)
        {
           var result = await _Genericrepo.DeleteAsync(id);
            return result;
        }

   

      
    }
}
