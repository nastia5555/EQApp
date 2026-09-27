using EQApp.Models;

namespace EQApp.Views;

public partial class EntryDetailPage : ContentPage
{
    public EntryDetailPage(JournalEntry entry)
    {
        InitializeComponent();

        MoodEmojiLabel.Text = entry.MoodEmoji;
        MoodTextLabel.Text = entry.MoodText;
        MoodRatingLabel.Text = $"Оценка: {entry.MoodRating}/10";
        DateLabel.Text = entry.Date.ToString("dd.MM.yyyy HH:mm");
        EmotionsLabel.Text = string.IsNullOrWhiteSpace(entry.EmotionWords) ? "—" : entry.EmotionWords;
        SituationLabel.Text = entry.Situation;
        ReflectionLabel.Text = string.IsNullOrWhiteSpace(entry.Reflection) ? "—" : entry.Reflection;
    }
}