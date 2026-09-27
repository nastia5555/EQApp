using EQApp.Models;

namespace EQApp.Views;

public partial class AddJournalEntryPage : ContentPage
{
    private User _user;
    private Journal _journal;
    private Action _onSaved;

    public AddJournalEntryPage(User user, Journal journal, Action onSaved)
    {
        InitializeComponent();
        _user = user;
        _journal = journal;
        _onSaved = onSaved;

        MoodSlider.ValueChanged += (s, e) =>
        {
            SliderValueLabel.Text = $"{e.NewValue:F0} / 10";
        };
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (MoodPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Внимание", "Выберите настроение", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(SituationEditor.Text))
        {
            await DisplayAlert("Внимание", "Опишите ситуацию", "OK");
            return;
        }

        var mood = (MoodType)MoodPicker.SelectedIndex;

        var entry = new JournalEntry(_user.Id)
        {
            Mood = mood,
            MoodRating = (int)MoodSlider.Value,
            EmotionWords = EmotionsEntry.Text ?? string.Empty,
            Situation = SituationEditor.Text,
            Reflection = ReflectionEditor.Text ?? string.Empty
        };

        _journal.AddEntry(entry);

        await DisplayAlert("Успех", "Запись сохранена в дневник", "OK");

        _onSaved?.Invoke();
        await Navigation.PopAsync();
    }
}