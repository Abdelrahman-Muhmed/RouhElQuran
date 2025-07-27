using AutoMapper;
using Core.IRepo;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Service.Dto_s;

namespace RouhElQuran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpolyeeController : ControllerBase
    {
        private readonly IGenericrepo<Employee> genericrepo;
        private readonly IMapper map;

        public EmpolyeeController(IGenericrepo<Employee> _genericrepo, IMapper _map)
        {
            genericrepo = _genericrepo;
            map = _map;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> Getall()
        {
            IEnumerable<Employee> employees =  genericrepo.GetAllAsync();
            return Ok(map.Map<List<EmployeeDto>>(employees));
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    if (id == 0)
        //        return BadRequest();
        //    else
        //    {
        //        Employee emp = await genericrepo.GetByIdAsync(id);
        //        if (emp == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(map.Map<EmployeeDto>(emp));
        //    }
        //}

        ///
        [HttpPost]
        public async Task<IActionResult> Save(EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            else
            {
                try
                {
                    var x = new Employee()
                    {
                        EmpUser_Id = employeeDto.UserId,
                        Salary = employeeDto.Salary,
                        HireDate = employeeDto.HireDate
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

        ///
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

        ////
        [HttpPut]
        public async Task<IActionResult> UpdateCourse(EmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var x = new Employee()
                    {
                        EmpUser_Id = employeeDto.UserId,
                        Salary = employeeDto.Salary,
                        HireDate = employeeDto.HireDate
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