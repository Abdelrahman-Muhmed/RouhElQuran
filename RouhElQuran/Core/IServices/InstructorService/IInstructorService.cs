using Core.Dto_s;
using Microsoft.AspNetCore.Http;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices.InstructorService
{
    public interface IInstructorService
    {
        public Task<IEnumerable<InstructorDto>> GetAllInstructor();
        public Task<InstructorDto> GetInstructorById(int? id);
        public Task<Instructor> CreateInstructor(InstructorDto courseDto, HttpRequest request);
        public Task<Instructor> updateInstructor(InstructorDto courseDto);

        public Task<Instructor> DeleteInstructor(int? id);
    }
}
