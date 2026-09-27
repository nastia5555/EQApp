namespace EQApp.Models;

public class EQTest
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime CompletedDate { get; set; }
    public int TotalScore { get; set; }
    public List<EQAnswer> Answers { get; set; } = new();
    public string ResultLevel { get; set; } = string.Empty;
    public string Recommendations { get; set; } = string.Empty;

    public EQTest()
    {
        Id = Guid.NewGuid().ToString();
        CompletedDate = DateTime.Now;
    }

    public EQTest(string userId) : this()
    {
        UserId = userId;
    }

    public void CalculateResult()
    {
        TotalScore = Answers.Sum(a => a.Score);

        if (TotalScore >= 80)
        {
            ResultLevel = "Высокий EQ";
            Recommendations = "Вы отлично понимаете свои и чужие эмоции. Продолжайте практиковать эмпатию!";
        }
        else if (TotalScore >= 60)
        {
            ResultLevel = "Средний EQ";
            Recommendations = "У вас хорошее понимание эмоций. Рекомендуем вести дневник эмоций.";
        }
        else if (TotalScore >= 40)
        {
            ResultLevel = "Ниже среднего";
            Recommendations = "Практикуйте осознанность и наблюдайте за своими реакциями.";
        }
        else
        {
            ResultLevel = "Начальный уровень";
            Recommendations = "Начните с дневника эмоций и дыхательных практик.";
        }
    }
}

public class EQAnswer
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string AnswerText { get; set; } = string.Empty;
    public int Score { get; set; }
}