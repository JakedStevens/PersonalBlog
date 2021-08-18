using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.Web.Models;
using PersonalBlog.Web.ViewModels;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PersonalBlog.Web.Controllers
{
	[AllowAnonymous]
	public class HomeController : Controller
	{
		private readonly DataManager _data;

		public HomeController(DataManager data)
		{
			_data = data;
		}

		public async Task<ViewResult> Home()
		{
			BlogPostsViewModel postVM = await _data.CreateAllPostsVM();

			return View(postVM);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
