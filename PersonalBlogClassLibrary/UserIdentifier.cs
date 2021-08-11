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
		private readonly PersonalBlogDbContext _context;

		public UserIdentifier(PersonalBlogDbContext context)
		{
			_context = context;
		}

		public PersonalBlogUser GetDealerLeadUser(ClaimsPrincipal user)
		{
			var userOid = Guid.Parse(user.Claims.ToList().FirstOrDefault(claim =>
				claim.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier"
			).Value);

			return _context.PersonalBlogUser.FirstOrDefault(x => x.AzureADId == userOid);
		}
	}
}
