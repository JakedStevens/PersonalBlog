using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PersonalBlog.ClassLibrary
{
	public class BlogPost
	{ 
		[Key]
		public int PostId { get; set; }

		public string PostTitle { get; set; }

		public string PostContent { get; set; }

		public string PostCategory { get; set; }

		public string PostImageURL { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime PostDate { get; set; }
	}
}
