using PersonalBlog.ClassLibrary;

namespace PersonalBlog.Web.ViewModels
{
	public class LoginRegisterViewModel
	{
		public Alert Alert { get; set; }
		public UserLogin UserLogin { get; set; }
		public UserRegister UserRegister { get; set; }

		public LoginRegisterViewModel()
		{
			this.Alert = new Alert();
			this.UserLogin = new UserLogin();
			this.UserRegister = new UserRegister();
		}
	}
}
