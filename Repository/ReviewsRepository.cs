using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebProject.Data;
using WebProject.Model;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace WebProject.Repository
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly BookReviewContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ReviewsRepository(BookReviewContext context, IMapper mapper, IWebHostEnvironment hostEnvironment) //instance of the context provided by services container
        {
            _context = context;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;

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


        public async Task<int> AddReviewAsync(ReviewModel reviewModel)
        {
            var review = new Reviews()
            {
                UserId = "hhh",

                ReviewBookName = reviewModel.ReviewBookName,

                ReviewTitle = reviewModel.ReviewTitle,

                ReviewBody = reviewModel.ReviewBody,

                ReviewDate = DateTime.Now,

                ReviewImage = await SaveImage(reviewModel.BookCover)



                /* public int CommentId { get; set; }

                 public string ReviewBookName { get; set; }

                 public string ReviewTitle { get; set; }

                 public string ReviewBody { get; set; }

                 public DateTime ReviewDate { get; set; }

                 public string ReviewImage { get; set; }*/

            };


            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return review.Id;
        }
       

        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }
    }
}
