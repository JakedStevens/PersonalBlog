using PersonalBlog.ClassLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Security.Claims;
using System.Linq;
using Microsoft.Extensions.Logging;
using PersonalBlog.Web.Controllers;
using PersonalBlog.Web.Models;

namespace PersonalBlog.Web
{
	public class Startup
	{
		private readonly IWebHostEnvironment _env;
		static readonly PersonalBlogDbContext _dbContext = new();

		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			_env = env;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
				{
					options.Cookie.HttpOnly = true;
					options.Cookie.SecurePolicy = _env.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
					options.Cookie.SameSite = SameSiteMode.Lax;
					options.Cookie.Name = "Blog.AuthCookie";
					options.LoginPath = "/User/Login";
					options.LogoutPath = "/User/Logout";
				});
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.MinimumSameSitePolicy = SameSiteMode.Strict;
				options.HttpOnly = HttpOnlyPolicy.None;
				options.Secure = _env.IsDevelopment()
				  ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
			});
			services.AddControllersWithViews(options => options.Filters.Add(new AuthorizeFilter()));
			services.AddTransient<PersonalBlogDbContext>();
			services.AddScoped<UserAuth>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();
			app.UseCookiePolicy();
			app.UseAuthentication();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "Home",
					pattern: "{controller=Home}/{action=Home}");
				endpoints.MapControllerRoute(
					name: "Blog Posts",
					pattern: "{controller=BlogPosts}/{action=AllPosts}",
					defaults: new { controller = "BlogPosts", action = "AllPosts" });
				endpoints.MapControllerRoute(
					name: "Login",
					pattern: "{controller=User}/{action=LoginRegister}/{ReturnUrl?}",
					defaults: new { controller = "User", action = "LoginRegister" });
			});
		}
	}
}
