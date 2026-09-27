namespace EQApp.Models;

public class JournalEntry : IComparable<JournalEntry>
{
    public string Id { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public MoodType Mood { get; set; }
    public int MoodRating { get; set; }      // 1-10
    public string EmotionWords { get; set; } = string.Empty;
    public string Situation { get; set; } = string.Empty;
    public string Reflection { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;

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

    // Для IComparable - сортировка по дате (новые сначала)
    public int CompareTo(JournalEntry? other)
    {
        if (other == null) return 1;
        return other.Date.CompareTo(this.Date);
    }

    // Для IReportGenerator
    public string GenerateReport()
    {
        return $"[{Date:dd.MM.yyyy}] {MoodEmoji} {MoodText} ({MoodRating}/10)\n" +
               $"Ситуация: {Situation}\n" +
               $"Рефлексия: {Reflection}\n";
    }
}