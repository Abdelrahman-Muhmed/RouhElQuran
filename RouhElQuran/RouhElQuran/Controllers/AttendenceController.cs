//using AutoMapper;
//using Core.IRepo;
//using Microsoft.AspNetCore.Mvc;
//using Repository.Models;
//using Service.Dto_s;

//namespace RouhElQuran.Controllers
//{
//    public class AttendenceController : BaseController
//    {
//        private readonly IGenericRepository<Attendence> genericrepo;
//        private readonly IMapper map;

//        public AttendenceController(IGenericRepository<Attendence> _genericrepo, IMapper _map)
//        {
//            genericrepo = _genericrepo;
//            map = _map;
//        }

//        [HttpGet("getAll")]
//        public async Task<IActionResult> Getall()
//        {
//            IEnumerable<Attendence> attendences =  genericrepo.GetAllAsync();
//            return Ok(map.Map<List<AttendenceDto>>(attendences));
//        }

//        //[HttpGet("{id}")]
//        //public async Task<IActionResult> GetById(int id)
//        //{
//        //    if (id == 0)
//        //        return BadRequest();
//        //    else
//        //    {
//        //        Attendence att = await genericrepo.GetByIdAsync(id);
//        //        if (att == null)
//        //        {
//        //            return NotFound();
//        //        }
//        //        return Ok(map.Map<AttendenceDto>(att));
//        //    }
//        //}

//        //
//        [HttpPost]
//        public async Task<IActionResult> Save(AttendenceDto attendenceDto)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest();
//            else
//            {
//                try
//                {
//                    Attendence attendence = map.Map<Attendence>(attendenceDto);
//                    //await genericrepo.BeginTransactionAsync();
//                    await genericrepo.AddAsync(attendence);
//                    //await genericrepo.CommitTransactionAsync();
//                    return Created();
//                }
//                catch (Exception)
//                {
//                    //await genericrepo.RollbackTransactionAsync();
//                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred While Adding");
//                }
//            }
//        }

//        //
//        [HttpDelete]
//        public async Task<IActionResult> delete(int id)
//        {
//            if (id == 0)
//                return BadRequest();
//            else
//            {
//                try
//                {
//                    await genericrepo.BeginTransactionAsync();
//                    var stu = await genericrepo.DeleteAsync(id);
//                    if (stu == null)
//                        return NotFound(id);
//                    await genericrepo.CommitTransactionAsync();
//                    return Ok("deleted succsesfuly");
//                }
//                catch (Exception)
//                {
//                    await genericrepo.RollbackTransactionAsync();
//                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred While Deleting");
//                }
//            }
//        }

//        [HttpPut]
//        public async Task<IActionResult> UpdateCourse(AttendenceDto attendenceDto)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    Attendence attendence = map.Map<Attendence>(attendenceDto);
//                    await genericrepo.BeginTransactionAsync();
//                    await genericrepo.UpdateAsync(attendence);
//                    await genericrepo.CommitTransactionAsync();
//                    return Ok("Update SuccessFully");
//                }
//                catch
//                {
//                    await genericrepo.RollbackTransactionAsync();
//                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred While Updating");
//                }
//            }
//            return BadRequest("Invalid Data");
//        }
//    }
//}