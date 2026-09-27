using EQApp.Models;
using EQApp.Interfaces;

namespace EQApp.Views;

public partial class ExercisePage : ContentPage
{
    private BaseExercise _exercise;
    private User _user;
    private Journal _journal;
    private Action _onComplete;

    // UI элементы для разных типов упражнений
    private Picker _emotionPicker;
    private Picker _subEmotionPicker;
    private Editor _reflectionEditor;
    private Slider _stressBeforeSlider;
    private Slider _stressAfterSlider;
    private Picker _techniquePicker;
    private Picker _situationPicker;
    private ListView _actionsListView;
    private Entry _bestActionEntry;

    public ExercisePage(BaseExercise exercise, User user, Journal journal, Action onComplete)
    {
        InitializeComponent();
        _exercise = exercise;
        _user = user;
        _journal = journal;
        _onComplete = onComplete;

        LoadExerciseUI();
    }

    private void LoadExerciseUI()
    {
        TitleLabel.Text = _exercise.Title;
        DurationLabel.Text = $"⏱ {_exercise.DurationMinutes} минут";
        InstructionLabel.Text = _exercise.Instruction;

        // Устанавливаем цвет категории
        var categoryColor = _exercise.Category switch
        {
            ExerciseCategory.SelfAwareness => "#4A90E2",
            ExerciseCategory.SelfRegulation => "#5C6BC0",
            ExerciseCategory.Empathy => "#7E57FF",
            ExerciseCategory.SocialSkills => "#FF8A65",
            ExerciseCategory.Motivation => "#4CAF50",
            ExerciseCategory.StressManagement => "#FF9800",
            _ => "#6C757D"
        };

        ContentBorder.BackgroundColor = Color.FromArgb(categoryColor + "20"); // 20% opacity

        // Создаём UI в зависимости от типа упражнения
        switch (_exercise)
        {
            case EmotionWheel wheelEx:
                CreateEmotionWheelUI(wheelEx);
                break;
            case EmpathyExercise empathyEx:
                CreateEmpathyUI(empathyEx);
                break;
            case StressManagement stressEx:
                CreateStressManagementUI(stressEx);
                break;
            case EmotionRegulation regulationEx:
                CreateEmotionRegulationUI(regulationEx);
                break;
        }

        ContentBorder.IsVisible = true;
        CompleteButton.IsVisible = true;
    }

    private void CreateEmotionWheelUI(EmotionWheel exercise)
    {
        var wheel = exercise.GetEmotionWheel();

        // Выбор основной эмоции
        var mainEmotionLabel = new Label { Text = "Выберите основную эмоцию:", FontAttributes = FontAttributes.Bold };
        _emotionPicker = new Picker { Title = "Основная эмоция" };
        foreach (var emotion in wheel.Keys)
            _emotionPicker.Items.Add(emotion);

        _emotionPicker.SelectedIndexChanged += (s, e) =>
        {
            if (_emotionPicker.SelectedIndex >= 0)
            {
                var selected = _emotionPicker.SelectedItem.ToString();
                _subEmotionPicker.Items.Clear();
                foreach (var sub in wheel[selected])
                    _subEmotionPicker.Items.Add(sub);
                _subEmotionPicker.IsEnabled = true;
            }
        };

        // Выбор оттенка эмоции
        var subEmotionLabel = new Label { Text = "Уточните оттенок эмоции:", FontAttributes = FontAttributes.Bold };
        _subEmotionPicker = new Picker { Title = "Оттенок", IsEnabled = false };

        // Ощущения в теле
        var bodyLabel = new Label { Text = "Что вы чувствуете в теле?", FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 10, 0, 0) };
        var bodyEntry = new Entry { Placeholder = "Например: ком в горле, напряжение в плечах..." };

