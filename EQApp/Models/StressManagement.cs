using EQApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQApp.Models;
public class StressManagement : BaseExercise
{
    private List<GroundingTechnique> _techniques;
    private string _experienceReport = "";
    private int _beforeStressLevel;
    private int _afterStressLevel;

    public StressManagement()
    {
        Title = "Техники заземления";
        Description = "Учимся быстро снижать уровень тревоги и стресса";
        Instruction = "Оцените уровень стресса до и после выполнения техник. " +
                      "Выберите одну из техник заземления и выполните её внимательно.";
        DurationMinutes = 8;
        DifficultyLevel = 2;
        Category = ExerciseCategory.StressManagement;

        LoadTechniques();
    }

    private void LoadTechniques()
    {
        _techniques = new List<GroundingTechnique>
        {
            new GroundingTechnique
            {
                Name = "5-4-3-2-1",
                Description = "Техника осознанности через органы чувств",
                Steps = new List<string>
                {
                    "Назовите 5 предметов, которые вы видите вокруг",
                    "Назовите 4 звука, которые вы слышите",
                    "Назовите 3 тактильных ощущения, которые чувствуете",
                    "Назовите 2 запаха, которые ощущаете (или любимых запаха)",
                    "Назовите 1 вкус, который вы чувствуете (или любимый)"
                }
            },
            new GroundingTechnique
            {
                Name = "Квадратное дыхание",
                Description = "Дыхательная техника для успокоения",
                Steps = new List<string>
                {
                    "Вдох на 4 счёта",
                    "Задержка дыхания на 4 счёта",
                    "Выдох на 4 счёта",
                    "Задержка на 4 счёта",
                    "Повторите 5-10 раз"
                }
            },
            new GroundingTechnique
            {
                Name = "Описание предмета",
                Description = "Переключение внимания на детали",
                Steps = new List<string>
                {
                    "Выберите любой предмет в комнате",
                    "Опишите его цвет, форму, текстуру, вес",
                    "Представьте, как бы вы объяснили этот предмет инопланетянину",
                    "Подумайте, из каких материалов он сделан",
                    "Заметьте тень и блики на предмете"
                }
            },
            new GroundingTechnique
            {
                Name = "Прогрессивная релаксация",
                Description = "Поочерёдное напряжение и расслабление мышц",
                Steps = new List<string>
                {
                    "Напрягите и расслабьте мышцы стоп (5 секунд)",
                    "Напрягите и расслабьте мышцы голеней",
                    "Напрягите и расслабьте мышцы бёдер и ягодиц",
                    "Напрягите и расслабьте живот и грудь",
                    "Напрягите и расслабьте руки и плечи",
                    "Напрягите и расслабьте шею и лицо"
                }
            }
        };
    }

    public List<GroundingTechnique> GetTechniques() => _techniques;

    public void SetStressLevels(int before, int after)
    {
        _beforeStressLevel = Math.Clamp(before, 1, 10);
        _afterStressLevel = Math.Clamp(after, 1, 10);
    }

    public void SetExperienceReport(string report)
    {
        _experienceReport = report;
        _userResponse = report;

        // Оценка на основе снижения стресса и рефлексии
        int stressReduction = _beforeStressLevel - _afterStressLevel;
        int score = stressReduction * 15;

        if (!string.IsNullOrWhiteSpace(report) && report.Length > 50)
            score += 25;

        if (report.Contains("помог", StringComparison.OrdinalIgnoreCase))
            score += 10;

        SetScore(Math.Min(score, 100), 100);
    }

    public int GetBeforeStressLevel() => _beforeStressLevel;
    public int GetAfterStressLevel() => _afterStressLevel;

    public override Task<ExerciseResult> StartAsync()
    {
        return Task.FromResult(CreateResult());
    }

    public override string GetReflectionPrompt()
    {
        return "Какая техника подошла вам больше всего? Как вы можете использовать её в повседневной жизни?";
    }

    protected override string GenerateFeedback()
    {
        int reduction = _beforeStressLevel - _afterStressLevel;

        if (reduction >= 3)
            return $"Отличный результат! Уровень стресса снизился с {_beforeStressLevel} до {_afterStressLevel}. " +
                   "Практикуйте эти техники регулярно, чтобы они стали автоматическими.";

        if (reduction >= 1)
            return $"Хороший прогресс! Стресс снизился с {_beforeStressLevel} до {_afterStressLevel}. " +
                   "Попробуйте разные техники, чтобы найти самую эффективную для вас.";

        return $"Уровень стресса не изменился ({_beforeStressLevel} → {_afterStressLevel}). " +
               "Попробуйте другую технику или продлите время практики. Управление стрессом требует практики.";
    }

    protected override List<string> GetKeyInsights()
    {
        return new List<string>
        {
            "Стресс можно снизить за несколько минут с помощью правильных техник",
            "Осознанное дыхание - самый быстрый способ успокоиться",
            "Техники заземления возвращают в настоящий момент и снижают тревогу",
            "Регулярная практика делает техники более эффективными"
        };
    }

    public class GroundingTechnique
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public List<string> Steps { get; set; } = new();
    }
}