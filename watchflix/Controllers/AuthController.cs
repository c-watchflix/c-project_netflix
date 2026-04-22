using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly watchflix.Services.Authentification _authService;

    public AuthController(watchflix.Services.Authentification authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine($"Login attempt with pseudo: {request.Pseudo}");
            
            var (user, errorMessage) = await _authService.LoginAsync(request.Pseudo, request.Password);

            if (user == null)
            {
                System.Diagnostics.Debug.WriteLine($"Login failed: {errorMessage}");
                return Unauthorized(new { message = errorMessage });
            }

            System.Diagnostics.Debug.WriteLine($"Login successful for user: {user.Pseudo}");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Pseudo),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });

            System.Diagnostics.Debug.WriteLine($"SignIn successful");
            return Ok();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Exception in login: {ex.Message}\n{ex.StackTrace}");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Cookies.Delete(".AspNetCore.Cookies");
        return Ok();
    }

    [HttpGet("state")]
    public IActionResult GetAuthState()
    {
        if (User?.Identity?.IsAuthenticated == true)
        {
            return Ok(User.Identity.Name);
        }
        return Unauthorized();
    }

    public class LoginRequest
    {
        public string Pseudo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}