        // Ситуация
        var situationLabel = new Label { Text = "Опишите ситуацию:", FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 10, 0, 0) };
        var situationEditor = new Editor { Placeholder = "Что произошло?", HeightRequest = 80 };

        // Кнопка предпросмотра
        var previewButton = new Button
        {
            Text = "Сохранить ответ",
            BackgroundColor = Color.FromArgb("#4A90E2"),
            TextColor = Colors.White,
            CornerRadius = 10,
            Margin = new Thickness(0, 15, 0, 0)
        };

        previewButton.Clicked += (s, e) =>
        {
            var mainEmotion = _emotionPicker.SelectedItem?.ToString() ?? "";
            var subEmotion = _subEmotionPicker.SelectedItem?.ToString() ?? "";
            var bodySensation = bodyEntry.Text ?? "";
            var situation = situationEditor.Text ?? "";

            exercise.SetUserResponse(mainEmotion, subEmotion, bodySensation, situation);
            _reflectionEditor = new Editor { Placeholder = exercise.GetReflectionPrompt(), HeightRequest = 80 };

            DisplayAlert("Готово", "Ответ сохранён. Теперь вы можете завершить упражнение.", "OK");
        };

        DynamicContent.Children.Add(mainEmotionLabel);
        DynamicContent.Children.Add(_emotionPicker);
        DynamicContent.Children.Add(subEmotionLabel);
        DynamicContent.Children.Add(_subEmotionPicker);
        DynamicContent.Children.Add(bodyLabel);
        DynamicContent.Children.Add(bodyEntry);
        DynamicContent.Children.Add(situationLabel);
        DynamicContent.Children.Add(situationEditor);
        DynamicContent.Children.Add(previewButton);
    }

    private void CreateEmpathyUI(EmpathyExercise exercise)
    {
        var scenario = exercise.GetRandomScenario();

        var titleLabel = new Label { Text = scenario.Title, FontSize = 18, FontAttributes = FontAttributes.Bold };
        var situationLabel = new Label { Text = scenario.Situation, TextColor = Color.FromArgb("#6C757D") };

        var reflectionPrompt = new Label { Text = "Ваш ответ:", FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 15, 0, 0) };
        var reflectionEditor = new Editor { Placeholder = scenario.Questions[0] + "\n\n" + scenario.Questions[1] + "\n\n" + scenario.Questions[2], HeightRequest = 200 };

        var saveButton = new Button
        {
            Text = "Сохранить размышления",
            BackgroundColor = Color.FromArgb("#4A90E2"),
            TextColor = Colors.White,
            CornerRadius = 10,
            Margin = new Thickness(0, 15, 0, 0)
        };

        saveButton.Clicked += (s, e) =>
        {
            exercise.SetUserReflection(reflectionEditor.Text);
            _reflectionEditor = new Editor { Placeholder = exercise.GetReflectionPrompt(), HeightRequest = 80 };
            DisplayAlert("Готово", "Ваши ответы сохранены", "OK");
        };

        DynamicContent.Children.Add(titleLabel);
        DynamicContent.Children.Add(situationLabel);
        DynamicContent.Children.Add(reflectionPrompt);
        DynamicContent.Children.Add(reflectionEditor);
        DynamicContent.Children.Add(saveButton);
    }

    private void CreateStressManagementUI(StressManagement exercise)
    {
        var beforeLabel = new Label { Text = "Оцените уровень стресса ДО выполнения (1-10):", FontAttributes = FontAttributes.Bold };
        _stressBeforeSlider = new Slider { Minimum = 1, Maximum = 10, Value = 5 };
        var beforeValueLabel = new Label { Text = "5", HorizontalOptions = LayoutOptions.Center };
        _stressBeforeSlider.ValueChanged += (s, e) => beforeValueLabel.Text = e.NewValue.ToString("F0");

        // Выбор техники
        var techniqueLabel = new Label { Text = "Выберите технику заземления:", FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 15, 0, 0) };
        _techniquePicker = new Picker { Title = "Техника" };

        var techniques = exercise.GetTechniques();
        foreach (var t in techniques)
            _techniquePicker.Items.Add(t.Name);

        var techniqueDescription = new Label { TextColor = Color.FromArgb("#6C757D") };
        var techniqueSteps = new StackLayout { Margin = new Thickness(10) };

        _techniquePicker.SelectedIndexChanged += (s, e) =>
        {
            if (_techniquePicker.SelectedIndex >= 0)
            {
                var tech = techniques[_techniquePicker.SelectedIndex];
                techniqueDescription.Text = tech.Description;
                techniqueSteps.Children.Clear();
                foreach (var step in tech.Steps)
                    techniqueSteps.Children.Add(new Label { Text = $"• {step}", FontSize = 13 });
            }
        };

        // После выполнения
        var afterLabel = new Label { Text = "Оцените уровень стресса ПОСЛЕ выполнения (1-10):", FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 15, 0, 0) };
        _stressAfterSlider = new Slider { Minimum = 1, Maximum = 10, Value = 5 };
        var afterValueLabel = new Label { Text = "5", HorizontalOptions = LayoutOptions.Center };
        _stressAfterSlider.ValueChanged += (s, e) => afterValueLabel.Text = e.NewValue.ToString("F0");

        var reportLabel = new Label { Text = "Опишите ваш опыт:", FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 15, 0, 0) };
        var reportEditor = new Editor { Placeholder = "Какая техника помогла? Что вы чувствовали?", HeightRequest = 100 };

        var saveButton = new Button
        {
            Text = "Сохранить результаты",
            BackgroundColor = Color.FromArgb("#4A90E2"),
            TextColor = Colors.White,
            CornerRadius = 10,
            Margin = new Thickness(0, 15, 0, 0)
        };

        saveButton.Clicked += (s, e) =>
        {
            exercise.SetStressLevels((int)_stressBeforeSlider.Value, (int)_stressAfterSlider.Value);
            exercise.SetExperienceReport(reportEditor.Text);
            _reflectionEditor = new Editor { Placeholder = exercise.GetReflectionPrompt(), HeightRequest = 80 };
            DisplayAlert("Готово", "Результаты сохранены", "OK");
        };

        DynamicContent.Children.Add(beforeLabel);
        DynamicContent.Children.Add(_stressBeforeSlider);
        DynamicContent.Children.Add(beforeValueLabel);
        DynamicContent.Children.Add(techniqueLabel);
        DynamicContent.Children.Add(_techniquePicker);
        DynamicContent.Children.Add(techniqueDescription);
        DynamicContent.Children.Add(techniqueSteps);
        DynamicContent.Children.Add(afterLabel);
        DynamicContent.Children.Add(_stressAfterSlider);
        DynamicContent.Children.Add(afterValueLabel);
        DynamicContent.Children.Add(reportLabel);
        DynamicContent.Children.Add(reportEditor);
        DynamicContent.Children.Add(saveButton);
    }

    private void CreateEmotionRegulationUI(EmotionRegulation exercise)
    {
        var situationLabel = new Label { Text = "Выберите ситуацию:", FontAttributes = FontAttributes.Bold };
        _situationPicker = new Picker { Title = "Ситуация" };

        var situations = exercise.GetSituations();
        foreach (var s in situations)
            _situationPicker.Items.Add(s.Title);

        var situationDescription = new Label { TextColor = Color.FromArgb("#6C757D") };
        var emotionDescription = new Label { TextColor = Color.FromArgb("#FF9800") };

        _situationPicker.SelectedIndexChanged += (s, e) =>
        {
            if (_situationPicker.SelectedIndex >= 0)
            {
                var sit = situations[_situationPicker.SelectedIndex];
                situationDescription.Text = sit.Situation;
                emotionDescription.Text = $"😤 Испытываемая эмоция: {sit.Emotion}";
                exercise.SetChosenSituation(sit.Title, sit.Situation);

                // Показываем возможные реакции
                _actionsListView.ItemsSource = sit.CommonReactions;
            }
        };

        var actionsLabel = new Label { Text = "Возможные реакции:", FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 15, 0, 0) };
        _actionsListView = new ListView { HeightRequest = 200 };

        var bestActionLabel = new Label { Text = "Какая реакция самая конструктивная?", FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 15, 0, 0) };
        _bestActionEntry = new Entry { Placeholder = "Напишите ваш ответ" };

        var reflectionLabel = new Label { Text = "Почему эта реакция лучше других?", FontAttributes = FontAttributes.Bold, Margin = new Thickness(0, 15, 0, 0) };
        var reflectionEditor = new Editor { Placeholder = "Объясните свой выбор", HeightRequest = 80 };

        var saveButton = new Button
        {
            Text = "Сохранить ответ",
            BackgroundColor = Color.FromArgb("#4A90E2"),
            TextColor = Colors.White,
            CornerRadius = 10,
            Margin = new Thickness(0, 15, 0, 0)
        };

        saveButton.Clicked += (s, e) =>
        {
            var possibleActions = ((IEnumerable<string>)_actionsListView.ItemsSource)?.ToList() ?? new List<string>();
            exercise.SetPossibleActions(possibleActions);
            exercise.SetBestAction(_bestActionEntry.Text ?? "");
            exercise.SetReflection(reflectionEditor.Text);
            _reflectionEditor = new Editor { Placeholder = exercise.GetReflectionPrompt(), HeightRequest = 80 };
            DisplayAlert("Готово", "Ответ сохранён", "OK");
        };

        DynamicContent.Children.Add(situationLabel);
        DynamicContent.Children.Add(_situationPicker);
        DynamicContent.Children.Add(situationDescription);
        DynamicContent.Children.Add(emotionDescription);
        DynamicContent.Children.Add(actionsLabel);
        DynamicContent.Children.Add(_actionsListView);
        DynamicContent.Children.Add(bestActionLabel);
        DynamicContent.Children.Add(_bestActionEntry);
        DynamicContent.Children.Add(reflectionLabel);
        DynamicContent.Children.Add(reflectionEditor);
        DynamicContent.Children.Add(saveButton);
    }

    private async void OnCompleteClicked(object sender, EventArgs e)
    {
        CompleteButton.IsEnabled = false;
        ProgressBar.IsVisible = true;

        // Имитация прогресса
        for (int i = 0; i <= 100; i += 10)
        {
            ProgressBar.Progress = i / 100d;
            await Task.Delay(50);
        }

        var result = await _exercise.StartAsync();

        // Сохраняем результат в журнал (создаём запись в дневнике)
        var journalEntry = new JournalEntry(_user.Id)
        {
            Mood = result.Score >= 70 ? MoodType.Good : MoodType.Neutral,
            MoodRating = result.Score / 10,
            EmotionWords = $"Упражнение: {result.ExerciseTitle}",
            Situation = $"Результат: {result.Score} из {result.MaxScore} баллов",
            Reflection = result.Feedback + "\n\n" + string.Join("\n", result.KeyInsights)
        };
        _journal.AddEntry(journalEntry);

        await DisplayAlert(
            $"Результат: {result.Score} / {result.MaxScore}",
            result.Feedback + "\n\n💡 Основные выводы:\n" + string.Join("\n", result.KeyInsights),
            "Отлично");

        _onComplete?.Invoke();
        await Navigation.PopAsync();
    }
}