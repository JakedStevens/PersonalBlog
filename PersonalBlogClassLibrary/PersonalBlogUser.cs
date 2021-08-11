﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.ClassLibrary
{
    public class PersonalBlogUser
    {
        //[Key]
        //[Column("UserId")]
        //public int Id { get; set; }

        //[Column("AzureADId")]
        //public Guid AzureADId { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime? CreateDate { get; set; }

        [Key]
        [Column("UserId")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(40)]
        [Column("UserName")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        [Column("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Column("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
