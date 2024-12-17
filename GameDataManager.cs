using System.Text.Json;

public class GameDataManager
{
    private string _file;

    public GameDataManager(string file)
    {
        _file = file;
    }

    public void SaveGame(GameStats gameStats)
    {
        var json = JsonSerializer.Serialize(gameStats);
        File.WriteAllText(_file, json);
    }

    public GameStats LoadGame()
    {
        if (File.Exists(_file))
        {
            var json = File.ReadAllText(_file);
            return JsonSerializer.Deserialize<GameStats>(json);
        }
        return new GameStats();
    }
}
