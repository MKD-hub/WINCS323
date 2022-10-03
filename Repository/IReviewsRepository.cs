using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Model;




namespace WebProject.Repository
{
    public interface IReviewsRepository
    {
        Task<List<ReviewModel>> GetAllReviewsAsync();
        Task<ReviewModel> GetReviewByIdAsync(int reviewId);


        Task<int> AddReviewAsync(ReviewModel reviewModel);
        
        Task EditReviewAsync(int reviewId, ReviewModel reviewModel);

        Task DeleteReviewAsync(int reviewId);

        Task<List<ReviewModel>> GetUserReviews(string userId);
    }
}
