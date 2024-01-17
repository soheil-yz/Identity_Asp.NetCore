using Identity.Areas.Admin.Models.Dto.Roles;
using Identity.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<Roles> _roleManager;

        public RolesController(RoleManager<Roles> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var role = _roleManager.Roles.Select(p=> 
            new RolesListDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            }).ToList();
            return View(role);
        }
    }
}
