using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Helper;
using RouhElQuran.Helper_Extensions;
using System.Security.Claims;

namespace RouhElQuran.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        //protected readonly ILogger<BaseController> _logger;

        //protected BaseController(ILogger<BaseController> logger)
        //{
        //    _logger = logger;
        //}

        protected int? UserId
        {
            get
            {
                var claim = User?.FindFirst(ClaimTypes.NameIdentifier);
                return claim != null ? int.Parse(claim.Value) : null;
            }
        }

        protected string? UserEmail
        {
            get
            {
                var claim = User?.FindFirst(ClaimTypes.Email);
                return claim != null ? claim.Value : null;
            }
        }
        protected IActionResult SuccessResponse<T>(T data, string message = "Success")
        {
            return Ok(new ApiResponse<T>(data, message));
        }

        protected IActionResult ErrorResponse(string message, int statusCode = 400)
        {
            //_logger.LogWarning("API Error: {Message}", message);
            return StatusCode(statusCode, new ApiResponse<string>(message, false));
        }

        protected IActionResult NotFoundResponse(string message = "Not Found")
        {
            return NotFound(new ApiResponse<string>(message, false));
        }
    }
}
