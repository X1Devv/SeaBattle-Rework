public class Player(bool isAI, int shipCount)
{
    public Field PlayerField { get; set; } = new Field();//візуалка зроблиа більш скорочений варіант запису
    public string LastInput { get; set; }
    public bool IsAI { get; set; } = isAI;
    public int ShipCount { get; set; } = shipCount;
}
