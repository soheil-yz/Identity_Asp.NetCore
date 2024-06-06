using Identity.Data;
using Identity.Models.Entities;
using Identity.Models.Entities.Dto.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BlogController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly UserManager<Users> _userManager;
        private readonly IAuthorizationService _service;

        public BlogController(
            DataBaseContext context, 
            UserManager<Users> userManager, 
            IAuthorizationService service)
        {
            _context = context;
            _userManager = userManager;
            _service = service;
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
            return View(blog);
        }
        [HttpGet]
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
                users = user ,
                UserId = user.Id
            };
            _context.Add(newblog);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        
        public IActionResult Edit(long Id)
        {
            var blog = _context.Blogs.
                Include(p => p.users).
                Where(p => p.Id == Id).
                Select(p => new CreateBlog
                {
                    Body = p.Body,
                    Title = p.Title,
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = p.users.UserName
                }).FirstOrDefault();
            var result = _service.AuthorizeAsync(User, blog, "IsBlogForYou").Result;
            if (result.Succeeded)
            {
                return View(blog);
            }
            else
            {
                return new ChallengeResult();
            }
           
        }
        [HttpPost]
        public IActionResult Edit(Blog blog)
        {

            return View(blog);
        }
    }
}
