using Forums.Core;
using Forums.Models;
using Forums.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Forums.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> _singIn;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _role;
        private readonly ProjectContext _context;

        public AccountController(ProjectContext context, SignInManager<Users> signIn, UserManager<Users> userManager, RoleManager<IdentityRole> role)
        {
            _singIn = signIn;
            _userManager = userManager;
            _role = role;
            _context = context;
        }
        public IActionResult Register()
        {
            List<SelectListItem> roleList = new();
            List<IdentityRole> roles = _role.Roles.ToList();
            foreach (var role in roles)
            {
                roleList.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Name
                });
            }
            ViewBag.roleList = roleList;

            List<Level> level = _context.levels.ToList();
            List<SelectListItem> selectList = new();
            level.ForEach(l =>
            {
                selectList.Add(new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.TypeName
                });
            });
            ViewBag.LevelsListItem = selectList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(CreateUser user)
        {
            var res = await _userManager.CreateAsync(new Users
            {
                Email = user.Email,
                FullName = user.FullName,
                UserName = user.Email,
                levelId = user.levelId,
                IsMempershipPaid = false
            }, user.Password);
            if (res.Succeeded)
            {
                Users findUser = await _userManager.FindByEmailAsync(user.Email);
                var roleRes = await _userManager.AddToRoleAsync(findUser, user.RoleId);
                await _singIn.SignInAsync(findUser, isPersistent: false);

                return RedirectToAction("Index", "Forums");
            }
            else
            {
                List<SelectListItem> roleList = new();
                List<IdentityRole> roles = _role.Roles.ToList();
                foreach (var role in roles)
                {
                    roleList.Add(new SelectListItem
                    {
                        Text = role.Name,
                        Value = role.Name
                    });
                }
                ViewBag.roleList = roleList;

                List<Level> level = _context.levels.ToList();
                List<SelectListItem> selectList = new();
                level.ForEach(l =>
                {
                    selectList.Add(new SelectListItem
                    {
                        Value = l.Id.ToString(),
                        Text = l.TypeName
                    });
                });
                ViewBag.LevelsListItem = selectList;
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
        public IActionResult Login(string? ReturnUrl)
        {
            if (ReturnUrl == null)
                ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        public async Task<IActionResult> SignOutAsync()
        {
            await _singIn.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(SignInUser user, string? ReturnUrl)
        {
            var res = await _singIn.PasswordSignInAsync(user.Email, user.Password, isPersistent: false, lockoutOnFailure: false);
            if (res.Succeeded)
            {
                Users findUser = await _userManager.FindByEmailAsync(user.Email);
                if (ReturnUrl != null)
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Forums");
            }
            ModelState.AddModelError("", "wrong username of password");
            return View(user);
        }
    }
}
