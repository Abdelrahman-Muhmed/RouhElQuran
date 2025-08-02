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
        protected IActionResult SuccessResponse<T>(T data, string? message = null)
           => Ok(new ApiResponse<T>(data, message ?? "Success"));


        protected IActionResult ErrorResponse(string? message = null, int statusCode = 400)
        {
            var finalMessage = message ?? "An unexpected error occurred.";
            return StatusCode(statusCode, new ApiResponse<string>(finalMessage, false));
        }


        protected IActionResult NotFoundResponse(string? message = null)
           => NotFound(new ApiResponse<string>(message ?? "Not Found", false));

        protected IActionResult UnauthorizedResponse(string? message = null)
           => Unauthorized(new ApiResponse<string>(message ?? "Unauthorized Please Login", false));
    }
}
