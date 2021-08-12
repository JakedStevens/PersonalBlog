﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalBlog.ClassLibrary;
using PersonalBlog.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly PersonalBlogDbContext _dbContext;
        private readonly ILogger<HomeController> _logger;

        public UserController(ILogger<HomeController> logger, PersonalBlogDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View("Authentication");
        }

        //public async Task<ViewResult> SignIn(PersonalBlogUser user)
        //{
        //    List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();
        //    BlogPostsViewModel postVM = new BlogPostsViewModel() { Posts = posts };
        //    return View("../Home/Index", postVM);
        //}

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
