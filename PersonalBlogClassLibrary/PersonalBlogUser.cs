using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.ClassLibrary
{
    public class PersonalBlogUser
    {
		[Key]
		[Column("UserId")]
		public int Id { get; set; }

		[Column("AzureADId")]
		public Guid AzureADId { get; set; }

		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? CreateDate { get; set; }
	}
}
