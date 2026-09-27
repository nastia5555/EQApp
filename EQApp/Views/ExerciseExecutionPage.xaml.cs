using EQApp.Interfaces;
using EQApp.Models;
using EQApp.Services;
using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;

namespace EQApp.Views;

public partial class ExerciseExecutionPage : ContentPage
{
    private BaseExercise _exercise;
    private User _user;
    private Journal _journal;
    private Action _onComplete;

    private Picker? _emotionPicker;
    private Picker? _subEmotionPicker;
    private Entry? _bodyEntry;
    private Editor? _situationEditor;
    private Editor? _answerEditor;
    private Editor? _reportEditor;
    private Slider? _stressBeforeSlider;
    private Slider? _stressAfterSlider;
    private Picker? _techniquePicker;
    private Editor? _reactionsEditor;
    private Entry? _bestActionEntry;
    private Editor? _reflectionRegulationEditor;
    private Label? _selectedMainLabel;
    private Label? _selectedSubLabel;

    public ExerciseExecutionPage(BaseExercise exercise, User user, Journal journal, Action onComplete)
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

        string categoryText = _exercise.Category switch
        {
            ExerciseCategory.SelfAwareness => "Самоосознание",
            ExerciseCategory.SelfRegulation => "Саморегуляция",
            ExerciseCategory.Empathy => "Эмпатия",
            ExerciseCategory.SocialSkills => "Социальные навыки",
            ExerciseCategory.Motivation => "Мотивация",
            ExerciseCategory.StressManagement => "Управление стрессом",
            _ => "Развитие"
        };
        CategoryLabel.Text = categoryText;
        InstructionLabel.Text = _exercise.Instruction;

