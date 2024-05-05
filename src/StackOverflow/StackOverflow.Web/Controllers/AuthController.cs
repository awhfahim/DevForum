using System.Text;
using System.Text.Encodings.Web;
using Autofac;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using StackOverflow.Application.Contracts.Features;
using StackOverflow.Application.Contracts.Utilities;
using StackOverflow.Domain.consts;
using StackOverflow.Infrastructure.Membership;
using StackOverflow.Web.Models.AuthModels;

namespace StackOverflow.Web.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly ILifetimeScope _scope;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IRecaptchaService _recaptchaService;
    private readonly IEmailService _emailService;

    public AuthController(ILogger<AccountController> logger, ILifetimeScope scope,
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IRecaptchaService recaptchaService, IEmailService emailService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scope = scope ?? throw new ArgumentNullException(nameof(scope));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _recaptchaService = recaptchaService;
        _emailService = emailService;
    }
    
    [HttpGet]
    public IActionResult Register()
    {
	    var model = new RegisterModel();
	    return View(model);
    }
    
    public IActionResult RegisterConfirmation(string email)
    {
	    return View();
    }
    
    public async Task<IActionResult> ConfirmEmail(string email, string code)
    {
	    var user = await _userManager.FindByEmailAsync(email);
	    if (user == null)
	    {
		    return RedirectToAction("Error", "Home");
	    }
	    var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
	    var result = await _userManager.ConfirmEmailAsync(user, decodedCode);
	    if (result.Succeeded)
	    {
		    return View();
	    }
	    else
	    {
		    return RedirectToAction("Error", "Home");
	    }
    }
    
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
	    if (!ModelState.IsValid) return View(model);
	    var recaptchaResponse = Request.Form["g-recaptcha-response"];
            
	    if (string.IsNullOrEmpty(recaptchaResponse))
	    {
		    ModelState.AddModelError(string.Empty, Messages.RobotVerification);
		    return View(model);
	    }
	    var isCaptchaValid = await _recaptchaService.IsCaptchaValidAsync(recaptchaResponse!);            

	    if (!isCaptchaValid)
	    {
		    ModelState.AddModelError(string.Empty, Messages.InvalidCaptcha);
		    return View(model);
	    }
	    
	    model.Resolve(_scope);
	    var result = await model.RegisterAsync();
	    result.redirectLocation ??= Url.Content("~/");

	    if (result.errors == null) return Redirect(result.redirectLocation);
	    
	    foreach (var error in result.errors)
	    {
		    ModelState.AddModelError(string.Empty, error.Description);
	    }

	    return View(model);

    }

    public async Task<IActionResult> Login( string? returnUrl = null)
    {
		// Clear the existing external cookie to ensure a clean login process
		await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
		if (returnUrl is null) 
		{
			TempData["ReturnUrl"] = Url.Content("~/");	
		}
		else
		{
			TempData["ReturnUrl"] = returnUrl;
		}
		var model = new LoginModel();
        return View(model);
    }
    
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
	{
		if (!ModelState.IsValid) return View(model);
		var user = await _userManager.FindByEmailAsync(model.Email);
        if(user is { EmailConfirmed: false })
        {
	        ViewBag.ResendConfirmation = "Email not confirmed yet. <a href='/Auth/ResendEmailConfirmation?email=" + model.Email + "'>Click here</a> to resend confirmation email.";
	        return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
	        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
	        {
		        return Redirect(model.ReturnUrl);
	        }
	        return RedirectToAction("Index", "Home");
        }
        
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
	 }
    
	public async Task<IActionResult> Logout(string? returnUrl = null)
	{
		await _signInManager.SignOutAsync();

		if (returnUrl != null)
		{
			return LocalRedirect(returnUrl);
		}
		return RedirectToAction("Index", "Home");
	}
	
	[HttpGet]
	public async Task<IActionResult> ResendEmailConfirmation(string email)
	{
		var user = await _userManager.FindByEmailAsync(email);
		if (user == null || user.EmailConfirmed)
		{
			// User does not exist or email is already confirmed
			return RedirectToAction("Error", "Home");
		}

		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
		var callbackUrl = $"https://localhost/Auth/ConfirmEmail?email={user.Email}&code={encodedToken}";

		if (user.Email != null)
		{
			var emailResult = await _emailService.SendSingleEmail(
				user.Email, 
				user.Email, 
				"Confirm your email",
				$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
			);

			if (emailResult == null)
			{
				// Email sending failed
				return RedirectToAction("Error", "Home");
			}
		}

		return View();
	}
    
}