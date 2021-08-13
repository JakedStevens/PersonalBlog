using PersonalBlog.ClassLibrary;

namespace PersonalBlog.Web.ViewModels
{
    public class SignInRegisterViewModel
    {
        public PersonalBlogUser User { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }
    }
}
