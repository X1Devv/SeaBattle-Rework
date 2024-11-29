public abstract class Weapon
{
    public abstract bool Fire(Field targetField, string input);
}

public class Bomb : Weapon
{
    public override bool Fire(Field targetField, string input)
    {
        int row = input[0] - 'a';
        int col = int.Parse(input.Substring(1)) - 1;

        bool ShipHit;
        bool validShot = targetField.LogicHit(row, col, out ShipHit);

        if (!validShot)
            return false;
        
        return ShipHit;
    }
}

