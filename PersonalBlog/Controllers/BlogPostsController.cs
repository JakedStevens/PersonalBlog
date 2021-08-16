using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.ClassLibrary;
using PersonalBlog.Web.ViewModels;

namespace PersonalBlog.Web.Controllers
{
	public class BlogPostsController : Controller
    {
        private readonly PersonalBlogDbContext _dbContext;

        public BlogPostsController(PersonalBlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: BlogPosts
        [AllowAnonymous]
        public async Task<ViewResult> AllPosts()
        {
            List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();
            BlogPostsViewModel postVM = new BlogPostsViewModel() { Posts = posts };
            return View("AllPosts", postVM);
        }

        [AllowAnonymous]
        public async Task<ViewResult> Drums()
        {
            List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();
            List<BlogPost> drumPosts = posts.Where(post => post.PostCategory == "Drums").ToList();
            BlogPostsViewModel postVM = new BlogPostsViewModel() { Posts = drumPosts };
            return View("DrumPosts", postVM);
        }

        [AllowAnonymous]
        public async Task<ViewResult> Guitar()
        {
            List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();
            List<BlogPost> guitarPosts = posts.Where(post => post.PostCategory == "Guitar").ToList();
            BlogPostsViewModel postVM = new BlogPostsViewModel() { Posts = guitarPosts };
            return View("GuitarPosts", postVM);
        }

        [AllowAnonymous]
        public async Task<ViewResult> Engineering()
        {
            List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();
            List<BlogPost> engineeringPosts = posts.Where(post => post.PostCategory == "Engineering").ToList();
            BlogPostsViewModel postVM = new BlogPostsViewModel() { Posts = engineeringPosts };
            return View("EngineeringPosts", postVM);
        }

        [AllowAnonymous]
        public async Task<ViewResult> Search(string searchInput)
        {
            if(!string.IsNullOrWhiteSpace(searchInput))
            {
                List<BlogPost> posts = await _dbContext.BlogPost.ToListAsync();
                List<BlogPost> filteredPosts = posts.Where(post => post.PostTitle.ToLower().Contains(searchInput.ToLower())).ToList();
                BlogPostsViewModel postVM = new BlogPostsViewModel() { Posts = filteredPosts };
                return View("PostSearch", postVM);
            }
            else
            {
                BlogPostsViewModel postVM = new BlogPostsViewModel();
                return View("PostSearch", postVM);
            }
            
        }

        [AllowAnonymous]
        // GET: BlogPosts/Details/{id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }

            var blogPost = await _dbContext.BlogPost.FirstOrDefaultAsync(m => m.PostId == id);

            if (blogPost == null) { return NotFound(); }

            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,PostTitle,PostContent,PostCategory,PostDate,PostImageURL")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(blogPost);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(AllPosts));
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _dbContext.BlogPost.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("PostId,PostTitle,PostContent,PostCategory,PostDate,PostImageURL")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(blogPost);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AllPosts));
            }
            return View("Index");
        }

        // GET: BlogPosts/Delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _dbContext.BlogPost
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _dbContext.BlogPost.FindAsync(id);
            _dbContext.BlogPost.Remove(blogPost);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(AllPosts));
        }

        private bool BlogPostExists(int id)
        {
            return _dbContext.BlogPost.Any(e => e.PostId == id);
        }
    }
}
