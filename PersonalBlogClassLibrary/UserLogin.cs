using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace PersonalBlog.ClassLibrary
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Email is Required")]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string LoginPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
