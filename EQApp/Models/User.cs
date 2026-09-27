using System.Text.Json.Serialization;
using EQApp.Interfaces;
namespace EQApp.Models;

public class User : INotifiable
{
    private static int _nextId = 1000;

    public string Id { get; private set; }
    public string Name { get; set; } = string.Empty;  
    public string Email { get; set; } = string.Empty; 
    public DateTime RegistrationDate { get; set; }
    public int TotalEQScore { get; set; }
    public int TestsCompleted { get; set; }
    public int ExercisesCompleted { get; set; } = 0;
    public User()
    {
        Id = (_nextId++).ToString();
        RegistrationDate = DateTime.Now;
    }

    public User(string name, string email) : this()
    {
        Name = name;
        Email = email;
    }

    public void AddTestResult(int score)
    {
        TotalEQScore += score;
        TestsCompleted++;
    }

    public double AverageEQ => TestsCompleted > 0 ? (double)TotalEQScore / TestsCompleted : 0;

    public string GetEQLevel()
    {
        double avg = AverageEQ;
        if (avg >= 80) return "Высокий EQ";
        if (avg >= 60) return "Средний EQ";
        if (avg >= 40) return "Ниже среднего";
        return "Начальный уровень";
    }
    public bool IsNotificationEnabled { get; set; } = true;

    public void SendNotification(string message)
    {
        if (IsNotificationEnabled)
        {
            Console.WriteLine($"[Уведомление для {Name}] {message}");
        }
    }
    public void AddCompletedExercise(int score)
    {
        ExercisesCompleted++;
        TotalEQScore += score;  
    }
}