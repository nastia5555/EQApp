using EQApp.Models;
using EQApp.Data;

namespace EQApp.Views;

public partial class EQTestPage : ContentPage
{
    private User _user;
    private List<EQQuestion> _questions;
    private List<EQAnswer> _answers;
    private int _currentIndex;
    private Action _onComplete;

    private Dictionary<int, RadioButton> _selectedRadioButtons = new();

    public EQTestPage(User user, Action onComplete)
    {
        InitializeComponent();
        _user = user;
        _onComplete = onComplete;
        _questions = EQTestData.GetQuestions();
        _answers = new List<EQAnswer>();
        _currentIndex = 0;

        LoadQuestion();
    }

    private void LoadQuestion()
    {
        var question = _questions[_currentIndex];
        QuestionLabel.Text = $"{question.Id}. {question.Text}";
        ProgressLabel.Text = $"Вопрос {_currentIndex + 1} из {_questions.Count}";

        PrevButton.IsVisible = _currentIndex > 0;
        NextButton.IsVisible = _currentIndex < _questions.Count - 1;
        FinishButton.IsVisible = _currentIndex == _questions.Count - 1;

        OptionsStack.Children.Clear();

        // Находим сохранённый ответ для этого вопроса
        var existingAnswer = _answers.FirstOrDefault(a => a.QuestionId == question.Id);
        int? savedScore = existingAnswer?.Score;

        for (int i = 0; i < question.Options.Count; i++)
        {
            var option = question.Options[i];

            var radioButton = new RadioButton
            {
                Content = option.Text,
                Value = option.Score,
                StyleId = i.ToString(),
                FontSize = 14,
                Margin = new Thickness(0, 5),
                TextColor = Color.FromArgb("#2C3E50")
            };

            // Если есть сохранённый ответ, отмечаем соответствующий RadioButton
            if (savedScore.HasValue && option.Score == savedScore.Value)
            {
                radioButton.IsChecked = true;
                _selectedRadioButtons[question.Id] = radioButton;
            }

            int questionId = question.Id;
            string answerText = option.Text;
            int score = option.Score;

            radioButton.CheckedChanged += (s, e) =>
            {
                if (e.Value)
                {
                    // Удаляем старый ответ для этого вопроса
                    var oldAnswer = _answers.FirstOrDefault(a => a.QuestionId == questionId);
                    if (oldAnswer != null)
                    {
                        _answers.Remove(oldAnswer);
                    }

                    // Добавляем новый ответ
                    OnOptionSelected(questionId, answerText, score);

                    // Сохраняем выбранный RadioButton
                    if (_selectedRadioButtons.ContainsKey(questionId))
                    {
                        _selectedRadioButtons[questionId] = (RadioButton)s;
                    }
                    else
                    {
                        _selectedRadioButtons.Add(questionId, (RadioButton)s);
                    }
                }
            };

            OptionsStack.Children.Add(radioButton);
        }

        UpdateScoreDisplay();
    }

    private void OnOptionSelected(int questionId, string answerText, int score)
    {
        var question = _questions.First(q => q.Id == questionId);

        _answers.Add(new EQAnswer
        {
            QuestionId = questionId,
            QuestionText = question.Text,
            AnswerText = answerText,
            Score = score
        });

        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        int totalScore = _answers.Sum(a => a.Score);
        ScoreLabel.Text = $"Баллов: {totalScore}";
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        var currentQuestion = _questions[_currentIndex];
        var hasAnswer = _answers.Any(a => a.QuestionId == currentQuestion.Id);

        if (!hasAnswer)
        {
            await DisplayAlertAsync("Внимание", "Пожалуйста, выберите ответ", "OK");
            return;
        }

        if (_currentIndex < _questions.Count - 1)
        {
            _currentIndex++;
            LoadQuestion();
        }
    }

    private void OnPrevClicked(object sender, EventArgs e)
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
            LoadQuestion();
        }
    }

    private async void OnFinishClicked(object sender, EventArgs e)
    {
        var currentQuestion = _questions[_currentIndex];
        var hasAnswer = _answers.Any(a => a.QuestionId == currentQuestion.Id);

        if (!hasAnswer)
        {
            await DisplayAlertAsync("Внимание", "Пожалуйста, выберите ответ на последний вопрос", "OK");
            return;
        }

        var test = new EQTest(_user.Id);
        test.Answers = _answers;
        test.CalculateResult();

        _user.AddTestResult(test.TotalScore);

        string resultIcon = test.TotalScore >= 80 ? "" : (test.TotalScore >= 60 ? "" : "");

        await DisplayAlertAsync(
            $"{resultIcon} Ваш результат: {test.TotalScore} из 100",
            $"Уровень: {test.ResultLevel}\n\nРекомендации:\n{test.Recommendations}",
            "Отлично");

        _onComplete?.Invoke();
        await Navigation.PopAsync();
    }
}