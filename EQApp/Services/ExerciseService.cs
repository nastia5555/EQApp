using EQApp.Delegates;
using EQApp.Interfaces;  
using EQApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQApp.Services;

public class ExerciseService
{
    // Событие
    public event ExerciseCompletedHandler? ExerciseCompleted;

    // Делегат для фильтрации упражнений
    public FilterDelegate<BaseExercise>? ExerciseFilter { get; set; }

    private List<BaseExercise> _exercises;

    public ExerciseService()
    {
        _exercises = new List<BaseExercise>
        {
            new EmotionWheel(),
            new EmpathyExercise(),
            new StressManagement(),
            new EmotionRegulation()
        };
    }

    public List<BaseExercise> GetAllExercises()
    {
        return _exercises.ToList();
    }

    public List<BaseExercise> GetExercisesByCategory(ExerciseCategory category)
    {
        return _exercises.Where(e => e.Category == category).ToList();
    }

    public BaseExercise? GetExerciseById(string id)
    {
        return _exercises.FirstOrDefault(e => e.Id == id);
    }

    public List<BaseExercise> GetFilteredExercises()
    {
        if (ExerciseFilter == null)
            return _exercises;

        return _exercises.Where(e => ExerciseFilter(e)).ToList();
    }

    public List<BaseExercise> GetExercisesByDifficulty(int maxDifficulty)
    {
        return _exercises.Where(e => e.DifficultyLevel <= maxDifficulty).ToList();
    }

    public async Task RunExerciseAsync(BaseExercise exercise, Action<int> onProgress)
    {
        for (int i = 1; i <= 10; i++)
        {
            await Task.Delay(200);
            onProgress(i * 10);
        }

        // StartAsync возвращает ExerciseResult из Interfaces
        var result = await exercise.StartAsync();

        ExerciseCompleted?.Invoke(result);
    }

    public ExerciseStatistics GetStatistics()
    {
        return new ExerciseStatistics
        {
            TotalExercises = _exercises.Count,
            Categories = _exercises.GroupBy(e => e.Category)
                                   .ToDictionary(g => g.Key, g => g.Count()),
            AverageDifficulty = _exercises.Average(e => e.DifficultyLevel),
            TotalDuration = _exercises.Sum(e => e.DurationMinutes)
        };
    }
}

public class ExerciseStatistics
{
    public int TotalExercises { get; set; }
    public Dictionary<ExerciseCategory, int> Categories { get; set; } = new();
    public double AverageDifficulty { get; set; }
    public int TotalDuration { get; set; }
}