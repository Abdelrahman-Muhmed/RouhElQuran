using AutoMapper;
using Core.IRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using RouhElQuran.Dto_s;

namespace RouhElQuran.Controllers
{
    [Route("api/Instructor")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IGenericrepo<Instructor> GenericRepository;
        private readonly IMapper _mapper;

        public InstructorController(IGenericrepo<Instructor> genericRepository, IMapper mapper)
        {
            GenericRepository = genericRepository ??
              throw new ArgumentNullException(nameof(genericRepository)); //Check If InstructoRepository Is Null Or Not
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper)); //Check If mapper Is Null Or Not
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var allInst = await GenericRepository.GetAllAsync();
            var InstMapp = _mapper.Map<IEnumerable<InstructorDto>>(allInst);
            return Ok(InstMapp);
        }

        //Get By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var insById = await GenericRepository.GetByIdAsync(id);
            if (insById == null)
            {
                return NotFound("Instructor not found");
            }
            var InsIdMapp = _mapper.Map<InstructorDto>(insById);
            return Ok(InsIdMapp);
        }

        //Add New Instructor
        [HttpPost("Add")]
        public async Task<IActionResult> Add(InstructorDto instructor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var InstructorMapped = _mapper.Map<Instructor>(instructor);
                    await GenericRepository.AddAsync(InstructorMapped);
                    return Ok("Instructor Added Successfully");
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
                }
            }
            return BadRequest("Invalid instructor");
        }

        //Update Instructor
        [HttpPut("Update")]
        public async Task<IActionResult> update(InstructorDto instructorDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var InstructorMapped = _mapper.Map<Instructor>(instructorDto);

                    await GenericRepository.UpdateAsync(InstructorMapped);
                    return Ok("Instructor Update successfully");
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
                }
            }
            return BadRequest("Invalid instructor");
        }

        //Remove Instructor
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletInst = await GenericRepository.DeleteAsync(id);
                if (deletInst != null)
                    return Ok("Instructor Deleted successfully");
                else
                    return NotFound("Instructor not found");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
            }
        }
    }
}