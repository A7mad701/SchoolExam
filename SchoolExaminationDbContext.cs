public class SchoolDbContext : DbContext
{
    public DbSet<SchoolExamination> SchoolExaminations { get; set; }

    // DbContext constructor and configuration goes here
}
