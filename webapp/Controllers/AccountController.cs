using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using webapp.Entity;
using webapp.ViewModels;

namespace webapp.Controllers;

public class AccountController : Controller
{
  
  
    private readonly UserManager<AppUser> _userM;
    private readonly SignInManager<AppUser> _signInM;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ILogger<AccountController> logger)
    {
        _userM = userManager;
        _signInM = signInManager;
        _logger = logger;
    }

    [HttpGet("Register")]
    public IActionResult Register(string returnUrl)
    {
        return View(new RegisterViewModel() { ReturnUrl = returnUrl ?? string.Empty });
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var user = new AppUser()
        {
            Fullname = model.Fullname,
            Email = model.Email,
            PhoneNumber = model.Phone,
        };

        var result = await _userM.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            return LocalRedirect($"/Login?returnUrl={model.ReturnUrl}");
        }

        return BadRequest(JsonSerializer.Serialize(result.Errors));
    }

    [HttpGet("Login")]
    public IActionResult Login(string returnUrl)
    {
        return View(new LoginViewModel() { ReturnUrl = returnUrl ?? string.Empty });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = await _userM.FindByEmailAsync(model.Email);
        if (user != null)
        {
            await _signInM.PasswordSignInAsync(user, model.Password, false, false); // isPersistant

            return LocalRedirect($"{model.ReturnUrl ?? "/"}");
        }

        return BadRequest("Wrong credentials");
    }
      [HttpPost]
    public async Task<IActionResult> Logout()
    {
        return LocalRedirect("/");
    }
    
}