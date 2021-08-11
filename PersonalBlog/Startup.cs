using PersonalBlog.ClassLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web.UI;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PersonalBlog.Web
{
	public class Startup
	{
		static readonly PersonalBlogDbContext _dbContext = new();

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
				.AddMicrosoftIdentityWebApp(options =>
				{
					Configuration.Bind("AzureAD", options);
					options.Events ??= new OpenIdConnectEvents();
					options.Events.OnTokenValidated += OnTokenValidatedFunc;
				});
			services.AddControllersWithViews(options =>
			{
				var policy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
				options.Filters.Add(new AuthorizeFilter(policy));
			});
			services.AddRazorPages()
				 .AddMicrosoftIdentityUI();
			services.AddTransient<PersonalBlogDbContext>();
			services.AddScoped<UserIdentifier>();
		}

		private async Task OnTokenValidatedFunc(TokenValidatedContext context)
		{
			GetUserOid(context);
			await Task.CompletedTask.ConfigureAwait(false);
		}

		public async void Register(Guid userGuid)
		{
			var newUser = new PersonalBlogUser() { AzureADId = userGuid };
			_dbContext.Add(newUser);
			await _dbContext.SaveChangesAsync();
		}

		public async void GetUserOid(TokenValidatedContext context)
		{
			var claimsList = context.Principal.Claims.ToList();
			var oidClaim = claimsList.FirstOrDefault(claim =>
				claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier"
			);
			Guid userGuid = Guid.Parse(oidClaim.Value);
			List<PersonalBlogUser> userList = await _dbContext.PersonalBlogUser.ToListAsync();
			bool userExists = userList.Any(user => user.AzureADId == userGuid);
			if (!userExists)
			{
				Register(userGuid);
			}
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
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "Home",
					pattern: "{controller=Home}/{action=Index}");
				endpoints.MapControllerRoute(
					name: "Drums",
					pattern: "{controller=Drums}/{action=Index}",
					defaults: new { controller = "Drums", action = "Index" });
			});
		}
	}
}
