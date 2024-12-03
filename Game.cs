public class Game
{
    Player _player1;
    Player _player2;
    Player _currentPlayer;
    Player _opponent;
    RenderField _render;
    private int _maxWinsToFinish;
    private int _player1Wins;
    private int _player2Wins;

    public Game(Player player1, Player player2, RenderField render, int MaxWinsToFinish)
    {
        _player1 = player1;
        _player2 = player2;
        _render = render;
        _maxWinsToFinish = MaxWinsToFinish;
        _currentPlayer = _player1;
        _opponent = _player2;//змінив назву минула була _enemy
    }

    public void Start()
    {
        while (_player1Wins < _maxWinsToFinish && _player2Wins < _maxWinsToFinish)
        {
            ResetGame();
            GameCycle();

            if (Winner(_player1))
                _player1Wins++;
            else
                _player2Wins++;

            Console.WriteLine($"Wins Player1 > {_player1Wins} Wins Player2 > {_player2Wins}");
        }

        Console.WriteLine(_player1Wins == _maxWinsToFinish ? "Player 1 end this game" : "Player 1 end this game");
    }

    private void ResetGame()
    {
        _player1.PlayerField.InitializeField();
        _player2.PlayerField.InitializeField();
        _player1.PlayerField.PlaceRandomShips(_player1.ShipCount);
        _player2.PlayerField.PlaceRandomShips(_player2.ShipCount);
        _currentPlayer = _player1;
        _opponent = _player2;
    }

    private void GameCycle()
    {
        while (!GameOver())
        {
            ProcessInput();
            Logic();
            Render();
        }

        DisplayRoundWinner();
    }

    private void ProcessInput()
    {
        if (_currentPlayer.IsAI)
        {
            _currentPlayer.LastInput = GenerateRandomInput();
            Thread.Sleep(100);
            return;
        }

        string? input;
        do
        {
            Console.WriteLine("Enter coordinates (a-j 1-10):");
            input = Console.ReadLine()?.Trim();
        } while (!ValidCoordinate(input));

        _currentPlayer.LastInput = input;
    }
    private string GenerateRandomInput()
    {
        Random random = new Random();
        char row = (char)('a' + random.Next(0, Field.Height));
        int col = random.Next(1, Field.Width + 1);
        return $"{row}{col}";
    }

    private void Logic()
    {
        var (row, col) = ParseInput(_currentPlayer.LastInput);

        if (!_opponent.PlayerField.IsCellAttackable(row, col))
            return;

        bool hit = _opponent.PlayerField.ProcessAttack(row, col);

        if (!hit)
            SwitchPlayers();
    }

    private void Render()
    {
        Console.Clear();
        Console.WriteLine($"Player 1 Wins: {_player1Wins} | Player 2 Wins: {_player2Wins}");
        Console.WriteLine($"Current Player: {(_currentPlayer == _player1 ? "Player 1" : "Player 2")}");
        _render.Render(_currentPlayer.PlayerField, false);
        Console.WriteLine();
        _render.Render(_opponent.PlayerField, true);
    }

    private void DisplayRoundWinner()
    {
        Console.WriteLine($"Winner of this round: {(_currentPlayer == _player1 ? "Player 1" : "Player 2")}");
        Thread.Sleep(3000);
    }

    private void SwitchPlayers() => (_currentPlayer, _opponent) = (_opponent, _currentPlayer);

    private bool GameOver() => Winner(_player1) || Winner(_player2);

    private bool Winner(Player player) => player.PlayerField.AllShipsDestroyed();

    private (int Row, int Col) ParseInput(string input)
    {
        int row = input[0] - 'a';
        int col = int.Parse(input.Substring(1)) - 1;
        return (row, col);
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
