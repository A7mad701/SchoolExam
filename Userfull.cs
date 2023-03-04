public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}


--------------------------------------------------------------------

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    // DbContext constructor and configuration goes here
}
------------------------------------------------------------------------------------------
public class UsersController : Controller
{
    private readonly UserDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UsersController(UserDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    // GET: Users/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Users/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login([Bind("Username,Password")] User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

        if (existingUser == null)
        {
            ModelState.AddModelError("Username", "Invalid username or password");
            return View(user);
        }

        var result = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.Password, user.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError("Password", "Invalid username or password");
            return View(user);
        }

        // Set the authentication cookie
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, existingUser.Username),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Home");
    }

    // GET: Users/Logout
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    // GET: Users/Register
    public IActionResult Register()
    {
        return View();
    }

    // POST: Users/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([Bind("Username,Password,Email")] User user)
    {
        // Check if username is taken
        if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        {
            ModelState.AddModelError("Username", "Username is already taken");
            return View(user);
        }

        // Hash the password
        user.Password = _passwordHasher.HashPassword(user, user.Password);

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

// GET: Users/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var user = await _context.Users.FindAsync(id);
    if (user == null)
    {
        return NotFound();
    }

    return View(user);
}

// POST: Users/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Email")] User user)
{
    if (id != user.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(user.Id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return RedirectToAction(nameof(Index));
    }
    return View(user);
}

// GET: Users/Delete/5
public async Task<IActionResult> Delete(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var user = await _context.Users
        .FirstOrDefaultAsync(m => m.Id == id);
    if (user == null)
    {
        return NotFound();
    }

    return View(user);
}

// POST: Users/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var user = await _context.Users.FindAsync(id);
    _context.Users.Remove(user);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

private bool UserExists(int id)
{
    return _context.Users.Any(e => e.Id == id);
}