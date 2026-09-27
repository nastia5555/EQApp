using EQApp.Models;
using EQApp.Services;
using EQApp.Delegates;
using EQApp.Interfaces;

namespace EQApp.Views;

public partial class MainPage : ContentPage
{
    private User _currentUser;
    private Journal _journal;
    private ExerciseService _exerciseService;
    private DataStorageService _storage;
    private List<CompletedExercise> _completedExercises;

    public MainPage()
    {
        InitializeComponent();

        _storage = new DataStorageService();
        _exerciseService = new ExerciseService();
        _completedExercises = new List<CompletedExercise>();

        // Загружаем данные асинхронно
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            var loadUserTask = _storage.LoadUserAsync();
            var loadJournalTask = _storage.LoadJournalAsync();
            var loadExercisesTask = _storage.LoadCompletedExercisesAsync();

            await Task.WhenAll(loadUserTask, loadJournalTask, loadExercisesTask);

            var savedUser = await loadUserTask;
            var savedJournal = await loadJournalTask;
            var savedExercises = await loadExercisesTask;

            if (savedUser != null)
            {
                _currentUser = savedUser;
                Console.WriteLine($"Загружен пользователь: {_currentUser.Name}");
            }
            else
            {
                _currentUser = new User("Алексей", "alex@example.com");
                await _storage.SaveUserAsync(_currentUser);
                Console.WriteLine("Создан новый пользователь");
            }

            _journal = new Journal(_currentUser.Id);
            if (savedJournal != null && savedJournal.Any())
            {
                _journal.LoadEntries(savedJournal);
                Console.WriteLine($"Загружено записей: {_journal.GetAllEntries().Count}");
            }
            else
            {
                var testEntry = new JournalEntry(_currentUser.Id)
                {
                    Mood = MoodType.Good,
                    MoodRating = 7,
                    Situation = "Добро пожаловать в приложение EQ!",
                    Reflection = "Ведите дневник эмоций, выполняйте упражнения и развивайте эмоциональный интеллект."
                };
                _journal.AddEntry(testEntry);
                await _storage.SaveJournalAsync(_journal);
            }

            if (savedExercises != null)
            {
                _completedExercises = savedExercises;
                Console.WriteLine($"Загружено упражнений: {_completedExercises.Count}");
            }

            SetupDelegatesAndEvents();
            UpdateUI();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки: {ex.Message}");

            _currentUser ??= new User("Алексей", "alex@example.com");
            _journal ??= new Journal(_currentUser.Id);

            SetupDelegatesAndEvents();
            UpdateUI();

        }
    }

    private void SetupDelegatesAndEvents()
    {
        if (_journal == null || _exerciseService == null || _currentUser == null)
        {
            Console.WriteLine("Ошибка: объекты не инициализированы");
            return;
        }

        _journal.EntryAdded += OnJournalEntryAdded;
        _journal.EntriesCleared += OnJournalEntriesCleared;
        _exerciseService.ExerciseCompleted += OnExerciseCompleted;

        _exerciseService.ExerciseFilter = (exercise) =>
        {
            if (_currentUser != null && exercise != null)
            {
                _currentUser.SendNotification($"Доступно упражнение: {exercise.Title}");
            }
            return exercise != null && exercise.DifficultyLevel <= 2;
        };

        bool isFirstLaunch = Preferences.Get("is_first_launch", true);
        if (isFirstLaunch)
        {
            _currentUser.SendNotification("Добро пожаловать в приложение!");
            Preferences.Set("is_first_launch", false);
        }

        StatisticsService.RegisterResult(0);
    }

    private async void SaveAllData()
    {
        try
        {
            await Task.WhenAll(
                _storage.SaveUserAsync(_currentUser),
                _storage.SaveJournalAsync(_journal),
                _storage.SaveCompletedExercisesAsync(_completedExercises)
            );
            Console.WriteLine("Все данные сохранены");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
        }
    }

    private async void OnExercisesClicked(object sender, EventArgs e)
    {
        var exercises = _exerciseService.GetAllExercises();

        if (!exercises.Any())
        {
            await DisplayAlertAsync("Упражнения", "Список упражнений пуст", "OK");
            return;
        }

        var options = exercises.Select(ex => $"{ex.Title} ({ex.DurationMinutes} мин)").ToArray();
        var result = await DisplayActionSheet("Выберите упражнение", "Отмена", null, options);

        if (result != null && result != "Отмена")
        {
            var selected = exercises.FirstOrDefault(ex => $"{ex.Title} ({ex.DurationMinutes} мин)" == result);
            if (selected != null)
            {
                await StartExercise(selected);
            }
        }
    }

    private async void OnQuickExerciseClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        string? title = button?.Text;

        var exercise = _exerciseService.GetAllExercises().FirstOrDefault(ex => ex.Title == title);
        if (exercise != null)
        {
            await StartExercise(exercise);
        }
    }

    private async Task StartExercise(BaseExercise exercise)
    {
        await Navigation.PushAsync(new ExerciseExecutionPage(exercise, _currentUser, _journal, () =>
        {
            UpdateUI();
            SaveAllData();
        }));
    }

    private async void OnEQTestClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EQTestPage(_currentUser, () =>
        {
            UpdateUI();
            SaveAllData();
        }));
    }

    private async void OnJournalClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new JournalPage(_currentUser, _journal, () =>
        {
            UpdateUI();
            SaveAllData();
        }));
    }


    private void OnJournalEntryAdded(object? sender, JournalEntry entry)
    {
        Console.WriteLine($"[СОБЫТИЕ] Добавлена запись: {entry.MoodEmoji}");
        _currentUser?.SendNotification($"Новая запись: {entry.MoodText}");
        SaveAllData();
        UpdateUI();
    }

    private void OnJournalEntriesCleared(object? sender, int count)
    {
        Console.WriteLine($"[СОБЫТИЕ] Удалено {count} записей");
        SaveAllData();
        UpdateUI();
    }

    private void OnExerciseCompleted(ExerciseResult result)
    {
        _currentUser?.SendNotification($"Вы набрали {result.Score} баллов!");
        StatisticsService.RegisterResult(result.Score);
        _currentUser.ExercisesCompleted++;
        var completed = new CompletedExercise(_currentUser.Id, result.ExerciseTitle, result.Score, result.Feedback);
        _completedExercises.Add(completed);
        SaveAllData();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await DisplayAlertAsync(
                $"Результат: {result.Score} / {result.MaxScore}",
                $"{result.Feedback}\n\nОсновные выводы:\n{string.Join("\n", result.KeyInsights)}",
                "Отлично");
            UpdateUI();
        });
    }

    private void UpdateUI()
    {
        if (_currentUser == null) return;
        if (_currentUser.TestsCompleted > 0)
        {
            EQLevelLabel.Text = $"Ваш уровень EQ: {_currentUser.GetEQLevel()}";
        }
        else
        {
            EQLevelLabel.Text = "Пройдите тест, чтобы узнать свой EQ";
        }

        TestsCountLabel.Text = _currentUser.TestsCompleted.ToString();
        AverageEQLabel.Text = $"{_currentUser.AverageEQ:F0}";

        if (JournalCountLabel != null)
        {
            JournalCountLabel.Text = _journal?.GetAllEntries().Count.ToString() ?? "0";
        }

        if (ExercisesCountLabel != null)
        {
            ExercisesCountLabel.Text = _currentUser.ExercisesCompleted.ToString();
        }

        var latest = _journal?.GetLatestEntry();
        if (latest != null && LatestEntryFrame != null)
        {
            LatestEntryFrame.IsVisible = true;
            LatestMoodLabel.Text = $"{latest.MoodEmoji} {latest.MoodText} ({latest.MoodRating}/10)";
            LatestEntryText.Text = latest.Situation;
        }
        else if (LatestEntryFrame != null)
        {
            LatestEntryFrame.IsVisible = false;
        }
    }
}