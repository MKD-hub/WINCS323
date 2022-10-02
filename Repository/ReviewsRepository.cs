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
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;


namespace WebProject.Repository
{
    public class ReviewsRepository : IReviewsRepository
    {
        private readonly BookReviewContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IServer _server;

        public ReviewsRepository(BookReviewContext context, IMapper mapper, IWebHostEnvironment hostEnvironment, IServer server) //instance of the context provided by services container
        {
            _context = context;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _server = server;

        }


        public async Task<List<ReviewModel>> GetAllReviewsAsync()
        {
            var reviews = await _context.Reviews.ToListAsync();
            var reviewsList = _mapper.Map<List<ReviewModel>>(reviews);

            var address = _server.Features.Get<IServerAddressesFeature>().Addresses;
            var imageUrl = address.ToList<String>();

            foreach (ReviewModel rev in reviewsList)
            {
                rev.ImageSrc = String.Concat(imageUrl[0], "Images/", rev.ReviewImage); //modified to get images for list of all reviews
            }

            return reviewsList; 
        }

        public async Task<ReviewModel> GetReviewByIdAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            var address = _server.Features.Get<IServerAddressesFeature>().Addresses;
            var imageUrl = address.ToList<String>();

            
            var rev = _mapper.Map<ReviewModel>(review);
            rev.ImageSrc = String.Concat(imageUrl[0], "Images/", rev.ReviewImage);
            return rev;
        }


        public async Task<int> AddReviewAsync(ReviewModel reviewModel)
        {
            
            var review = _mapper.Map<Reviews>(reviewModel);

            review.ReviewDate = DateTime.Now;
            review.ReviewImage = await SaveImage(reviewModel.BookCover);
            /*var review = new Reviews()
            {
                UserId = "hhh",

                ReviewBookName = reviewModel.ReviewBookName,

                ReviewTitle = reviewModel.ReviewTitle,

                ReviewBody = reviewModel.ReviewBody,

                ReviewDate = DateTime.Now,

                ReviewImage = await SaveImage(reviewModel.BookCover)



                *//* public int CommentId { get; set; }

                 public string ReviewBookName { get; set; }

                 public string ReviewTitle { get; set; }

                 public string ReviewBody { get; set; }

                 public DateTime ReviewDate { get; set; }

                 public string ReviewImage { get; set; }*//*

            };
*/

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return review.Id;
        }


        public async Task EditReviewAsync(int reviewId, ReviewModel reviewModel)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review != null)
            {
                review.ReviewBody = reviewModel.ReviewBody;  //Users can only update these two fields. They shouldn't be allowed to update everything.
                review.ReviewTitle = reviewModel.ReviewTitle;

                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = new Reviews() { Id = reviewId }; // Didn't "Hit" the DB just created an object here.

            _context.Reviews.Remove(review);

            await _context.SaveChangesAsync();
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
