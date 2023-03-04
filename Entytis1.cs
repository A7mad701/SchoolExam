public class Exam
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public int Duration { get; set; }
    public ICollection<Question> Questions { get; set; }
}

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    public ICollection<Option> Options { get; set; }
}

public class Option
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
}
