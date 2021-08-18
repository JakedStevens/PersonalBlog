using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace PersonalBlog.ClassLibrary
{
	public class PersonalBlogDbContext : DbContext
	{
        private string _connectionString;

        public PersonalBlogDbContext()
        {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };
            var client = new SecretClient(new Uri("https://amusicblogkeyvault.vault.azure.net/"), new DefaultAzureCredential(), options);
            KeyVaultSecret secret = client.GetSecret("ConnectionStrings--Azure");
            _connectionString = secret.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(_connectionString);

        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<PersonalBlogUser> PersonalBlogUser { get; set; }

    }
}
