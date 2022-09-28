using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebProject.Data;
using WebProject.Model;
using AutoMapper;

namespace WebProject.Repository
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly BookReviewContext _context;
        private readonly IMapper _mapper;

        public ReviewsRepository(BookReviewContext context, IMapper mapper) //instance of the context provided by services container
        {
            _context = context;
            _mapper = mapper;

        }


        public async Task<List<ReviewModel>> GetAllReviewsAsync()
        {
            var reviews = await _context.Reviews.ToListAsync();
            var reviewsList = _mapper.Map<List<ReviewModel>>(reviews);

            return reviewsList; 
        }

        public async Task<ReviewModel> GetReviewByIdAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            return _mapper.Map<ReviewModel>(review);
        }
    }
}
