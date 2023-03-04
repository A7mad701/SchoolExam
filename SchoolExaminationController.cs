public class SchoolExaminationsController : Controller
{
    private readonly SchoolDbContext _context;

    public SchoolExaminationsController(SchoolDbContext context)
    {
        _context = context;
    }

    // GET: SchoolExaminations
    public async Task<IActionResult> Index()
    {
        return View(await _context.SchoolExaminations.ToListAsync());
    }

    // GET: SchoolExaminations/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var schoolExamination = await _context.SchoolExaminations
            .FirstOrDefaultAsync(m => m.Id == id);
        if (schoolExamination == null)
        {
            return NotFound();
        }

        return View(schoolExamination);
    }

    // GET: SchoolExaminations/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: SchoolExaminations/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Date,Subject,Marks")] SchoolExamination schoolExamination)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schoolExamination);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(schoolExamination);
    }

    // GET: SchoolExaminations/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var schoolExamination = await _context.SchoolExaminations.FindAsync(id);
        if (schoolExamination == null)
        {
            return NotFound();
        }
        return View(schoolExamination);
    }

    // POST: SchoolExaminations/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date,Subject,Marks")] SchoolExamination schoolExamination)
    {
        if (id != schoolExamination
