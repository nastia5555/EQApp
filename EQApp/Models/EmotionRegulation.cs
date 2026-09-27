using EQApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
namespace EQApp.Models;

public class EmotionRegulation : BaseExercise
{
    private List<EmotionSituation> _situations;
    private string _chosenSituation = "";
    private List<string> _possibleActions = new();
    private string _bestAction = "";
    private string _reflection = "";

    public EmotionRegulation()
    {
        Title = "Управление эмоциями";
        Description = "Учимся находить конструктивные способы реагирования в эмоциональных ситуациях";
        Instruction = "Выберите ситуацию, которая вызывает у вас сильные эмоции. " +
                      "Перечислите все возможные способы реакции, затем выберите самый конструктивный.";
        DurationMinutes = 12;
        DifficultyLevel = 3;
        Category = ExerciseCategory.SelfRegulation;

        LoadSituations();
    }

    private void LoadSituations()
    {
        _situations = new List<EmotionSituation>
        {
            new EmotionSituation
            {
                Title = "Очередь в магазине",
                Emotion = "Раздражение, нетерпение",
                Situation = "Вы стоите в длинной очереди, а кассир работает очень медленно. " +
                           "Вы опаздываете на важную встречу.",
                CommonReactions = new List<string>
                {
                    "Начать спорить с кассиром",
                    "Громко вздыхать и показывать недовольство",
                    "Уйти без покупок",
                    "Сделать глубокий вдох и принять ситуацию",
                    "Вежливо спросить, можно ли побыстрее"
                }
            },
            new EmotionSituation
            {
                Title = "Критика от начальника",
                Emotion = "Обида, гнев, стыд",
                Situation = "Начальник раскритиковал вашу работу при всех коллегах, " +
                           "хотя вы вложили много усилий.",
                CommonReactions = new List<string>
                {
                    "Защищаться и оправдываться",
                    "Замолчать и уйти",
                    "Расплакаться",
                    "Поблагодарить за обратную связь и попросить обсудить наедине",
                    "Подумать, что из критики можно использовать для улучшения работы"
                }
            },
            new EmotionSituation
            {
                Title = "Предательство друга",
                Emotion = "Боль, гнев, разочарование",
                Situation = "Узнали, что близкий друг рассказывал другим ваши секреты.",
                CommonReactions = new List<string>
                {
                    "Устроить скандал",
                    "Прекратить общение без объяснений",
                    "Распространять слухи о друге",
                    "Спокойно поговорить и объяснить, что вам больно",
                    "Дать себе время остыть, затем обсудить ситуацию"
                }
            }
        };
    }

    public List<EmotionSituation> GetSituations() => _situations;

    public void SetChosenSituation(string title, string situation)
    {
        _chosenSituation = $"{title}: {situation}";
    }

    public void SetPossibleActions(List<string> actions)
    {
        _possibleActions = actions;
    }

    public void SetBestAction(string action)
    {
        _bestAction = action;
    }

    public void SetReflection(string reflection)
    {
        _reflection = reflection;
        _userResponse = $"""
            Ситуация: {_chosenSituation}
            Возможные реакции: {string.Join(", ", _possibleActions)}
            Выбранная реакция: {_bestAction}
            Рефлексия: {_reflection}
            """;

        // Оценка
        int score = 0;

        if (_possibleActions.Count >= 3) score += 20;
        if (!string.IsNullOrWhiteSpace(_bestAction)) score += 30;

        // Проверяем, выбран ли конструктивный вариант
        var constructiveWords = new[] { "поговорить", "обсудить", "спокойно", "вдох", "подумать",
                                        "обратная связь", "принять" };
        if (constructiveWords.Any(w => _bestAction.Contains(w, StringComparison.OrdinalIgnoreCase)))
            score += 30;

        if (_reflection.Length > 50) score += 20;

        SetScore(score, 100);
    }

    public override Task<ExerciseResult> StartAsync()
    {
        return Task.FromResult(CreateResult());
    }

    public override string GetReflectionPrompt()
    {
        return "Что даёт вам пауза между эмоцией и реакцией? Как вы можете использовать это на практике?";
    }

    protected override string GenerateFeedback()
    {
        if (_score >= 80)
            return "Отличная работа! Вы умеете находить конструктивные способы реагирования. " +
                   "Пауза между эмоцией и действием - ключевой навык эмоционального интеллекта.";

        if (_score >= 50)
            return "Хорошо! Обратите внимание на разницу между реактивными (мгновенными) " +
                   "и проактивными (осознанными) реакциями. Пауза в 5 секунд может изменить результат.";

        return "Помните: между стимулом и реакцией есть пауза. В этой паузе - наша свобода выбора. " +
               "Практикуйте 'стоп-сигнал' - когда чувствуете сильную эмоцию, сделайте глубокий вдох " +
               "и спросите себя: 'Какая реакция будет лучшей для меня в долгосрочной перспективе?'";
    }

    protected override List<string> GetKeyInsights()
    {
        return new List<string>
        {
            "У вас всегда есть выбор, как реагировать",
            "Пауза между эмоцией и реакцией - ключ к саморегуляции",
            "Конструктивная реакция учитывает долгосрочные последствия",
            "Эмоции не требуют немедленного действия"
        };
    }

    public class EmotionSituation
    {
        public string Title { get; set; } = "";
        public string Emotion { get; set; } = "";
        public string Situation { get; set; } = "";
        public List<string> CommonReactions { get; set; } = new();
    }
}