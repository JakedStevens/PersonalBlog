using System.Collections.Generic;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PersonalBlog.ClassLibrary
{
    public class UserAuth
    {
        private readonly PersonalBlogDbContext _dbContext;

        public UserAuth(PersonalBlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateUser(UserRegister user)
        {
            string hashedPassword = Crypto.HashPassword(user.Password);
            user.Password = hashedPassword;

            _dbContext.Add(user);
            return await _dbContext.SaveChangesAsync();
        }

        public List<Claim> CreateAuthClaims(UserLogin loginUser)
        {
            PersonalBlogUser user = GetUserInfo(loginUser.LoginEmail);
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim> {
                    new Claim("user_id", userId),
                    new Claim("first_name", user.FirstName),
                    new Claim("last_name", user.LastName),
                    new Claim("email", user.Email),
                    new Claim("created_on", user.CreatedOn.ToString()),
                    new Claim("access_token", GetAccessToken(userId)),
            };
            return claims;
        }

        private static string GetAccessToken(string userId)
        {
            const string issuer = "localhost";
            const string audience = "localhost";

            var identity = new ClaimsIdentity(new List<Claim> { new Claim("sub", userId) });

            var bytes = Encoding.UTF8.GetBytes(userId);
            // Would switch this to AsymmetricSecurityKey if this was a real site
            var key = new SymmetricSecurityKey(bytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateJwtSecurityToken(issuer, audience, identity, now, now.Add(TimeSpan.FromHours(1)), now, signingCredentials);

            return handler.WriteToken(token);
        }

        public bool AreCredentialsValid(UserLogin user)
        {
            PersonalBlogUser retrievedUser = _dbContext.PersonalBlogUser.SingleOrDefault(record => record.Email == user.LoginEmail);

            return retrievedUser == null ? false : Crypto.VerifyHashedPassword(retrievedUser.Password, user.LoginPassword);
        }

        public PersonalBlogUser GetUserInfo(string email)
        {
            return _dbContext.PersonalBlogUser.SingleOrDefault(record => record.Email == email);
        }

        public bool DoesAccountExist(UserRegister user)
        {
            return _dbContext.PersonalBlogUser.Any(record => record.Email == user.Email);
        }
    }
}
