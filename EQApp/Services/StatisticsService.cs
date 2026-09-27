// Services/StatisticsService.cs - статические методы
namespace EQApp.Services;

public static class StatisticsService
{
    private static int _totalSessions = 0;
    private static int _totalScore = 0;

    // Статическое свойство
    public static double AverageScore => _totalSessions > 0 ? (double)_totalScore / _totalSessions : 0;

    // Статический конструктор - вызывается 1 раз при первом использовании
    static StatisticsService()
    {
        Console.WriteLine("Статистика инициализирована");
    }

    // Статический метод
    public static void RegisterResult(int score)
    {
        _totalSessions++;
        _totalScore += score;
    }

    public static string GetGlobalStats()
    {
        return $"Всего сессий: {_totalSessions}, Средний балл: {AverageScore:F0}";
    }
}