namespace EQApp.Models;

public class User
{
    private static int _nextId = 1000;

    public string Id { get; private set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int TotalEQScore { get; set; }
    public int TestsCompleted { get; set; }

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
}