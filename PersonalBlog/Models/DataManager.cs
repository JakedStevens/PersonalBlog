using Microsoft.EntityFrameworkCore;
using PersonalBlog.ClassLibrary;
using PersonalBlog.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.Web.Models
{
	public class DataManager
	{
		private readonly PersonalBlogDbContext _dbContext;

		public DataManager(PersonalBlogDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<BlogPostsViewModel> CreateAllPostsVM()
		{
			return new BlogPostsViewModel() { Posts = await _dbContext.BlogPost.ToListAsync() };
		}

		public async Task<BlogPostsViewModel> CreateFilteredPostsVM(PostTypeEnum postType)
		{
			List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();

			return new BlogPostsViewModel() { Posts = posts.Where(post => post.PostCategory == postType.ToString()).ToList() };
		}

		public async Task<BlogPostsViewModel> CreateSearchedPostsVM(string searchInput)
		{
			List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();

			return new BlogPostsViewModel() { Posts = posts.Where(post => post.PostTitle.ToLower().Contains(searchInput.ToLower())).ToList() };
		}

		public LoginRegisterViewModel CreateAlertLRVM(AlertTypeEnum alertType, string message)
		{
			Alert alert = new Alert() { ShowAlert = true, AlertType = alertType, Message = message };

			return new LoginRegisterViewModel() { Alert = alert };
		}

		public LoginRegisterViewModel CreateLoginRedirectLRVM(string returnUrl, AlertTypeEnum alertType, string message)
		{
			Alert alert = new Alert() { ShowAlert = true, AlertType = alertType, Message = message };
			UserLogin userLogin = new UserLogin() { ReturnUrl = returnUrl };
			
			return new LoginRegisterViewModel() { Alert = alert, UserLogin = userLogin };
		}
	}
}
