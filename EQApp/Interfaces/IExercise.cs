using System;
using System.Collections.Generic;
using System.Text;

namespace EQApp.Interfaces;
public interface IExercise
{
    string Id { get; }
    string Title { get; }
    string Description { get; }
    string Instruction { get; }
    int DurationMinutes { get; }
    int DifficultyLevel { get; }
    ExerciseCategory Category { get; }

    Task<ExerciseResult> StartAsync();
    string GetReflectionPrompt();
}

public enum ExerciseCategory
{
    SelfAwareness,      // Самоосознание - понимание своих эмоций
    SelfRegulation,     // Саморегуляция - управление эмоциями
    Empathy,            // Эмпатия - понимание чужих эмоций
    SocialSkills,       // Социальные навыки - общение
    Motivation,         // Мотивация - позитивное мышление
    StressManagement    // Управление стрессом
}

public class ExerciseResult
{
    public string ExerciseId { get; set; } = "";
    public string ExerciseTitle { get; set; } = "";
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public DateTime CompletedAt { get; set; }
    public string UserAnswer { get; set; } = "";
    public string Feedback { get; set; } = "";
    public List<string> KeyInsights { get; set; } = new();
}