namespace EQApp.Models;

public class EQTest
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime CompletedDate { get; set; }
    public int TotalScore { get; set; }
    public List<EQAnswer> Answers { get; set; }
    public string ResultLevel { get; set; }
    public string Recommendations { get; set; }

    public EQTest()
    {
        Id = Guid.NewGuid().ToString();
        CompletedDate = DateTime.Now;
        Answers = new List<EQAnswer>();
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
    public string QuestionText { get; set; }
    public string AnswerText { get; set; }
    public int Score { get; set; }
}