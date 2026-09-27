using EQApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQApp.Models;
public class EmpathyExercise : BaseExercise
{
    private List<Scenario> _scenarios;
    private string _userReflection = "";

    public EmpathyExercise()
    {
        Title = "Эмпатическое слушание";
        Description = "Учимся понимать чувства других людей";
        Instruction = "Прочитайте ситуацию и представьте себя на месте человека. " +
                      "Опишите, что он может чувствовать, и что бы вы сказали, чтобы поддержать его.";
        DurationMinutes = 10;
        DifficultyLevel = 2;
        Category = ExerciseCategory.Empathy;

        LoadScenarios();
    }

    private void LoadScenarios()
    {
        _scenarios = new List<Scenario>
        {
            new Scenario
            {
                Title = "Уставший коллега",
                Situation = "Ваш коллега работает сверхурочно уже третью неделю. " +
                           "Он выглядит уставшим и раздражённым. Сегодня он допустил ошибку в отчёте " +
                           "и очень расстроен.",
                Questions = new List<string>
                {
                    "Какие эмоции испытывает коллега?",
                    "Что бы вы сказали, чтобы его поддержать?",
                    "Как бы вы предложили решить проблему с переработками?"
                }
            },
            new Scenario
            {
                Title = "Друг в горе",
                Situation = "Ваш близкий друг потерял любимого человека. " +
                           "Он не хочет ни с кем говорить и закрылся в себе.",
                Questions = new List<string>
                {
                    "Что чувствует ваш друг?",
                    "Как лучше проявить поддержку в этой ситуации?",
                    "Что НЕ стоит говорить человеку в горе?"
                }
            },
            new Scenario
            {
                Title = "Ребёнок, который боится",
                Situation = "Ребёнок боится идти в школу после того, " +
                           "как его высмеяли одноклассники.",
                Questions = new List<string>
                {
                    "Что чувствует ребёнок?",
                    "Как родитель может помочь ему справиться со страхом?",
                    "Какие слова поддержки нужны ребёнку?"
                }
            },
            new Scenario
            {
                Title = "Конфликт с партнёром",
                Situation = "Вы с партнёром поссорились из-за забытого важного события. " +
                           "Он/она обижена и не разговаривает с вами.",
                Questions = new List<string>
                {
                    "Какие эмоции испытывает партнёр?",
                    "Как лучше начать разговор для примирения?",
                    "Что важно признать первым делом?"
                }
            }
        };
    }

    public List<Scenario> GetScenarios() => _scenarios;
    public Scenario GetRandomScenario() => _scenarios[new Random().Next(_scenarios.Count)];

    public void SetUserReflection(string reflection)
    {
        _userReflection = reflection;
        _userResponse = reflection;

        // Оценка за глубину эмпатии
        int score = 0;

        // Проверяем наличие ключевых слов эмпатии
        var empathyWords = new[] { "чувствует", "переживает", "понимаю", "поддержать", "выслушать",
                                   "сочувствую", "сопереживаю", "обижена", "расстроен", "нуждается" };

        foreach (var word in empathyWords)
        {
            if (reflection.Contains(word, StringComparison.OrdinalIgnoreCase))
                score += 8;
        }

        // За глубину ответа
        if (reflection.Length > 100) score += 20;
        if (reflection.Length > 200) score += 20;

        SetScore(Math.Min(score, 100), 100);
    }

    public override Task<ExerciseResult> StartAsync()
    {
        return Task.FromResult(CreateResult());
    }

    public override string GetReflectionPrompt()
    {
        return "Какие чувства вы испытывали, когда представляли себя на месте другого человека? " +
               "Легко ли вам было поставить себя на его место?";
    }

    protected override string GenerateFeedback()
    {
        if (_score >= 80)
            return "У вас отличная эмпатия! Вы хорошо понимаете чувства других и можете оказать поддержку. " +
                   "Исследования показывают, что эмпатия - один из ключевых факторов успешных отношений.";

        if (_score >= 50)
            return "Хороший уровень эмпатии! Продолжайте практиковать активное слушание. " +
                   "Попробуйте задавать открытые вопросы: 'Что ты чувствуешь?', 'Как я могу тебя поддержать?'";

        return "Эмпатию можно развивать! Начните с простого: старайтесь замечать эмоции окружающих. " +
               "Задавайте себе вопрос: 'Что этот человек сейчас чувствует?' каждый раз, когда общаетесь с кем-то.";
    }

    protected override List<string> GetKeyInsights()
    {
        return new List<string>
        {
            "Эмпатия - это не жалость, а понимание чувств другого",
            "Вместо советов часто нужно просто выслушать",
            "Фраза 'Я понимаю, что ты чувствуешь' может быть очень поддерживающей",
            "Эмпатия улучшает отношения и помогает решать конфликты"
        };
    }

    public class Scenario
    {
        public string Title { get; set; } = "";
        public string Situation { get; set; } = "";
        public List<string> Questions { get; set; } = new();
    }
}