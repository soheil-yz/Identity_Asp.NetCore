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
            var role = _roleManager.Roles.Select(p =>
            new RolesListDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            }).ToList();
            return View(role);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(AddNewRoleDto addNewRole)
        {
            if (!ModelState.IsValid)
            {
                return View(addNewRole);
            }
            Roles roles = new Roles
            {
                Description = addNewRole.Description,
                Name = addNewRole.Name,
            };
            var result = _roleManager.CreateAsync(roles).Result;
            if (result.Succeeded)
                return RedirectToAction("Index", "Roles", new { area = "admin" });

            ViewBag.Errors = result.Errors.ToList();
            return View(addNewRole);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var roles = _roleManager.FindByIdAsync(id).Result;
            EditRolesDto editRolesDto = new EditRolesDto
            {
                Name = roles.Name,
                Description = roles.Description,
            };
            return View(editRolesDto);
        }        
        [HttpPost]
        public IActionResult Edit(EditRolesDto editRolesDto)
        {
            var role = _roleManager.FindByIdAsync(editRolesDto.Id).Result;
            role.Name = editRolesDto.Name;
            role.Description = editRolesDto.Description;

            var result = _roleManager.UpdateAsync(role).Result;

            if (result.Succeeded)
                return RedirectToAction("Index", "Roles", new { area = "admin" });

            return View(editRolesDto);
        }
    }
}
