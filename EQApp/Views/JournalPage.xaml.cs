using EQApp.Models;

namespace EQApp.Views;

public partial class JournalPage : ContentPage
{
    private User _user;
    private Journal _journal;
    private Action _onUpdate;

    public JournalPage(User user, Journal journal, Action onUpdate)
    {
        InitializeComponent();
        _user = user;
        _journal = journal;
        _onUpdate = onUpdate;

        LoadEntries();
        UpdateStats();
    }

    private void LoadEntries()
    {
        EntriesListView.ItemsSource = _journal.GetAllEntries();
    }

    private void UpdateStats()
    {
        var entries = _journal.GetAllEntries();
        TotalEntriesLabel.Text = entries.Count.ToString();
        AvgMoodLabel.Text = $"{_journal.GetAverageMoodRating():F1}";

        var stats = _journal.GetMoodStatistics();
        if (stats.Any())
        {
            var mostFrequent = stats.OrderByDescending(x => x.Value).First();
            string moodText = mostFrequent.Key switch
            {
                MoodType.Awesome => "😍 Великолепно",
                MoodType.Happy => "😊 Счастлив",
                MoodType.Good => "🙂 Хорошо",
                MoodType.Neutral => "😐 Нейтрально",
                MoodType.Sad => "😔 Грустно",
                MoodType.Angry => "😠 Злость",
                MoodType.Anxious => "😰 Тревога",
                MoodType.Tired => "😫 Усталость",
                _ => "—"
            };
            MostFrequentMoodLabel.Text = moodText;
        }
        else
        {
            MostFrequentMoodLabel.Text = "—";
        }
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddJournalEntryPage(_user, _journal, () =>
        {
            LoadEntries();
            UpdateStats();
            _onUpdate?.Invoke();
        }));
    }

    // Удаление отдельной записи
    private async void OnDeleteEntryClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var entry = button?.CommandParameter as JournalEntry;

        if (entry == null) return;

        bool confirm = await DisplayAlert("Удаление",
            $"Удалить запись от {entry.Date:dd.MM.yyyy}?",
            "Да", "Нет");

        if (confirm)
        {
            var entries = _journal.GetAllEntries().ToList();
            entries.Remove(entry);
            _journal.LoadEntries(entries);

            LoadEntries();
            UpdateStats();
            _onUpdate?.Invoke();
        }
    }
}