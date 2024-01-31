using Identity.Areas.Admin.Models.Dto;
using Identity.Areas.Admin.Models.Dto.Roles;
using Identity.Models.Entities;
using Identity.Models.Entities.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Identity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<Roles> _roleManager;
        public UsersController(UserManager<Users> userManager, RoleManager<Roles> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.Select(p => new UserListDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                UserName = p.UserName,
                PhoneNumber = p.PhoneNumber,
                EmailConfirmed = p.EmailConfirmed,
                AccessFailedCount = p.AccessFailedCount

            }).ToList();
            return View(users);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(RegisterDto registerDto)
        {
            if (ModelState.IsValid == false)
            {
                return View(registerDto);
            }
            Users newUser = new Users
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email

            };

            var Users = _userManager.CreateAsync(newUser, registerDto.Password).Result;
            if (Users.Succeeded)
                return RedirectToAction("Index", "Users", new { area = "admin" });

            string message = "";
            foreach (var item in Users.Errors.ToList())
                message += item.Description + Environment.NewLine;

            TempData["Massege"] = message;
            return View(registerDto);
        }

        public IActionResult Edit(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;
            UserEditDto userEditDto = new UserEditDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhotoNumber = user.PhoneNumber,
                UserName = user.UserName
            };

            return View(userEditDto);
        }
        [HttpPost]
        public IActionResult Edit(UserEditDto userEdit)
        {
            var user = _userManager.FindByIdAsync(userEdit.Id).Result;
            user.FirstName = userEdit.FirstName;
            user.LastName = userEdit.LastName;
            user.PhoneNumber = userEdit.PhotoNumber;
            user.Email = userEdit.Email;
            user.UserName = userEdit.UserName;
            var result = _userManager.UpdateAsync(user).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Users", new { area = "admin" });
            }

            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;
            return View(userEdit);
        }

        public IActionResult Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            UserDeleteDto userDeleteDto = new UserDeleteDto
            {
                Email = user.Email,
                FullName = $"{user.FirstName} + {user.LastName}",
                Id = user.Id,
                UserName = user.UserName,

            };
            return View(userDeleteDto);
        }
        [HttpPost]
        public IActionResult Delete(UserDeleteDto userDelete)
        {
            var user = _userManager.FindByIdAsync(userDelete.Id).Result;
            var result = _userManager.DeleteAsync(user).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Users", new { area = "admin" });
            }
            string message = "";
            foreach (var item in result.Errors.ToList())
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;
            return View(userDelete);
        }
        [HttpGet]
        public IActionResult Details(string id)
        {
            var userId = _userManager.FindByIdAsync(id).Result;
            //var User = _userManager.Users.Select(p => new UserDetailsDto
            //{
            //    FirstName = userId.FirstName,
            //    LastName = userId.LastName,
            //    Email = userId.Email,
            //    FullName = $"{userId.FirstName} + {userId.LastName}",
            //    Id = userId.Id,
            //    PhoneNumber = userId.PhoneNumber,
            //    EmailConfirmed = userId.EmailConfirmed,
            //    UserName = userId.UserName,
            //    AccessFailedCount = userId.AccessFailedCount,

            //}).ToList();
            ////////////////////////////////////
            //var User = _userManager.Users.FirstOrDefault(userId).ToString();
            UserDetailsDto userDetails = new UserDetailsDto
            {
                FirstName = userId.FirstName,
                LastName = userId.LastName,
                Email = userId.Email,
                FullName = $"{userId.FirstName} {userId.LastName}",
                Id = userId.Id,
                PhoneNumber = userId.PhoneNumber,
                EmailConfirmed = userId.EmailConfirmed,
                UserName = userId.UserName,
                AccessFailedCount = userId.AccessFailedCount,

            };
            return View(userDetails);

        }

        public IActionResult AddUserRole(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;
            var roles = new List<SelectListItem>(
                _roleManager.Roles.Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Name
                }).ToList());

            return View(new AddUserRoleDto
            { 
                Id = Id,
                Roles = roles,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
            });
        }
        [HttpPost]
        public IActionResult AddUserRole(AddUserRoleDto addRole)
        {
            var user = _userManager.FindByIdAsync(addRole.Id).Result;
            var result = _userManager.AddToRoleAsync(user, addRole.Role).Result;
            return RedirectToAction("UserRoles", "Users", new { Id = user.Id , area = "Admin" });

        }

        public IActionResult UserRoles(string Id)
        {
            var user = _userManager.FindByIdAsync(Id).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            ViewBag.UserInfo = $"{user.FirstName} {user.LastName} , Email : {user.Email}";
            return View(roles);
        }

    }

}
