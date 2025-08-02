using Core.Models;
using Repository.Helper;
using Service.Dto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.IServices.ReviewService
{
    public interface IReviewService
    {
        Task<ApiResponse<string>> AddReviewAsync(UserReviewDto userReviewDto, int userId);
    }
}
