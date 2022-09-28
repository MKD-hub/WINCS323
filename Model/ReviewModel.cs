using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebProject.Model
{
    public class ReviewModel
    {
        public int Id { get; set; }

        
        //We resolve this one through user_manager after creating Login
        public string UserId { get; set; }

        //Might Delete this later
        public int CommentId { get; set; }

        [Required]
        public string ReviewBookName { get; set; }

        [Required]
        public string ReviewTitle { get; set; }

        [Required]
        public string ReviewBody { get; set; }

        [Required]
        public DateTime ReviewDate { get; set; }

        public string ReviewImage { get; set; }

        [Required]
        [NotMapped]

        public IFormFile BookCover { get; set; }
    }
}
