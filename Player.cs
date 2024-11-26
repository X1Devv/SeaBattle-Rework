class Player
{
    Game _game;
    int _playerNumber;

    public Player(Game game, int playerNumber)
    {
        _game = game;
        _playerNumber = playerNumber;
    }

    public void DisplayFields()
    {
        Console.Clear();
        Console.WriteLine($"Player {_playerNumber} field:");
        if (_playerNumber == 1)
        {
            _game.Player1Field.Draw(false);
            Console.WriteLine("Enemy field:");
            _game.Player2Field.Draw(true);
        }
        else
        {
            _game.Player2Field.Draw(false);
            Console.WriteLine("Enemy field:");
            _game.Player1Field.Draw(true);
        }
    }

    public void HandleTurn()
    {
        DisplayFields();

        string input = GetInput();
        _game.ControlPlayerTurn(input, _playerNumber);
    }

    public string GetInput()
    {
        string input;
        while (true)
        {
            Console.WriteLine("Enter coordinates (a-j 1-10):");
            input = Console.ReadLine();

            if (ValidCoordinate(input))
                break;
        }
        return input;
    }

    public bool ValidCoordinate(string input)
    {
        input = input.Replace(" ", "");

        if (input.Length < 2 || input.Length > 3)
            return false;

        char Row = input[0];
        if (Row < 'a' || Row > 'j')
            return false;

        if (!int.TryParse(input.Substring(1), out int Col))
            return false;

        return Col >= 1 && Col <= Field.Width;
    }

}
