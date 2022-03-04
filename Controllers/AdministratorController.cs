using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetMvc.Models;

namespace NetMvc.Controllers;

[Authorize]
public class AdministratorController : Controller
{
    private readonly RoleManager<IdentityRole> roleManager;

    public AdministratorController(RoleManager<IdentityRole> roleManager)
    {
        this.roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult CreateRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRole model)
    {
        if(ModelState.IsValid)
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = model.RoleName
            };

            IdentityResult result = await roleManager.CreateAsync(identityRole);

            if(result.Succeeded)
            {
                return RedirectToAction("index", "home");
            }

            foreach(IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

        }

        return View(model);
  
    }
}