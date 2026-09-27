namespace EQApp.Models;

public class Journal
{
    private List<JournalEntry> _entries;
    public string UserId { get; set; }

    public Journal(string userId)
    {
        UserId = userId;
        _entries = new List<JournalEntry>();
    }

    // Добавить запись
    public void AddEntry(JournalEntry entry)
    {
        entry.UserId = UserId;
        _entries.Add(entry);
    }

    // Получить все записи
    public List<JournalEntry> GetAllEntries()
    {
        return _entries.OrderByDescending(e => e.Date).ToList();
    }

    // Получить записи по настроению
    public List<JournalEntry> GetEntriesByMood(MoodType mood)
    {
        return _entries.Where(e => e.Mood == mood)
                       .OrderByDescending(e => e.Date)
                       .ToList();
    }

    // Получить статистику по настроениям
    public Dictionary<MoodType, int> GetMoodStatistics()
    {
        return _entries.GroupBy(e => e.Mood)
                       .ToDictionary(g => g.Key, g => g.Count());
    }

    // Средняя оценка настроения
    public double GetAverageMoodRating()
    {
        return _entries.Any() ? _entries.Average(e => e.MoodRating) : 0;
    }

    // Последняя запись
    public JournalEntry GetLatestEntry()
    {
        return _entries.OrderByDescending(e => e.Date).FirstOrDefault();
    }
}