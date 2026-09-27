using EQApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EQApp.Models;
public abstract class BaseExercise : IExercise
{
    public string Id { get; protected set; }
    public string Title { get; protected set; } = "";
    public string Description { get; protected set; } = "";
    public string Instruction { get; protected set; } = "";
    public int DurationMinutes { get; protected set; }
    public int DifficultyLevel { get; protected set; }
    public ExerciseCategory Category { get; protected set; }

    protected string _userResponse = "";
    protected int _score;

    public BaseExercise()
    {
        Id = Guid.NewGuid().ToString();
    }

    public abstract Task<ExerciseResult> StartAsync();
    public abstract string GetReflectionPrompt();

    protected void SetScore(int score, int maxScore)
    {
        _score = Math.Min(score, maxScore);
    }

    protected ExerciseResult CreateResult()
    {
        return new ExerciseResult
        {
            ExerciseId = Id,
            ExerciseTitle = Title,
            Score = _score,
            MaxScore = 100,
            CompletedAt = DateTime.Now,
            UserAnswer = _userResponse,
            Feedback = GenerateFeedback(),
            KeyInsights = GetKeyInsights()
        };
    }

    protected abstract string GenerateFeedback();
    protected abstract List<string> GetKeyInsights();
}