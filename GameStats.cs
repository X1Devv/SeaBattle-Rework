public class GameStats
{
    public int Player1Wins { get; set; }
    public int Player2Wins { get; set; }
    public PlayerStats Player1Stats { get; set; } = new PlayerStats();
    public PlayerStats Player2Stats { get; set; } = new PlayerStats();
}

public class PlayerStats
{
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Rating { get; set; }
}
