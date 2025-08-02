using Azure;
using Core.IRepo;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Dto_s;
using Service.IServices.ReviewService;

namespace RouhElQuran.Controllers
{
    public class ReviewsController : BaseController
    {
        private readonly IReviewService _ReviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _ReviewService = reviewService;
        }

        [HttpPost("AddReview")]
        [Authorize]
        public async Task<IActionResult> AddUserReview(UserReviewDto userReviewDto)
        {
            // Check user ID from token
            var userId = UserId;
            if (userId == null)
                return UnauthorizedResponse("Please Login First");

            var response = await _ReviewService.AddReviewAsync(userReviewDto, userId.Value);

            if (!response.Success)
                return ErrorResponse(response.Message);

            return SuccessResponse(response.Message);
        }
    }
}