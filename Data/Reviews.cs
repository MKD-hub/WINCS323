using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProject.Data
{
    public class Reviews
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int CommentId { get; set; }

        public string ReviewBookName { get; set; }

        public string ReviewTitle { get; set; }

        public string ReviewBody { get; set; }

        public DateTime ReviewDate { get; set; }

        public string ReviewImage { get; set; }


    }
}