        switch (_exercise)
        {
            case EmotionWheel wheel:
                CreateEmotionWheelUI(wheel);
                break;
            case EmpathyExercise empathy:
                CreateEmpathyUI(empathy);
                break;
            case StressManagement stress:
                CreateStressManagementUI(stress);
                break;
            case EmotionRegulation regulation:
                CreateEmotionRegulationUI(regulation);
                break;
            default:
                CreateDefaultUI();
                break;
        }
    }

    private void CreateEmotionWheelUI(EmotionWheel exercise)
    {
        var wheel = exercise.GetEmotionWheel();

        AddSectionHeader("ШАГ 1: ОСНОВНАЯ ЭМОЦИЯ");

        _selectedMainLabel = new Label
        {
            Text = "Не выбрано",
            FontSize = 14,
            TextColor = Color.FromArgb("#FF9800"),
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 8, 0, 15),
            FontAttributes = FontAttributes.Bold
        };

        _emotionPicker = new Picker
        {
            Title = "Выберите основную эмоцию",
            BackgroundColor = Color.FromArgb("#F0F4F8"),
            TextColor = Color.FromArgb("#2C3E50"),
            TitleColor = Color.FromArgb("#6C757D"),
            Margin = new Thickness(0, 5, 0, 0),
            HeightRequest = 45
        };

        foreach (var emotion in wheel.Keys)
            _emotionPicker.Items.Add(emotion);

        _emotionPicker.SelectedIndexChanged += (s, e) =>
        {
            if (_emotionPicker.SelectedIndex >= 0)
            {
                var selected = _emotionPicker.SelectedItem?.ToString();
                if (selected != null && wheel.ContainsKey(selected))
                {
                    _selectedMainLabel.Text = $"Выбрано: {selected}";
                    _selectedMainLabel.TextColor = Color.FromArgb("#4CAF50");

                    _subEmotionPicker!.Items.Clear();
                    foreach (var sub in wheel[selected])
                        _subEmotionPicker.Items.Add(sub);
                    _subEmotionPicker.IsEnabled = true;
                    _subEmotionPicker.BackgroundColor = Color.FromArgb("#E8F0FE");

                    if (_selectedSubLabel != null)
                    {
                        _selectedSubLabel.Text = "Выберите оттенок";
                        _selectedSubLabel.TextColor = Color.FromArgb("#FF9800");
                    }
                }
            }
        };

        InputArea.Children.Add(_emotionPicker);
        InputArea.Children.Add(_selectedMainLabel);

        AddSectionHeader("ШАГ 2: ОТТЕНОК ЭМОЦИИ");

        _selectedSubLabel = new Label
        {
            Text = "Сначала выберите основную эмоцию",
            FontSize = 14,
            TextColor = Color.FromArgb("#999999"),
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 8, 0, 15),
            FontAttributes = FontAttributes.Bold
        };

        _subEmotionPicker = new Picker
        {
            Title = "Уточните оттенок",
            IsEnabled = false,
            BackgroundColor = Color.FromArgb("#F8F9FA"),
            TextColor = Color.FromArgb("#999999"),
            Margin = new Thickness(0, 5, 0, 0),
            HeightRequest = 45
        };

        _subEmotionPicker.SelectedIndexChanged += (s, e) =>
        {
            if (_subEmotionPicker.SelectedIndex >= 0)
            {
                var selected = _subEmotionPicker.SelectedItem?.ToString();
                if (_selectedSubLabel != null)
                {
                    _selectedSubLabel.Text = $"Выбрано: {selected}";
                    _selectedSubLabel.TextColor = Color.FromArgb("#4CAF50");
                }
            }
        };

        InputArea.Children.Add(_subEmotionPicker);
        InputArea.Children.Add(_selectedSubLabel);

        AddSectionHeader("ШАГ 3: ОЩУЩЕНИЯ В ТЕЛЕ");
        _bodyEntry = new Entry
        {
            Placeholder = "Например: напряжение в плечах, ком в горле, учащённое сердцебиение",
            BackgroundColor = Color.FromArgb("#F0F4F8"),
            TextColor = Color.FromArgb("#2C3E50"),
            Margin = new Thickness(0, 5, 0, 15),
            HeightRequest = 45
        };
        InputArea.Children.Add(_bodyEntry);

        AddSectionHeader("ШАГ 4: ОПИСАНИЕ СИТУАЦИИ");
        _situationEditor = new Editor
        {
            Placeholder = "Что произошло? Опишите ситуацию максимально подробно",
            BackgroundColor = Color.FromArgb("#F0F4F8"),
            TextColor = Color.FromArgb("#2C3E50"),
            Margin = new Thickness(0, 5, 0, 15),
            HeightRequest = 100
        };
        InputArea.Children.Add(_situationEditor);
    }

    private void CreateEmpathyUI(EmpathyExercise exercise)
    {
        var scenario = exercise.GetRandomScenario();

        var situationCard = new Border
        {
            StrokeThickness = 1,
            Stroke = Color.FromArgb("#E0E0E0"),
            BackgroundColor = Color.FromArgb("#FFF8E1"),
            StrokeShape = new RoundRectangle { CornerRadius = 15 },
            Padding = new Thickness(15),
            Margin = new Thickness(0, 10, 0, 10)
        };

        var situationStack = new VerticalStackLayout { Spacing = 8 };
        situationStack.Children.Add(new Label
        {
            Text = scenario.Title,
            FontSize = 16,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#E65100")
        });
        situationStack.Children.Add(new Label
        {
            Text = scenario.Situation,
            TextColor = Color.FromArgb("#424242"),
            LineBreakMode = LineBreakMode.WordWrap
        });
        situationCard.Content = situationStack;
        InputArea.Children.Add(situationCard);

        AddSectionHeader("ВАШ ОТВЕТ");

        _answerEditor = new Editor
        {
            Placeholder = $"{scenario.Questions[0]}\n\n{scenario.Questions[1]}\n\n{scenario.Questions[2]}",
            BackgroundColor = Color.FromArgb("#F0F4F8"),
            TextColor = Color.FromArgb("#2C3E50"),
            HeightRequest = 200,
            Margin = new Thickness(0, 5, 0, 15)
        };
        InputArea.Children.Add(_answerEditor);
    }

    private void CreateStressManagementUI(StressManagement exercise)
    {
        AddSectionHeader("ОЦЕНКА ДО ВЫПОЛНЕНИЯ");
        InputArea.Children.Add(new Label
        {
            Text = "Уровень стресса (1-10):",
            FontSize = 14,
            TextColor = Color.FromArgb("#2C3E50"),
            Margin = new Thickness(0, 5, 0, 5)
        });

        _stressBeforeSlider = new Slider { Minimum = 1, Maximum = 10, Value = 5 };
        var beforeLabel = new Label
        {
            Text = "5",
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#4A90E2")
        };
        _stressBeforeSlider.ValueChanged += (s, e) => beforeLabel.Text = e.NewValue.ToString("F0");
        InputArea.Children.Add(_stressBeforeSlider);
        InputArea.Children.Add(beforeLabel);

        AddSectionHeader("ВЫБЕРИТЕ ТЕХНИКУ", 15);

        var techniques = exercise.GetTechniques();
        _techniquePicker = new Picker
        {
            Title = "Техника заземления",
            BackgroundColor = Color.FromArgb("#F0F4F8"),
            TextColor = Color.FromArgb("#2C3E50"),
            Margin = new Thickness(0, 5, 0, 10),
            HeightRequest = 45
        };
        foreach (var t in techniques)
            _techniquePicker.Items.Add(t.Name);

        var techCard = new Border
        {
            StrokeThickness = 1,
            Stroke = Color.FromArgb("#E0E0E0"),
            BackgroundColor = Color.FromArgb("#F5F5F5"),
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = new Thickness(12),
            Margin = new Thickness(0, 5, 0, 10),
            IsVisible = false
        };

        var techStack = new VerticalStackLayout { Spacing = 8 };
        var techDescription = new Label { TextColor = Color.FromArgb("#424242") };
        var techSteps = new StackLayout { Margin = new Thickness(10) };
        techStack.Children.Add(techDescription);
        techStack.Children.Add(techSteps);
        techCard.Content = techStack;

        _techniquePicker.SelectedIndexChanged += (s, e) =>
        {
            if (_techniquePicker.SelectedIndex >= 0)
            {
                var tech = techniques[_techniquePicker.SelectedIndex];
                techDescription.Text = tech.Description;
                techSteps.Children.Clear();
                foreach (var step in tech.Steps)
                    techSteps.Children.Add(new Label
                    {
                        Text = $"• {step}",
                        FontSize = 13,
                        TextColor = Color.FromArgb("#555555"),
                        Margin = new Thickness(0, 3, 0, 3)
                    });
                techCard.IsVisible = true;
            }
        };
        InputArea.Children.Add(_techniquePicker);
        InputArea.Children.Add(techCard);

        var startTechButton = new Button
        {
            Text = "ВЫПОЛНИТЬ ТЕХНИКУ (3 минуты)",
            BackgroundColor = Color.FromArgb("#4A90E2"),
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold,
            CornerRadius = 12,
            HeightRequest = 50,
            Margin = new Thickness(0, 10, 0, 15)
        };
        startTechButton.Clicked += async (s, e) =>
        {
            await DisplayAlertAsync("Выполнение техники",
                "Следуйте инструкциям техники в течение 3 минут.\n\nПосле завершения нажмите OK и оцените результат.",
                "OK");
        };
        InputArea.Children.Add(startTechButton);

        AddSectionHeader("ОЦЕНКА ПОСЛЕ ВЫПОЛНЕНИЯ");
        InputArea.Children.Add(new Label
        {
            Text = "Уровень стресса (1-10):",
            FontSize = 14,
            TextColor = Color.FromArgb("#2C3E50"),
            Margin = new Thickness(0, 5, 0, 5)
        });

        _stressAfterSlider = new Slider { Minimum = 1, Maximum = 10, Value = 5 };
        var afterLabel = new Label
        {
            Text = "5",
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            TextColor = Color.FromArgb("#4CAF50")
        };
        _stressAfterSlider.ValueChanged += (s, e) => afterLabel.Text = e.NewValue.ToString("F0");
        InputArea.Children.Add(_stressAfterSlider);
        InputArea.Children.Add(afterLabel);

        AddSectionHeader("ОПИШИТЕ ВАШ ОПЫТ");
        _reportEditor = new Editor
        {
            Placeholder = "Какая техника помогла? Что вы чувствовали? Как изменилось состояние?",
            BackgroundColor = Color.FromArgb("#F0F4F8"),
            TextColor = Color.FromArgb("#2C3E50"),
            HeightRequest = 100,
            Margin = new Thickness(0, 5, 0, 15)
        };
        InputArea.Children.Add(_reportEditor);
    }

    private void CreateEmotionRegulationUI(EmotionRegulation exercise)
    {
        var situations = exercise.GetSituations();

        AddSectionHeader("ВЫБЕРИТЕ СИТУАЦИЮ");

        var situationPicker = new Picker
        {
            Title = "Ситуация",
            BackgroundColor = Color.FromArgb("#F0F4F8"),
            TextColor = Color.FromArgb("#2C3E50"),
            Margin = new Thickness(0, 5, 0, 10),
            HeightRequest = 45
        };
        foreach (var s in situations)
            situationPicker.Items.Add(s.Title);

        var situationCard = new Border
        {
            StrokeThickness = 1,
            Stroke = Color.FromArgb("#E0E0E0"),
            BackgroundColor = Color.FromArgb("#FFF8E1"),
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(12),
            Margin = new Thickness(0, 5, 0, 10),
            IsVisible = false
        };

        var situationDesc = new Label { TextColor = Color.FromArgb("#424242"), LineBreakMode = LineBreakMode.WordWrap };
        var emotionDesc = new Label
        {
            TextColor = Color.FromArgb("#E65100"),
            FontAttributes = FontAttributes.Bold,
            Margin = new Thickness(0, 5, 0, 0)
        };

        var situationStack = new VerticalStackLayout { Spacing = 8 };
        situationStack.Children.Add(situationDesc);
        situationStack.Children.Add(emotionDesc);
        situationCard.Content = situationStack;

        situationPicker.SelectedIndexChanged += (s, e) =>
        {
            if (situationPicker.SelectedIndex >= 0)
            {
                var sit = situations[situationPicker.SelectedIndex];
                situationDesc.Text = sit.Situation;
                emotionDesc.Text = $"Испытываемая эмоция: {sit.Emotion}";
                situationCard.IsVisible = true;
                exercise.SetChosenSituation(sit.Title, sit.Situation);
            }
        };
        InputArea.Children.Add(situationPicker);
        InputArea.Children.Add(situationCard);

        AddSectionHeader("ВОЗМОЖНЫЕ РЕАКЦИИ");
        _reactionsEditor = new Editor
        {
            Placeholder = "Перечислите все реакции, которые приходят в голову",
            BackgroundColor = Color.FromArgb("#F0F4F8"),
            TextColor = Color.FromArgb("#2C3E50"),
            HeightRequest = 80,
            Margin = new Thickness(0, 5, 0, 10)
        };
        InputArea.Children.Add(_reactionsEditor);

        AddSectionHeader("КОНСТРУКТИВНАЯ РЕАКЦИЯ");
        _bestActionEntry = new Entry
        {
            Placeholder = "Какая реакция будет самой конструктивной?",
            BackgroundColor = Color.FromArgb("#F0F4F8"),
            TextColor = Color.FromArgb("#2C3E50"),
            Margin = new Thickness(0, 5, 0, 10),
            HeightRequest = 45
        };
        InputArea.Children.Add(_bestActionEntry);

        AddSectionHeader("РЕФЛЕКСИЯ");
        _reflectionRegulationEditor = new Editor
        {
            Placeholder = "Почему эта реакция лучше других? Что вы узнали о себе?",
            BackgroundColor = Color.FromArgb("#F0F4F8"),
            TextColor = Color.FromArgb("#2C3E50"),
            HeightRequest = 80,
            Margin = new Thickness(0, 5, 0, 15)
        };
        InputArea.Children.Add(_reflectionRegulationEditor);
    }

    private void CreateDefaultUI()
    {
        AddSectionHeader("ВЫПОЛНЕНИЕ УПРАЖНЕНИЯ");

        InputArea.Children.Add(new Label
        {
            Text = _exercise.Instruction,
            TextColor = Color.FromArgb("#2C3E50"),
            FontSize = 14,
            Margin = new Thickness(0, 5, 0, 15)
        });

        _answerEditor = new Editor
        {
            Placeholder = "Опишите ваш опыт выполнения упражнения здесь",
            BackgroundColor = Color.FromArgb("#F0F4F8"),
            TextColor = Color.FromArgb("#2C3E50"),
            HeightRequest = 150,
            Margin = new Thickness(0, 5, 0, 15)
        };
        InputArea.Children.Add(_answerEditor);
    }

    private void AddSectionHeader(string title, int topMargin = 0)
    {
        var headerBorder = new Border
        {
            BackgroundColor = Color.FromArgb("#4A90E2"),
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Padding = new Thickness(12, 6),
            Margin = new Thickness(0, topMargin, 0, 10)
        };

        headerBorder.Content = new Label
        {
            Text = title,
            TextColor = Colors.White,
            FontSize = 13,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        };

        InputArea.Children.Add(headerBorder);
    }

    private async void OnCompleteClicked(object sender, EventArgs e)
    {
        try
        {
            CompleteButton.IsEnabled = false;
            CompleteButton.Text = "Обработка...";

            Debug.WriteLine("Нажата кнопка завершения");

            if (_exercise is EmotionWheel wheel)
            {
                wheel.SetUserResponse(
                    _emotionPicker?.SelectedItem?.ToString() ?? "",
                    _subEmotionPicker?.SelectedItem?.ToString() ?? "",
                    _bodyEntry?.Text ?? "",
                    _situationEditor?.Text ?? "");
                Debug.WriteLine("Сохранены ответы для EmotionWheel");
            }
            else if (_exercise is EmpathyExercise empathy)
            {
                empathy.SetUserReflection(_answerEditor?.Text ?? "");
                Debug.WriteLine("Сохранены ответы для EmpathyExercise");
            }
            else if (_exercise is StressManagement stress)
            {
                if (_stressBeforeSlider != null && _stressAfterSlider != null)
                {
                    stress.SetStressLevels((int)_stressBeforeSlider.Value, (int)_stressAfterSlider.Value);
                }
                stress.SetExperienceReport(_reportEditor?.Text ?? "");
                Debug.WriteLine("Сохранены ответы для StressManagement");
            }
            else if (_exercise is EmotionRegulation regulation)
            {
                var possibleActions = _reactionsEditor?.Text?.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
                regulation.SetPossibleActions(possibleActions);
                regulation.SetBestAction(_bestActionEntry?.Text ?? "");
                regulation.SetReflection(_reflectionRegulationEditor?.Text ?? "");
                Debug.WriteLine("Сохранены ответы для EmotionRegulation");
            }
            else
            {
                if (_answerEditor != null && _exercise != null)
                {
                    var method = _exercise.GetType().GetMethod("SetUserReflection");
                    if (method != null)
                    {
                        method.Invoke(_exercise, new object[] { _answerEditor.Text ?? "" });
                        Debug.WriteLine("Сохранены ответы для DefaultUI");
                    }
                }
            }

            Debug.WriteLine("Запускаем StartAsync");
            var result = await _exercise.StartAsync();
            Debug.WriteLine($"StartAsync завершён, результат: {result.Score}");

            var journalEntry = new JournalEntry(_user.Id)
            {
                Mood = result.Score >= 70 ? MoodType.Good : MoodType.Neutral,
                MoodRating = result.Score / 10,
                EmotionWords = $"Упражнение: {result.ExerciseTitle}",
                Situation = $"Результат: {result.Score} из {result.MaxScore} баллов",
                Reflection = result.Feedback + "\n\n" + string.Join("\n", result.KeyInsights)
            };
            _journal.AddEntry(journalEntry);

            await DisplayAlertAsync(
                $"РЕЗУЛЬТАТ: {result.Score} / {result.MaxScore}",
                result.Feedback + "\n\nОСНОВНЫЕ ВЫВОДЫ:\n" + string.Join("\n", result.KeyInsights),
                "Отлично");

            Debug.WriteLine("Алерт показан, возвращаемся назад");

            _onComplete?.Invoke();
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Ошибка при завершении упражнения: {ex.Message}");
            Debug.WriteLine($"Стек: {ex.StackTrace}");
            await DisplayAlertAsync("Ошибка", $"Не удалось завершить упражнение: {ex.Message}", "OK");

            CompleteButton.IsEnabled = true;
            CompleteButton.Text = "Завершить упражнение";
        }
    }
}