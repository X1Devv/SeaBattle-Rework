public class Game
{
    Player _player1;
    Player _player2;
    Player _currentPlayer;
    Player _enemy;

    public bool IsGameOver { get; set; } = false;

    public Game()
    {
        while (true)
        {
            Console.WriteLine("Select mod pvp pve eve:");
            string mod = Console.ReadLine();
            if (mod == "pvp")
            {
                _player1 = new Player();
                _player2 = new Player();
                break;
            }
            else if (mod == "pve")
            {
                _player1 = new Player();
                _player2 = new Player(isAI: true);
                break;
            }
            else if (mod == "eve")
            {
                _player1 = new Player(isAI: true);
                _player2 = new Player(isAI: true);
                break;
            }
            else continue;
        }
        _currentPlayer = _player1;
        _enemy = _player2;
    }

    public void InitializeGame()
    {
        _player1.PlayerField.PlaceRandomShips(8);
        _player2.PlayerField.PlaceRandomShips(8);
    }

    public void Render()
    {
        Console.Clear();
        _currentPlayer.DisplayFields(_enemy.PlayerField);
    }

    public void HandleInput()
    {
        string input = GetTargetInput();
        bool hit = _currentPlayer.CurrentWeapon.Fire(_enemy.PlayerField, input);

        if (!hit)
        {
            SwitchPlayers();
        }
    }

    public void CheckEndGame()
    {
        if (_enemy.PlayerField.AllShipsKilled())
        {
            Console.WriteLine($"Player {(_currentPlayer == _player1 ? 1 : 2)} win!");
            IsGameOver = true;
        }
    }

    private void SwitchPlayers()
    {
        _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
        _enemy = _enemy == _player1 ? _player2 : _player1;
    }

    private string GetTargetInput()
    {
        if (_currentPlayer.AIPlayer)
        {
            Random random = new Random();
            char row = (char)('a' + random.Next(0, Field.Height));
            int col = random.Next(1, Field.Width + 1);
            return $"{row}{col}";
        }

        while (true)
        {
            Console.WriteLine("Choose weapon: 1. Bomb");
            string Choice = Console.ReadLine();
            if (Choice == "1")
            {
                _currentPlayer.CurrentWeapon = new Bomb();
                break;
            }
            else continue;
        }

        while (true)
        {
            Console.WriteLine("Enter coordinates (a-j 1-10):");

            string input = Console.ReadLine();
            if (ValidCoordinate(input))
                return input;
        }
    }

    private bool ValidCoordinate(string input)
    {
        input = input.Replace(" ", "");

        if (input.Length < 2 || input.Length > 3)
            return false;

        char row = input[0];
        if (row < 'a' || row > 'j')
            return false;

        if (!int.TryParse(input.Substring(1), out int col))
            return false;

        return col >= 1 && col <= Field.Width;
    }
}
