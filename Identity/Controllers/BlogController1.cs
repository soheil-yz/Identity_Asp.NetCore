using Identity.Data;
using Identity.Models.Entities.Dto.Blog;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    public class BlogController1 : Controller
    {
        private readonly DataBaseContext _context;

        public BlogController1(DataBaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Create()
        {
            return View();
        }        
        public IActionResult Create(CreateBlog blog)
        {
            return View();
        }
    }
}
