public class ExamController : Controller
{
    private readonly ExamContext _context;

    public ExamController(ExamContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var exams = _context.Exams.ToList();
        return View(exams);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Exam exam)
    {
        _context.Exams.Add(exam);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var exam = _context.Exams.FirstOrDefault(e => e.Id == id);
        if (exam == null)
        {
            return NotFound();
        }
        return View(exam);
    }

    [HttpPost]
    public IActionResult Edit(Exam exam)
    {
        _context.Exams.Update(exam);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var exam = _context.Exams.FirstOrDefault(e => e.Id == id);
        if (exam == null)
        {
            return NotFound();
        }
        return View(exam);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        var exam = _context.Exams.FirstOrDefault(e => e.Id == id);
        if (exam == null)
			  {
        return NotFound();
    }
    _context.Exams.Remove(exam);
    _context.SaveChanges();
    return RedirectToAction("Index");
   }
}
