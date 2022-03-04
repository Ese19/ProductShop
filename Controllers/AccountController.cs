using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetMvc.Models;

namespace NetMvc.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public AccountController(UserManager<IdentityUser> userManager, 
                             SignInManager<IdentityUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }


    [AcceptVerbs("Get", "Post")]
    public async Task<IActionResult> IsEmailInUse(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        if(user == null)
        {
            return Json(true);
        }
        else
        {
            return Json($"Email {email} is already in use");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Register(Register model)
    {
        if(ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("index", "home");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("index", "home");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login model, string returnUrl)
    {
        if(ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if(result.Succeeded)
            {
                if(!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("index", "home");
                }
                
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
        }

        return View(model);
    }
}