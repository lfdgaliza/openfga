using System.ComponentModel.DataAnnotations;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account;

public class LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
    : PageModel
{
    [BindProperty]
    public InputModel? Input { get; set; }

    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        public string? Username { get; init; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; init; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; init; }
    }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (!ModelState.IsValid) return Page();
        
        Guard.Against.Null(Input, nameof(Input));
        Guard.Against.NullOrEmpty(Input.Username, nameof(Input.Username));
        Guard.Against.NullOrEmpty(Input.Password, nameof(Input.Password));
        
        var result = await signInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            logger.LogInformation("User logged in.");
            return LocalRedirect(returnUrl);
        }
        if (result.RequiresTwoFactor)
        {
            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        }
        if (result.IsLockedOut)
        {
            logger.LogWarning("User account locked out.");
            return RedirectToPage("./Lockout");
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return Page();
    }
}