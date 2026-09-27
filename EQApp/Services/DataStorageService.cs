using System.Text.Json;
using EQApp.Models;

namespace EQApp.Services;

public class DataStorageService
{
    private readonly string _userFilePath;
    private readonly string _journalFilePath;
    private readonly string _exercisesFilePath;

    public DataStorageService()
    {
        // Папка для данных приложения
        string appDataPath = FileSystem.AppDataDirectory;
        _userFilePath = Path.Combine(appDataPath, "user.json");
        _journalFilePath = Path.Combine(appDataPath, "journal.json");
        _exercisesFilePath = Path.Combine(appDataPath, "exercises.json");
    }

    // Сохранить пользователя
    public async Task SaveUserAsync(User user)
    {
        try
        {
            var json = JsonSerializer.Serialize(user, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_userFilePath, json);
            Console.WriteLine($"[СОХРАНЕНИЕ] Пользователь сохранён: {user.Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ОШИБКА] Не удалось сохранить пользователя: {ex.Message}");
        }
    }

    // Загрузить пользователя
    public async Task<User?> LoadUserAsync()
    {
        try
        {
            if (File.Exists(_userFilePath))
            {
                var json = await File.ReadAllTextAsync(_userFilePath);
                var user = JsonSerializer.Deserialize<User>(json);
                Console.WriteLine($"[ЗАГРУЗКА] Пользователь загружен: {user?.Name}");
                return user;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ОШИБКА] Не удалось загрузить пользователя: {ex.Message}");
        }
        return null;
    }

    // Сохранить дневник
    public async Task SaveJournalAsync(Journal journal)
    {
        try
        {
            var entries = journal.GetAllEntries();
            var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_journalFilePath, json);
            Console.WriteLine($"[СОХРАНЕНИЕ] Дневник сохранён, записей: {entries.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ОШИБКА] Не удалось сохранить дневник: {ex.Message}");
        }
    }

    // Загрузить дневник
    public async Task<List<JournalEntry>?> LoadJournalAsync()
    {
        try
        {
            if (File.Exists(_journalFilePath))
            {
                var json = await File.ReadAllTextAsync(_journalFilePath);
                var entries = JsonSerializer.Deserialize<List<JournalEntry>>(json);
                Console.WriteLine($"[ЗАГРУЗКА] Дневник загружен, записей: {entries?.Count ?? 0}");
                return entries;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ОШИБКА] Не удалось загрузить дневник: {ex.Message}");
        }
        return null;
    }

    // Сохранить выполненные упражнения
    public async Task SaveCompletedExercisesAsync(List<CompletedExercise> exercises)
    {
        try
        {
            var json = JsonSerializer.Serialize(exercises, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_exercisesFilePath, json);
            Console.WriteLine($"[СОХРАНЕНИЕ] Упражнения сохранены, количество: {exercises.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ОШИБКА] Не удалось сохранить упражнения: {ex.Message}");
        }
    }

    // Загрузить выполненные упражнения
    public async Task<List<CompletedExercise>?> LoadCompletedExercisesAsync()
    {
        try
        {
            if (File.Exists(_exercisesFilePath))
            {
                var json = await File.ReadAllTextAsync(_exercisesFilePath);
                var exercises = JsonSerializer.Deserialize<List<CompletedExercise>>(json);
                Console.WriteLine($"[ЗАГРУЗКА] Упражнения загружены, количество: {exercises?.Count ?? 0}");
                return exercises;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ОШИБКА] Не удалось загрузить упражнения: {ex.Message}");
        }
        return null;
    }

    // Проверить, есть ли сохранённые данные
    public bool HasSavedData()
    {
        return File.Exists(_userFilePath) && File.Exists(_journalFilePath);
    }

    // Удалить все данные (сброс приложения)
    public void ClearAllData()
    {
        try
        {
            if (File.Exists(_userFilePath)) File.Delete(_userFilePath);
            if (File.Exists(_journalFilePath)) File.Delete(_journalFilePath);
            if (File.Exists(_exercisesFilePath)) File.Delete(_exercisesFilePath);
            Console.WriteLine("[СБРОС] Все данные удалены");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ОШИБКА] Не удалось удалить данные: {ex.Message}");
        }
    }
}