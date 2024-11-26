public class Game
{
    const int ShipCount = 8;
    public Field Player1Field { get; set; }
    public Field Player2Field { get; set; } 

    public bool YourStep { get; set; } = true;

    public Game()
    {
        Player1Field = new Field();
        Player2Field = new Field();
    }

    public void InitializeGame()
    {
        Player1Field.PlaceRandomShips(ShipCount);
        Player2Field.PlaceRandomShips(ShipCount);
    }

    public bool CheckEndGame()
    {
        if (Player2Field.AllShipsKilled())
        {
            Console.WriteLine("Player 1 won!");
            return true;
        }

        if (Player1Field.AllShipsKilled())
        {
            Console.WriteLine("Player 2 won!");
            return true;
        }

        return false;
    }

    public void ControlPlayerTurn(string input, int playerNumber)
    {
        int Row = input[0] - 'a';
        int Col = int.Parse(input.Substring(1)) - 1;

        if (playerNumber == 1)
        {
            if (Player2Field.LogicHit(Row, Col, out bool Hit))
            {
                if (!Hit)
                {
                    YourStep = false;
                }
            }
        }
        else if (playerNumber == 2)
        {
            if (Player1Field.LogicHit(Row, Col, out bool Hit))
            {
                if (!Hit)
                {
                    YourStep = true;
                }
            }
        }
    }
}
