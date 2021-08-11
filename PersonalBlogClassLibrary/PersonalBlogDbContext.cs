using Microsoft.EntityFrameworkCore;

namespace PersonalBlog.ClassLibrary
{
	public class PersonalBlogDbContext : DbContext
	{
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=.;Database=PersonalBlog;Trusted_Connection=True");

        public DbSet<BlogPost> BlogPost { get; set; }
    }
}
