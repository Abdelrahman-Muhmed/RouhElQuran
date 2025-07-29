using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouhElQuran.AccountService;
using Service.Dto_s;

namespace RouhElQuran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult LoginUser()
        {
            return Ok("good i am here");
        }


    }
}
