using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PersonalBlog.ClassLibrary
{
	public class BlogPost
	{ 
		[Key]
		public int PostId { get; set; }

		[Display(Name = "Title")]
		public string PostTitle { get; set; }

		[Display(Name = "Content")]
		public string PostContent { get; set; }

		[Display(Name = "Category")]
		public string PostCategory { get; set; }

		[Display(Name = "Image URL")]
		public string PostImageURL { get; set; }

		[Display(Name = "Date Posted")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime PostDate { get; set; }
	}
}
