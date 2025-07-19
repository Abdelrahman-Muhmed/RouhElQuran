using AutoMapper;
using Core.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using NuGet.Protocol.Core.Types;
using Org.BouncyCastle.Utilities;
using Repository.Models;
using Core.Dto_s;

namespace RouhElQuran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IGenericrepo<Student> genericrepo;
        private readonly IMapper map;

        public StudentController(IGenericrepo<Student> _genericrepo, IMapper _map)
        {
            genericrepo = _genericrepo;
            map = _map;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> Getall()
        {
            IEnumerable<Student> students =  genericrepo.GetAllAsync();
            return Ok(map.Map<List<StudentDto>>(students));
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    if (id == 0)
        //        return BadRequest();
        //    else
        //    {
        //        Student stu = await genericrepo.GetByIdAsync(id);
        //        if (stu == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(map.Map<StudentDto>(stu));
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Save(StudentDto student)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            else
            {
                try
                {
                    var x = new Student()
                    {
                        StdUser_Id = student.UserId,
                        CountHours = student.CountHours,
                    };
                    await genericrepo.BeginTransactionAsync();
                    await genericrepo.AddAsync(x);
                    await genericrepo.CommitTransactionAsync();
                    return Created();
                }
                catch (Exception)
                {
                    await genericrepo.RollbackTransactionAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred While Adding");
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            if (id == 0)
                return BadRequest();
            else
            {
                try
                {
                    await genericrepo.BeginTransactionAsync();
                    var stu = await genericrepo.DeleteAsync(id);
                    if (stu == null)
                        return NotFound(id);
                    await genericrepo.CommitTransactionAsync();
                    return Ok("deleted succsesfuly");
                }
                catch (Exception)
                {
                    await genericrepo.RollbackTransactionAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred While Deleting");
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse(StudentDto student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var x = new Student()
                    {
                        StdUser_Id = student.UserId,
                        CountHours = student.CountHours,
                    };
                    await genericrepo.BeginTransactionAsync();
                    await genericrepo.UpdateAsync(x);
                    await genericrepo.CommitTransactionAsync();
                    return Ok("Update SuccessFully");
                }
                catch
                {
                    await genericrepo.RollbackTransactionAsync();
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred While Updating");
                }
            }
            return BadRequest("Invalid Data");
        }
    }
}