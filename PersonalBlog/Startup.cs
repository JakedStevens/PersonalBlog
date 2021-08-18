using PersonalBlog.ClassLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using PersonalBlog.Web.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using System;

namespace PersonalBlog.Web
{
	public class Startup
	{
		private readonly IWebHostEnvironment _env;
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			_env = env;
		}

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
			services.AddTransient<UserAuth>();
			services.AddScoped<DataManager>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//if (env.IsDevelopment())
			//{
			//	app.UseDeveloperExceptionPage();
			//}
			//else
			//{
			//	app.UseExceptionHandler("/Home/Error");
			//	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			//	app.UseHsts();
			//}

			app.UseDeveloperExceptionPage();


			app.UseHttpsRedirection();
			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthorization();
			app.UseCookiePolicy();
			app.UseAuthentication();

			//SecretClientOptions options = new SecretClientOptions()
			//{
			//	Retry =
			//	{
			//		Delay= TimeSpan.FromSeconds(2),
			//		MaxDelay = TimeSpan.FromSeconds(16),
			//		MaxRetries = 5,
			//		Mode = RetryMode.Exponential
			//	 }
			//};
			//var client = new SecretClient(new Uri("https://amusicblogkeyvault.vault.azure.net/"), new DefaultAzureCredential(), options);
			//KeyVaultSecret secret = client.GetSecret("");
			//string secretValue = secret.Value;

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
