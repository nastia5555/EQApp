namespace EQApp.Models;

public class JournalEntry
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public MoodType Mood { get; set; }
    public int MoodRating { get; set; }      // 1-10
    public string EmotionWords { get; set; } // Какие эмоции испытывал
    public string Situation { get; set; }    // Что произошло
    public string Reflection { get; set; }   // Рефлексия
    public string UserId { get; set; }

    public JournalEntry()
    {
        Id = Guid.NewGuid().ToString();
        Date = DateTime.Now;
    }

    public JournalEntry(string userId) : this()
    {
        UserId = userId;
    }

    public string MoodEmoji => Mood switch
    {
        MoodType.Awesome => "😍",
        MoodType.Happy => "😊",
        MoodType.Good => "🙂",
        MoodType.Neutral => "😐",
        MoodType.Sad => "😔",
        MoodType.Angry => "😠",
        MoodType.Anxious => "😰",
        MoodType.Tired => "😫",
        _ => "😐"
    };

    public string MoodText => Mood switch
    {
        MoodType.Awesome => "Великолепно",
        MoodType.Happy => "Счастлив",
        MoodType.Good => "Хорошо",
        MoodType.Neutral => "Нейтрально",
        MoodType.Sad => "Грустно",
        MoodType.Angry => "Злюсь",
        MoodType.Anxious => "Тревожно",
        MoodType.Tired => "Устал",
        _ => "Не определено"
    };
}