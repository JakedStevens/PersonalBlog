using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace PersonalBlog.ClassLibrary
{
	public class BlogPost
	{ 
		[Key]
		public int PostId { get; set; }

		public string PostTitle { get; set; }

		public string PostContent { get; set; }

		public string PostCategory { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public string PostDate { get; set; }
	}
}
