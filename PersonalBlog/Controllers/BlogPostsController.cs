using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.ClassLibrary;
using PersonalBlog.Web.Models;
using PersonalBlog.Web.ViewModels;

namespace PersonalBlog.Web.Controllers
{
	public class BlogPostsController : Controller
    {
        private readonly DataManager _data;
        private readonly PersonalBlogDbContext _dbContext;

        public BlogPostsController(DataManager data, PersonalBlogDbContext dbContext)
        {
            _data = data;
            _dbContext = dbContext;
        }

        // GET: BlogPosts
        [AllowAnonymous]
        public async Task<ViewResult> AllPosts()
        {
            BlogPostsViewModel postVM = await _data.CreateAllPostsVM();

            return View("AllPosts", postVM);
        }

        [AllowAnonymous]
        public async Task<ViewResult> Drums()
        {
            BlogPostsViewModel postVM = await _data.CreateFilteredPostsVM(PostTypeEnum.Drums);

            return View("DrumPosts", postVM);
        }

        [AllowAnonymous]
        public async Task<ViewResult> Guitar()
        {
            BlogPostsViewModel postVM = await _data.CreateFilteredPostsVM(PostTypeEnum.Guitar);

            return View("GuitarPosts", postVM);
        }

        [AllowAnonymous]
        public async Task<ViewResult> Engineering()
        {
            BlogPostsViewModel postVM = await _data.CreateFilteredPostsVM(PostTypeEnum.Engineering);

            return View("EngineeringPosts", postVM);
        }

        [AllowAnonymous]
        public async Task<ViewResult> Search(string searchInput)
        {
            if(!string.IsNullOrWhiteSpace(searchInput))
            {
                BlogPostsViewModel postVM = await _data.CreateSearchedPostsVM(searchInput);

                return View("PostSearch", postVM);
            }
            else
            {
                BlogPostsViewModel postVM = new BlogPostsViewModel();
                return View("PostSearch", postVM);
            }
        }

        // GET: BlogPosts/Details/{id}
        [AllowAnonymous]
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
