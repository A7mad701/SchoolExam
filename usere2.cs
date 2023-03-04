public class UserController : Controller
{
    private readonly UserDbContext _context;

    public UserController(UserDbContext context)
    {
        _context = context;
    }

    // GET: User
    public async Task<IActionResult> Index()
    {
        return View(await _context.Users.ToListAsync());
    }

    // GET: User/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: User/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([Bind("Username,Password,Email")] User user)
    {
        if (ModelState.IsValid)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    // GET: User/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: User/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (ModelState.IsValid)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                // Set authentication cookie and redirect to dashboard
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, user.Username) }, CookieAuthenticationDefaults.AuthenticationScheme)));
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
            }
        }
        return View();
    }

    // POST: User/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}

-------------------------------------------------------


public void ConfigureServices(IServiceCollection services)
{
    // Add DbContext
    services.AddDbContext<UserDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

    // Add authentication services
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.LoginPath = "/User/Login";
            options.LogoutPath = "/User/Logout";
            options.AccessDeniedPath = "/AccessDenied";
        });

    services.AddControllersWithViews();
}

public




--------------------------------------------

[Authorize]
public class HomeController : Controller
{
    // GET: Home/Dashboard
    public IActionResult Dashboard()
    {
        return View();
    }

    // GET: Home/Profile
    public IActionResult Profile()
    {
        return View();
    }

    // Other actions go here
}

[AllowAnonymous]
public class HomeController : Controller
{
    // GET: Home/Index
    public IActionResult Index()
    {
        return View();
    }

    // Other actions go here
}
