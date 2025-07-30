using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouhElQuran.AccountService;
using Service.Dto_s;

namespace RouhElQuran.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        [Authorize]
        public IActionResult LoginUser()
        {
            return SuccessResponse("very good man");
        }

        [HttpGet("test")]
        public IActionResult FaieldUser()
        {
            return ErrorResponse("unAuthrized", 401);
        }
    }
}
