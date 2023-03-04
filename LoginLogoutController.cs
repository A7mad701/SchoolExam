public class LoginController : Controller
{
    private readonly MyDbContext _context;

    public LoginController(MyDbContext context)
    {
        _context = context;
    }

    // GET: Login
    public IActionResult Index()
    {
        return View();
    }

    // POST: Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([Bind("Username,Password")] LoginModel loginModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginModel.Username);
            if (user != null)
            {
                var hasher = new PasswordHasher<User>();
                if (hasher.VerifyHashedPassword(user, user.Password, loginModel.Password) == PasswordVerificationResult.Success)
                {
                    // Set the authentication cookie
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        ModelState.AddModelError(string.Empty, "Invalid username or password.");
        return View(loginModel);
    }
}

public class LogoutController : Controller
{
    // GET: Logout
    public async Task<IActionResult> Index()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
