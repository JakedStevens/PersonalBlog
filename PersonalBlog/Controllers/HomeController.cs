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
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly PersonalBlogDbContext _context;

		public HomeController(ILogger<HomeController> logger, PersonalBlogDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public async Task<ViewResult> Index()
		{
			List<BlogPost> posts = await _context.BlogPost.ToListAsync();
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
