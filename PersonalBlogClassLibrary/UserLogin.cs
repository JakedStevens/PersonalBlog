using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace PersonalBlog.ClassLibrary
{
    public class UserLogin
    {
        [Column("Email")]
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Column("Password")]
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}
