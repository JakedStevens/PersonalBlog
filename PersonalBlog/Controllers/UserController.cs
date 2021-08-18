using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PersonalBlog.ClassLibrary;
using PersonalBlog.Web.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using PersonalBlog.Web.Models;

namespace PersonalBlog.Web.Controllers
{
    [AllowAnonymous]
	public class UserController : Controller
	{
		private readonly IMemoryCache _cache;
		private readonly UserAuth _auth;
		private readonly DataManager _data;

		public UserController(IMemoryCache cache, UserAuth auth, DataManager data)
		{
			_cache = cache;
			_auth = auth;
			_data = data;
		}

		public ViewResult LoginRegister()
		{
			var lrVM = new LoginRegisterViewModel();
			return View("LoginRegister", lrVM);
		}

		[HttpPost]
		public async Task<IActionResult> Register([Bind("FirstName,LastName,Email,Password,ConfirmPassword")] UserRegister userRegister)
		{
			bool doesAccExist = _auth.DoesAccountExist(userRegister);

			if (!doesAccExist)
			{
				int createResponse = await _auth.CreateUser(userRegister);

				if (createResponse == 1)
				{
					var newUser = new UserLogin() { LoginEmail = userRegister.Email, LoginPassword = userRegister.Password, ReturnUrl = "" };
					return await Login(newUser);
				}
				else
				{
					LoginRegisterViewModel lrVM = _data.CreateAlertLRVM(AlertTypeEnum.danger, "There was a problem creating an account with this information. Please re-enter and check for correctness.");
					return View("LoginRegister", lrVM);
				}
			}
			else
			{
				LoginRegisterViewModel lrVM = _data.CreateAlertLRVM(AlertTypeEnum.danger, "There is already an account for the email entered.");
				return View("LoginRegister", lrVM);
			}
		}

		public ViewResult Login(string returnUrl)
		{
			if (Request.QueryString.HasValue)
			{
				LoginRegisterViewModel lrVM = _data.CreateLoginRedirectLRVM(returnUrl, AlertTypeEnum.info, "Please sign in to continue.");
				return View("LoginRegister", lrVM);
			}
			else
			{
				var lrVM = new LoginRegisterViewModel();
				return View("LoginRegister", lrVM);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Login([Bind("LoginEmail,LoginPassword,ReturnUrl")] UserLogin userLogin)
        {
			if (_auth.AreCredentialsValid(userLogin))
			{
				List<Claim> claims = _auth.CreateAuthClaims(userLogin);

				ClaimsIdentity claimsId = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				AuthenticationProperties authProps = new AuthenticationProperties();
				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsId), authProps);

				return !string.IsNullOrEmpty(userLogin.ReturnUrl) ? Redirect($"{userLogin.ReturnUrl}") : RedirectToAction("Home", "Home");
			}
			else
            {
				if (!string.IsNullOrEmpty(userLogin.ReturnUrl))
				{
					LoginRegisterViewModel lrVM = _data.CreateLoginRedirectLRVM(userLogin.ReturnUrl, AlertTypeEnum.danger, "The email or password you entered was incorrect.");

					return View("LoginRegister", lrVM);
				}
				else
                {
					LoginRegisterViewModel lrVM = _data.CreateAlertLRVM(AlertTypeEnum.danger, "The email or password you entered was incorrect.");

					return View("LoginRegister", lrVM);
				}
			}
        }

		public async Task<RedirectToActionResult> Logout()
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
				LoginRegisterViewModel lrVM = _data.CreateAlertLRVM(AlertTypeEnum.danger, "You are not logged in. Log in to view your profile.");

				return View("LoginRegister", lrVM);
			}
			
		}

		// Not using this right now, keeping incase I need to implement revoking the auth cookie
		private ViewResult Revoke()
        {
			ClaimsPrincipal principal = HttpContext.User;
			var userId = principal?.Claims.First(c => c.Type == "user_id");

			_cache.Set("revoke-" + userId.Value, true);
			return View();
		}
	}
}
