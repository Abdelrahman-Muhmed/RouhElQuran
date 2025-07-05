using AutoMapper;
using Core.Dto_s;
using Core.IRepo;
using Core.IServices.InstructorService;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Repository.Repos;
using Service.Helper.CalculatHelper;
using Service.Helper.FileUploadHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.InstructorService
{
    public class InstructorService : IInstructorService
    {
        private readonly IGenericrepo<Instructor> _GenericrInstructorepo;

        private readonly IGenericrepo<Files> _fileGenericRepo;

        private readonly IMapper _mapper;
        public InstructorService(IGenericrepo<Instructor> GenericrInstructorepo, IMapper mapper , IGenericrepo<Files> fileGenericRepo)
        {
            _GenericrInstructorepo = GenericrInstructorepo;
            _mapper = mapper;
            _fileGenericRepo = fileGenericRepo;

        }

        public async Task<IEnumerable<InstructorDto>> GetAllInstructor()
        {
            var result = await _GenericrInstructorepo.GetAllAsync();
            var resultMap = _mapper.Map<IEnumerable<InstructorDto>>(result);

            return resultMap;

        }

        public async Task<InstructorDto> GetInstructorById(int? id)
        {
            var result = await _GenericrInstructorepo.GetByIdAsync(id);
            var resultMap = _mapper.Map<InstructorDto>(result);
            return resultMap;

        }
        public async Task<Instructor> CreateInstructor(InstructorDto instructorDto , HttpRequest request)
        {
            try
            {
                await _GenericrInstructorepo.BeginTransactionAsync();
                var resultMap = _mapper.Map<Instructor>(instructorDto);

                resultMap.YearsOfExperience = CalculatHelper.calculatYearsOfExperience(resultMap.WorkExperienceTo , resultMap.WorkExperienceFrom);
                var result = await _GenericrInstructorepo.AddAsync(resultMap);

                var fileContent = await FileHelper.streamedOrBufferedProcess(request, instructorDto.FileUpload, _fileGenericRepo, userId: instructorDto.InsUser_Id);

                return result;
            }
            catch
            {
                await _GenericrInstructorepo.RollbackTransactionAsync();
                throw;

            }
        }
        public async Task<Instructor> updateInstructor(InstructorDto instructorDto)
        {
            var resultMap = _mapper.Map<Instructor>(instructorDto);
            var result = await _GenericrInstructorepo.UpdateAsync(resultMap);
            return result;
        }
        public async Task<Instructor> DeleteInstructor(int? id)
        {
           var result = await _GenericrInstructorepo.DeleteAsync(id);
            return result;
        }

   

      
    }
}
