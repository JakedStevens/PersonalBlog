using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PersonalBlog.ClassLibrary
{
    public class UserRegister
    {
        [StringLength(20)]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; }

        [StringLength(20)]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [StringLength(26)]
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [StringLength(26)]
        [Required(ErrorMessage = "Confirm is Required")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
