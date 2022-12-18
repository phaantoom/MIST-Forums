using Forums.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forums.Controllers
{
    public class AdminstrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _role;

        public AdminstrationController(RoleManager<IdentityRole> role)
        {
            _role = role;
        }
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync(CreateRole createRole)
        {
            var res = await _role.CreateAsync(new IdentityRole { Name = createRole.RoleName });
            if (res.Succeeded)
            {
                return View();
            }
            return View(createRole);
        }
    }
}
