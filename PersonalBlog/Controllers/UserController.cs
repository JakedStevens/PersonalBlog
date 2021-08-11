using Microsoft.AspNetCore.Mvc;
using PersonalBlog.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View("Authentication");
        }

        public IActionResult SignIn(PersonalBlogUser user)
        {
            return View("Authentication");
        }

        public IActionResult Register(PersonalBlogUser user)
        {
            return View("Authentication");
        }
    }
}
