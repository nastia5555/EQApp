namespace EQApp.Models;

public class EQQuestion
{
    public int Id { get; set; }
    public string Text { get; set; }
    public EQCategory Category { get; set; }
    public List<EQOption> Options { get; set; }

    public EQQuestion(int id, string text, EQCategory category)
    {
        Id = id;
        Text = text;
        Category = category;
        Options = new List<EQOption>();
    }

    public void AddOption(string text, int score)
    {
        Options.Add(new EQOption { Text = text, Score = score });
    }
}

public class EQOption
{
    public string Text { get; set; }
    public int Score { get; set; }
}

public enum EQCategory
{
    SelfAwareness,   // Самоосознание
    SelfRegulation,  // Саморегуляция
    Motivation,      // Мотивация
    Empathy,         // Эмпатия
    SocialSkills     // Социальные навыки
}