using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Data;
using WebProject.Model;
using WebProject.Repository;

namespace WebProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<BookReviewContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BookReviewsDB")));

            /*Whenever the above line executes it creates a session with the DB during the lifetime of the HTTP request invoking it. When it's invoked it creates a DBContext
             of type BookReviewContext and pass the options provided establish a session with the DB.*/

            services.AddIdentity<ApplicationUser, IdentityRole>().
                AddEntityFrameworkStores<BookReviewContext>().
                AddDefaultTokenProviders();

            services.AddControllers(); 
            services.AddTransient<IReviewsRepository, ReviewsRepository>();
            services.AddAutoMapper(typeof(Startup));
            services.AddCors(option => 
            {
                option.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           
            app.UseStaticFiles(new StaticFileOptions
            {  
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Images")),
                RequestPath = "/Images"
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(); // must be used here since ASP goes through the middlewares in the order they're presented

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
