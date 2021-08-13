using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalBlog.ClassLibrary;
using PersonalBlog.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace PersonalBlog.Web.Controllers
{
    public class UserController : Controller
	{
		private readonly PersonalBlogDbContext _dbContext;
		private readonly UserAuth _auth;

		public UserController(UserAuth auth, PersonalBlogDbContext dbContext)
		{
			_dbContext = dbContext;
			_auth = auth;
		}

		public IActionResult Index()
		{
			return View("Authentication");
		}

		public async Task<ViewResult> SignIn([Bind("Email,Password")] PersonalBlogLoginUser user)
        {
			
			bool areCredentialsValid = _auth.AreCredentialsValid(user);

			List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();
            BlogPostsViewModel postVM = new BlogPostsViewModel() { Posts = posts };
            return View("../Home/Index", postVM);
        }

        public async Task<ViewResult> Register([Bind("FirstName,LastName,Email,Password")] PersonalBlogUser user)
        {
			await _auth.CreateUser(user);

			List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();
            BlogPostsViewModel postVM = new BlogPostsViewModel() { Posts = posts };
            return View("../Home/Index", postVM);
        }
    }
}
