using PersonalBlog.ClassLibrary;

namespace PersonalBlog.Web.ViewModels
{
    public class SignInRegisterViewModel
    {
        public PersonalBlogUser User2 { get; set; }
        public PersonalBlogLoginUser User1 { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; }

        public SignInRegisterViewModel()
        {
            this.User2 = new PersonalBlogUser();
            this.User1 = new PersonalBlogLoginUser();
        }
    }
}
