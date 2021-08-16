using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalBlog.ClassLibrary;
using PersonalBlog.Web.Models;
using PersonalBlog.Web.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PersonalBlog.Web.Controllers
{
	[AllowAnonymous]
	public class HomeController : Controller
	{
		private readonly PersonalBlogDbContext _dbContext;

		public HomeController(PersonalBlogDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ViewResult> Home()
		{
			List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();
			BlogPostsViewModel postVM = new BlogPostsViewModel() { Posts = posts };
			return View(postVM);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
