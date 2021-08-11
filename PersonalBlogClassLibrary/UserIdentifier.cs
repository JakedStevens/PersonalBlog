using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.ClassLibrary
{
    public class UserIdentifier
    {
		private readonly PersonalBlogDbContext _dbContext;

		public UserIdentifier(PersonalBlogDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public PersonalBlogUser GetBlogUser(ClaimsPrincipal user)
		{
			var userOid = Guid.Parse(user.Claims.ToList().FirstOrDefault(claim =>
				claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier"
			).Value);

			return _dbContext.PersonalBlogUser.FirstOrDefault(x => x.AzureADId == userOid);
		}
	}
}
