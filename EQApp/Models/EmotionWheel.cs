using EQApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQApp.Models;
public class EmotionWheel: BaseExercise
{
    // Словарь базовых эмоций
    private readonly Dictionary<string, List<string>> _emotionWheel = new()
    {
        { "Злость", new List<string> { "Раздражение", "Ярость", "Зависть", "Обида" } },
        { "Печаль", new List<string> { "Грусть", "Тоска", "Разочарование", "Одиночество" } },
        { "Страх", new List<string> { "Тревога", "Паника", "Беспокойство", "Неуверенность" } },
        { "Радость", new List<string> { "Счастье", "Восторг", "Гордость", "Спокойствие" } },
        { "Любовь", new List<string> { "Нежность", "Привязанность", "Забота", "Принятие" } }
    };

    public EmotionWheel()
    {
        Title = "Колесо эмоций";
        Description = "Учимся точно определять и называть свои эмоции";
        Instruction = "Вспомните ситуацию за последние 24 часа, которая вызвала у вас эмоции. " +
                      "Выберите основную эмоцию, а затем уточните её оттенок. Опишите, что вы чувствовали в теле.";
        DurationMinutes = 7;
        DifficultyLevel = 1;
        Category = ExerciseCategory.SelfAwareness;
    }

    public void SetUserResponse(string mainEmotion, string subEmotion, string bodySensation, string situation)
    {
        _userResponse = $"""
            Ситуация: {situation}
            Основная эмоция: {mainEmotion}
            Оттенок эмоции: {subEmotion}
            Ощущения в теле: {bodySensation}
            """;

        // Оценка за полноту и глубину рефлексии
        int score = 0;
        if (!string.IsNullOrWhiteSpace(mainEmotion)) score += 20;
        if (!string.IsNullOrWhiteSpace(subEmotion)) score += 25;
        if (!string.IsNullOrWhiteSpace(bodySensation)) score += 25;
        if (!string.IsNullOrWhiteSpace(situation) && situation.Length > 20) score += 30;

        SetScore(score, 100);
    }

    public Dictionary<string, List<string>> GetEmotionWheel() => _emotionWheel;

    public override Task<ExerciseResult> StartAsync()
    {
        return Task.FromResult(CreateResult());
    }

    public override string GetReflectionPrompt()
    {
        return "Подумайте: Как точное называние эмоции меняет ваше отношение к ситуации?";
    }

    protected override string GenerateFeedback()
    {
        if (_score >= 80)
            return "Отлично! Вы хорошо различаете оттенки эмоций. Это ключевой навык эмоционального интеллекта. " +
                   "Исследования показывают, что люди, которые могут точно назвать свои эмоции, лучше справляются со стрессом.";

        if (_score >= 50)
            return "Хорошее начало! Попробуйте обращать внимание не только на основную эмоцию, но и на её оттенки. " +
                   "Это поможет вам лучше понимать себя.";

        return "Каждая эмоция имеет множество оттенков. Например, гнев может быть раздражением, возмущением или яростью. " +
               "Попробуйте упражнение снова, уделяя больше внимания нюансам.";
    }

    protected override List<string> GetKeyInsights()
    {
        return new List<string>
        {
            "Эмоции можно и нужно называть точно",
            "У каждой базовой эмоции есть множество оттенков",
            "Эмоции сопровождаются физическими ощущениями в теле",
            "Осознание эмоции - первый шаг к управлению ей"
        };
    }
}