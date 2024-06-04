using Identity.Data;
using Identity.Models.Entities;
using Identity.Models.Entities.Dto.Blog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Controllers
{
    public class BlogController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly UserManager<Users> _userManager;

        public BlogController(DataBaseContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var blog = _context.Blogs.Include(p => p.users).Select(p => new CreateBlog
            {
                Id = p.Id,
                Body = p.Body,
                Title = p.Title,
                UserName = p.users.UserName
            });
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateBlog blog)
        {
            var user = _userManager.GetUserAsync(User).Result;
            Blog newblog = new Blog()
            {
                Body = blog.Body,
                Title = blog.Title,
                users = user


            };
            _context.Add(newblog);    
            return RedirectToAction("Index");
        }

        public IActionResult Edit(long Id) 
        {
            var blog = _context.Blogs.Find(Id);
            return View(new CreateBlog
            {
                Body = blog.Body,
                Title = blog.Title,
                Id = blog.Id
            });      
        }
        [HttpPost]
        public IActionResult Edit(Blog blog)
        {

            return View(blog);
        }
    }
}
