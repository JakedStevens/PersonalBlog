﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.ClassLibrary
{
    public class PersonalBlogUserLogin
    {
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        [Column("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Column("Password")]
        public string Password { get; set; }
    }
}