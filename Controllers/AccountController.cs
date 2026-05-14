using EasyLearn.Models;
using EasyLearn.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace EasyLearn.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IProfileService _profileService;
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IProfileService profileService, ApplicationDbContext context, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _profileService = profileService;
        _context = context;
        _emailService = emailService;
    }

    [HttpGet]
    public IActionResult Register() => View("RegisterModern");

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await EnsureRolesExist();
                var role = !string.IsNullOrEmpty(model.Role) ? model.Role : "Student";
                await _userManager.AddToRoleAsync(user, role);
                await _profileService.GetOrCreateProfileAsync(user.Id);
                
                // Store registration entry
                var registrationEntry = new RegistrationEntry
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RegistrationTime = DateTime.UtcNow,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
                    UserAgent = HttpContext.Request.Headers.UserAgent.ToString(),
                    IsEmailVerified = false,
                    Role = role
                };
                _context.RegistrationEntries.Add(registrationEntry);
                await _context.SaveChangesAsync();
                
                await _signInManager.SignInAsync(user, isPersistent: false);

                _ = _emailService.SendEmailAsync(
                    user.Email!,
                    "Welcome to EasyLearn!",
                    $"""
                    <div style="font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:30px;border:1px solid #e0e0e0;border-radius:10px">
                        <h2 style="color:#4f46e5">Welcome to EasyLearn! 🎓</h2>
                        <p>Hi <strong>{user.FirstName} {user.LastName}</strong>,</p>
                        <p>We're thrilled to have you on board! Your account has been successfully created.</p>
                        <p>You can now browse courses, take exams, and begin your learning journey.</p>
                        <p style="margin-top:20px">Happy Learning! 📚</p>
                        <p style="color:#888;font-size:13px">— The EasyLearn Team</p>
                    </div>
                    """
                );

                return role switch
                {
                    "Admin" => RedirectToAction("Index", "Admin"),
                    "Instructor" => RedirectToAction("Index", "Instructor"),
                    _ => RedirectToAction("Index", "Student")
                };
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        return View("RegisterModern", model);
    }

    [HttpGet]
    public IActionResult Login() => View("LoginModern");

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            
            // Only store login entry if user exists
            if (user != null)
            {
                var loginEntry = new LoginEntry
                {
                    UserId = user.Id,
                    Email = model.Email,
                    LoginTime = DateTime.UtcNow,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    UserAgent = HttpContext.Request.Headers.UserAgent.ToString(),
                    IsSuccessful = result.Succeeded,
                    FailureReason = result.Succeeded ? null : "Invalid credentials"
                };
                _context.LoginEntries.Add(loginEntry);
                await _context.SaveChangesAsync();
            }
            
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user!);
                await _profileService.GetOrCreateProfileAsync(user!.Id);

                _ = _emailService.SendEmailAsync(
                    user!.Email!,
                    "EasyLearn - Welcome Back!",
                    $"""
                    <div style="font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:30px;border:1px solid #e0e0e0;border-radius:10px">
                        <h2 style="color:#4f46e5">Welcome Back, {user.FirstName}!</h2>
                        <p>Hi <strong>{user.FirstName} {user.LastName}</strong>,</p>
                        <p>You have successfully logged in to your EasyLearn account.</p>
                        <p>Login time: <strong>{DateTime.Now:dd MMM yyyy, hh:mm tt}</strong></p>
                        <p>If this wasn't you, please change your password immediately.</p>
                        <p style="color:#888;font-size:13px">— The EasyLearn Team</p>
                    </div>
                    """
                );

                return roles.FirstOrDefault() switch
                {
                    "Admin" => RedirectToAction("Index", "Admin"),
                    "Instructor" => RedirectToAction("Index", "Instructor"),
                    _ => RedirectToAction("Index", "Student")
                };
            }
            ModelState.AddModelError("", "Invalid login attempt.");
        }
        return View("LoginModern", model);
    }

    [HttpPost]
    public async Task<IActionResult> ExternalLogin(string provider, string returnUrl = null!)
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null!, string remoteError = null!)
    {
        if (remoteError != null)
        {
            ModelState.AddModelError("", $"Error from external provider: {remoteError}");
            return View("LoginModern");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            return RedirectToAction(nameof(Login));
        }

        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            var roles = await _userManager.GetRolesAsync(user!);
            await _profileService.GetOrCreateProfileAsync(user!.Id);
            
            return roles.FirstOrDefault() switch
            {
                "Admin" => RedirectToAction("Index", "Admin"),
                "Instructor" => RedirectToAction("Index", "Instructor"),
                _ => RedirectToAction("Index", "Student")
            };
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? "";
        var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "";

        if (email != null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName
                };

                await _userManager.CreateAsync(user);
                await EnsureRolesExist();
                await _userManager.AddToRoleAsync(user, "Student");
                await _profileService.GetOrCreateProfileAsync(user.Id);
            }

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Student");
        }

        return View("LoginModern");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var otp = new Random().Next(1000, 9999).ToString();
                HttpContext.Session.SetString("OTP", otp);
                HttpContext.Session.SetString("OTP_Email", model.Email);
                HttpContext.Session.SetString("OTP_Expiry", DateTime.UtcNow.AddMinutes(10).ToString());

                await _emailService.SendEmailAsync(
                    model.Email,
                    "EasyLearn - Your Password Reset OTP",
                    $"""
                    <div style="font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:30px;border:1px solid #e0e0e0;border-radius:10px">
                        <h2 style="color:#4f46e5">Password Reset OTP</h2>
                        <p>Hi <strong>{user.FirstName}</strong>,</p>
                        <p>We received a request to reset your EasyLearn account password. Use the OTP below to proceed:</p>
                        <div style="font-size:40px;font-weight:bold;letter-spacing:10px;color:#4f46e5;text-align:center;padding:20px;background:#f5f3ff;border-radius:8px;margin:20px 0">{otp}</div>
                        <p>This OTP is valid for <strong>10 minutes</strong> only.</p>
                        <p>If you did not request a password reset, please ignore this email.</p>
                        <p style="color:#888;font-size:13px">— The EasyLearn Team</p>
                    </div>
                    """
                );

                TempData["OtpEmail"] = model.Email;
                return RedirectToAction(nameof(VerifyOtp));
            }
            ModelState.AddModelError("", "No account found with this email address.");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult VerifyOtp()
    {
        var email = TempData["OtpEmail"]?.ToString();
        if (string.IsNullOrEmpty(email)) return RedirectToAction(nameof(ForgotPassword));
        TempData.Keep("OtpEmail");
        return View(new VerifyOtpViewModel { Email = email });
    }

    [HttpPost]
    public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var sessionOtp = HttpContext.Session.GetString("OTP");
        var sessionEmail = HttpContext.Session.GetString("OTP_Email");
        var expiryStr = HttpContext.Session.GetString("OTP_Expiry");

        if (sessionOtp == null || sessionEmail == null || expiryStr == null)
        {
            ModelState.AddModelError("", "OTP has expired. Please try again.");
            return View(model);
        }

        if (DateTime.Parse(expiryStr) < DateTime.UtcNow)
        {
            HttpContext.Session.Remove("OTP");
            ModelState.AddModelError("", "OTP has expired. Please request a new one.");
            return View(model);
        }

        if (model.Otp != sessionOtp || model.Email != sessionEmail)
        {
            ModelState.AddModelError("", "Invalid OTP. Please try again.");
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "User not found.");
            return View(model);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

        if (result.Succeeded)
        {
            HttpContext.Session.Remove("OTP");
            HttpContext.Session.Remove("OTP_Email");
            HttpContext.Session.Remove("OTP_Expiry");
            TempData["Success"] = "Your password has been changed successfully. Please log in.";
            return RedirectToAction(nameof(Login));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);
        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPassword(string userId, string token) => View(new ResetPasswordViewModel { UserId = userId, Token = token });

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.UserId != null && model.Token != null)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        TempData["Success"] = "Password reset successfully.";
                        return RedirectToAction(nameof(Login));
                    }
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
        }
        return View(model);
    }

    private async Task EnsureRolesExist()
    {
        string[] roles = { "Admin", "Instructor", "Student" };
        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

public class RegisterViewModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string Role { get; set; } = "Student";
}

public class LoginViewModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
}

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}

public class VerifyOtpViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "OTP is required")]
    [StringLength(4, MinimumLength = 4, ErrorMessage = "OTP must be 4 digits")]
    public string Otp { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your password")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

public class ResetPasswordViewModel
{
    public string? UserId { get; set; }
    public string? Token { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
    
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}