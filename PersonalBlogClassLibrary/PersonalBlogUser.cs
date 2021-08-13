using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.ClassLibrary
{
    public class PersonalBlogUser
    {
        [Key]
        [Column("UserId")]
        public int Id { get; set; }

        [Column("FirstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

		[Column("LastName")]
		[Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
    }
}
