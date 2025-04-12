using AutoMapper;
using Core.IRepo;
using Core.IServices.UserService;
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

        private readonly IUserService<InstructorDto,Instructor> _userService;


        public InstructorController( IUserService<InstructorDto, Instructor> userService)
        {
 
            _userService = userService;

		}

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllUser();

			return Ok(result);
        }

        //Get By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetUserById(id);
            if (result == null)
            {
                return NotFound("Instructor not found");
            }
            return Ok(result);
        }

        //Add New Instructor
        [HttpPost("Add")]
        public async Task<IActionResult> Add(InstructorDto instructor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //await _userService.CreateUser(instructor);
                    return Ok("Instructor Added Successfully");
                }
                catch
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
                }
            }
            return BadRequest("Invalid instructor");
        }

        ////Update Instructor
        //[HttpPut("Update")]
        //public async Task<IActionResult> update(InstructorDto instructorDto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var InstructorMapped = _mapper.Map<Instructor>(instructorDto);

        //            await GenericRepository.UpdateAsync(InstructorMapped);
        //            return Ok("Instructor Update successfully");
        //        }
        //        catch
        //        {
        //            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
        //        }
        //    }
        //    return BadRequest("Invalid instructor");
        //}

        ////Remove Instructor
        //[HttpDelete("delete/{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        var deletInst = await GenericRepository.DeleteAsync(id);
        //        if (deletInst != null)
        //            return Ok("Instructor Deleted successfully");
        //        else
        //            return NotFound("Instructor not found");
        //    }
        //    catch
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
        //    }
        //}
    }
}