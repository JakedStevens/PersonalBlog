﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PersonalBlog.Web.HtmlHelpers
{
	public static class HtmlHelpers
	{
		public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimType)
		{
			var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);

			return (claim != null) ? claim.Value : string.Empty;
		}
	}
}
