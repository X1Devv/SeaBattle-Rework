public class Player
{
    public Field PlayerField { get; private set; }
    public Weapon CurrentWeapon { get; set; }
    public bool AIPlayer { get; private set; }

    public Player(bool isAI = false)
    {
        PlayerField = new Field();
        CurrentWeapon = new Bomb();
        AIPlayer = isAI;
    }

    public void DisplayFields(Field EnemyField)
    {
        Console.WriteLine("Player Field:");
        PlayerField.Draw(false);
        Console.WriteLine("Enemy Field:");
        EnemyField.Draw(true);
    }
}
