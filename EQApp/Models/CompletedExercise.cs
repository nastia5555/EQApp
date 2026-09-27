namespace EQApp.Models;

public class CompletedExercise
{
    public string Id { get; set; } = string.Empty;
    public string ExerciseTitle { get; set; } = string.Empty;
    public int Score { get; set; }
    public DateTime CompletedDate { get; set; }
    public string Feedback { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;

    public CompletedExercise()
    {
        Id = Guid.NewGuid().ToString();
        CompletedDate = DateTime.Now;
    }

    public CompletedExercise(string userId, string title, int score, string feedback) : this()
    {
        UserId = userId;
        ExerciseTitle = title;
        Score = score;
        Feedback = feedback;
    }
}