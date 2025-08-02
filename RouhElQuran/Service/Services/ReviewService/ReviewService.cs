using Azure;
using Core.IUnitOfWork;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Repository.Helper;
using Service.Dto_s;
using Service.IServices.ReviewService;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.ReviewsService
{
    public class ReviewService : ServiceBase, IReviewService
    {
        public ReviewService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task<ApiResponse<string>> AddReviewAsync(UserReviewDto userReviewDto, int userId)
        {
            try
            {
                var review = new Reviews
                {
                    UserId = userId,
                    Rating = userReviewDto.Rating,
                    Comment = userReviewDto.Comment
                };

                if (userReviewDto.CourseID.HasValue)
                {
                    var course = await _UnitOfWork.CourseRepository.GetByIdAsync(userReviewDto.CourseID.Value);
                    if (course is null)
                        return new ApiResponse<string>("Course Not Found");

                    var existingReview = await _UnitOfWork.ReviewRepository.GetFirstOrDefaultAsync(r => r.UserId == userId && r.CourseId == userReviewDto.CourseID.Value);
                    if (existingReview is not null)
                        return new ApiResponse<string>("You have already submitted a review for this course");

                    review.CourseId = userReviewDto.CourseID.Value;
                }
                else if (userReviewDto.InstructorID.HasValue)
                {
                    var instructor = await _UnitOfWork.InstructorRepository.GetByIdAsync(userReviewDto.InstructorID.Value);
                    if (instructor is null)
                        return new ApiResponse<string>("Instructor Not Found");

                    var existingReview = await _UnitOfWork.ReviewRepository.GetFirstOrDefaultAsync(r => r.UserId == userId && r.InstructorId == userReviewDto.InstructorID.Value);
                    if (existingReview is not null)
                        return new ApiResponse<string>("You have already submitted a review for this Instructor");

                    review.InstructorId = userReviewDto.InstructorID.Value;
                }
                else
                {
                    return new ApiResponse<string>("Please Send CourseID or InstructorID");
                }

                await _UnitOfWork.ReviewRepository.AddAsync(review);
                await _UnitOfWork.SaveChangesAsync();
                return new ApiResponse<string>("", "Review added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(ex.Message);
            }
        }

    }
}
