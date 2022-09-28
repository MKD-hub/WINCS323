using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Data;
using WebProject.Model;

namespace WebProject.Helpers
{
    public class ReviewsMapper : Profile
    {
        public ReviewsMapper()
        {
            //This way we only have to change the Model & Review.cs and nothing on the front-end.
            CreateMap<Reviews, ReviewModel>().ReverseMap();
        }
    }
}
