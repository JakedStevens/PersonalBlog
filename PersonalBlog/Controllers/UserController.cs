using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.ClassLibrary;
using PersonalBlog.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace PersonalBlog.Web.Controllers
{
    [AllowAnonymous]
	public class UserController : Controller
	{
		private readonly PersonalBlogDbContext _dbContext;
		private readonly UserAuth _auth;
		private readonly IMemoryCache _cache;

		public UserController(IMemoryCache cache, UserAuth auth, PersonalBlogDbContext dbContext)
		{
			_cache = cache;
			_dbContext = dbContext;
			_auth = auth;
		}

		public ViewResult LoginRegister()
		{
			var lrVM = new LoginRegisterViewModel();
			return View("LoginRegister", lrVM);
		}

		[HttpPost]
		public async Task<ViewResult> Register([Bind("FirstName,LastName,Email,Password,ConfirmPassword")] UserRegister user)
		{
			var doesAccExist = _auth.DoesAccountExist(user);

			if (!doesAccExist)
			{
				await _auth.CreateUser(user);

				List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();
				BlogPostsViewModel postVM = new BlogPostsViewModel() { Posts = posts };
				return View("../Home/Home", postVM);
			}
			else
			{
				Alert failAlert = new Alert() { ShowAlert = true, AlertType = AlertEnum.danger, Message = "There is already an account for the email entered." };
				var lrVM = new LoginRegisterViewModel() { Alert = failAlert };
				return View("LoginRegister", lrVM);
			}
		}

		public IActionResult Login(string returnUrl)
		{
			if (Request.QueryString.HasValue)
			{
				Alert infoAlert = new Alert() { ShowAlert = true, AlertType = AlertEnum.info, Message = "Please sign in to continue." };
				UserLogin userLogin = new UserLogin() { ReturnUrl = returnUrl };
				var lrVM = new LoginRegisterViewModel() { Alert = infoAlert, UserLogin = userLogin };
				return View("LoginRegister", lrVM);
			}
			else
			{
				var lrVM = new LoginRegisterViewModel();
				return View("LoginRegister", lrVM);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Login([Bind("LoginEmail,LoginPassword,ReturnUrl")] UserLogin loginUser)
        {
			bool areCredentialsValid = _auth.AreCredentialsValid(loginUser);

			if (areCredentialsValid)
			{
				PersonalBlogUser user = _auth.GetUserInfo(loginUser.LoginEmail);
				var userId = Guid.NewGuid().ToString();
				var claims = new List<Claim> {
					new Claim("user_id", userId),
					new Claim("first_name", user.FirstName),
					new Claim("last_name", user.LastName),
					new Claim("email", user.Email),
					new Claim("created_on", user.CreatedOn.ToString()),
					new Claim("access_token", GetAccessToken(userId)),
				};
				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var authProperties = new AuthenticationProperties();
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

				return Redirect($"{loginUser.ReturnUrl}");
			}
			else
            {
				if (!string.IsNullOrEmpty(loginUser.ReturnUrl))
				{
					Alert failAlert = new Alert() { ShowAlert = true, AlertType = AlertEnum.danger, Message = "The email or password you entered was incorrect." };
					UserLogin userLogin = new UserLogin() { ReturnUrl = loginUser.ReturnUrl };
					var lrVM = new LoginRegisterViewModel() { Alert = failAlert, UserLogin = userLogin };
					return View("LoginRegister", lrVM);
				}
				else
                {
					Alert failAlert = new Alert() { ShowAlert = true, AlertType = AlertEnum.danger, Message = "The email or password you entered was incorrect." };
					var lrVM = new LoginRegisterViewModel() { Alert = failAlert };
					return View("LoginRegister", lrVM);
				}
			}
			
        }

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("Home", "Home");
		}

		public ViewResult Profile()
		{
			if (this.User.Claims.ToList().Count > 0)
			{
				var email = this.User.Claims.ToList().FirstOrDefault(claim => claim.Type == "email").Value;
				PersonalBlogUser user = _auth.GetUserInfo(email);

				return View("Profile", user);
			}
			else
			{
				Alert failAlert = new Alert() { ShowAlert = true, AlertType = AlertEnum.danger, Message = "No user logged in. Log in to view your profile." };
				var lrVM = new LoginRegisterViewModel() { Alert = failAlert };
				return View("LoginRegister", lrVM);
			}
			
		}

		private ViewResult Revoke()
        {
			var principal = HttpContext.User as ClaimsPrincipal;
			var userId = principal?.Claims
			  .First(c => c.Type == "user_id");

			_cache.Set("revoke-" + userId.Value, true);
			return View();
		}

		private static string GetAccessToken(string userId)
		{
			const string issuer = "localhost";
			const string audience = "localhost";

			var identity = new ClaimsIdentity(new List<Claim> { new Claim("sub", userId) });

			var bytes = Encoding.UTF8.GetBytes(userId);
			var key = new SymmetricSecurityKey(bytes);
			var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var now = DateTime.UtcNow;
			var handler = new JwtSecurityTokenHandler();

			var token = handler.CreateJwtSecurityToken(issuer, audience, identity, now, now.Add(TimeSpan.FromHours(1)), now, signingCredentials);

			return handler.WriteToken(token);
		}
	}
}
