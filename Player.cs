public class Player
{
    public Field PlayerField { get; set; } = new Field();
    public int ShipCount { get; set; }
    public bool IsAI { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int Rating { get; set; }

    public Player(bool isAI, int shipCount)
    {
        IsAI = isAI;
        ShipCount = shipCount;
        Wins = 0;
        Losses = 0;
        Rating = 1000;
    }

    public void RecordWin()
    {
        Wins++;
        Rating += 20;
    }

    public void RecordLoss()
    {
        Losses++;
        Rating -= 15;
    }
}