using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Model;

namespace WebProject.Data
{
    public class BookReviewContext : IdentityDbContext<ApplicationUser>
    {
        public BookReviewContext(DbContextOptions<BookReviewContext> options) : base(options)
        {

        }

        public DbSet<Reviews> Reviews { get; set; }
    }


}
