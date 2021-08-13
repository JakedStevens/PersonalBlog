﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

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

        public bool AreCredentialsValid(UserLogin user)
        {
            PersonalBlogUser retrievedUser = _dbContext.PersonalBlogUser.SingleOrDefault(record => record.Email == user.Email);

            string savedHashedPassword = retrievedUser.Password;

            return Crypto.VerifyHashedPassword(savedHashedPassword, user.Password);
        }

        public PersonalBlogUser GetUserInfo(UserLogin user)
        {
            return _dbContext.PersonalBlogUser.SingleOrDefault(record => record.Email == user.Email);
        }
    }
}
