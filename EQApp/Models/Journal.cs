using EQApp.Interfaces;

namespace EQApp.Models;

public class Journal
{
    private List<JournalEntry> _entries;
    public string UserId { get; set; }

    public event EventHandler<JournalEntry>? EntryAdded;
    public event EventHandler<int>? EntriesCleared;

    public Journal(string userId)
    {
        UserId = userId;
        _entries = new List<JournalEntry>();
    }

    public void LoadEntries(List<JournalEntry> entries)
    {
        _entries = entries.Where(e => e.UserId == UserId).ToList();
        _entries = _entries.OrderByDescending(e => e.Date).ToList();
    }

    public void AddEntry(JournalEntry entry)
    {
        entry.UserId = UserId;
        _entries.Add(entry);
        EntryAdded?.Invoke(this, entry);
    }

    public void ClearEntries()
    {
        int count = _entries.Count;
        _entries.Clear();
        EntriesCleared?.Invoke(this, count);
    }

    public List<JournalEntry> GetAllEntries()
    {
        return _entries.OrderByDescending(e => e.Date).ToList();
    }

    public double GetAverageMoodRating()
    {
        if (_entries.Count == 0) return 0;
        return _entries.Average(e => e.MoodRating);
    }

    public Dictionary<MoodType, int> GetMoodStatistics()
    {
        return _entries.GroupBy(e => e.Mood)
                       .ToDictionary(g => g.Key, g => g.Count());
    }

    public List<JournalEntry> GetEntriesByMood(MoodType mood)
    {
        return _entries.Where(e => e.Mood == mood)
                       .OrderByDescending(e => e.Date)
                       .ToList();
    }

    public JournalEntry? GetLatestEntry()
    {
        return _entries.OrderByDescending(e => e.Date).FirstOrDefault();
    }

    public List<JournalEntry> GetSortedEntries()
    {
        var sorted = _entries.ToList();
        sorted.Sort();
        return sorted;
    }
}