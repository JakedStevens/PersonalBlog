using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.ClassLibrary
{
	public static class HelperMethods
	{
		public static string Truncate(string value, int maxChars)
		{
			return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
		}
	}
}
