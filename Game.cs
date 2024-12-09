public class Game
{
    private Player _player1;
    private Player _player2;
    private Player _currentPlayer;
    private Player _opponent;
    private RenderField _render;
    private int _maxWinsToFinish;
    private int _player1Wins;
    private int _player2Wins;

    public Game(Player player1, Player player2, RenderField render, int maxWinsToFinish)
    {
        _player1 = player1;
        _player2 = player2;
        _render = render;
        _maxWinsToFinish = maxWinsToFinish;

        _currentPlayer = _player1;
        _opponent = _player2;
    }

    public void Start()
    {
        while (_player1Wins < _maxWinsToFinish && _player2Wins < _maxWinsToFinish)
        {
            PlayRound();
        }

        Console.WriteLine(_player1Wins == _maxWinsToFinish ? "Player 1 wins the game!" : "Player 2 wins the game!");
    }

    private void PlayRound()
    {
        ResetGame();
        GameCycle();

        if (Winner(_player1))
            _player1Wins++;
        else
            _player2Wins++;

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
            var (row, col) = ProcessInput();
            Logic(row, col);
            Render();
        }

        DisplayWinner();
    }

    private (int Row, int Col) ProcessInput()
    {
        if (_currentPlayer.IsAI)
        {
            return GenerateRandomInput();
        }

        Console.WriteLine("Enter coordinates (a-j 1-10):");
        string? input;
        do
        {
            input = Console.ReadLine()?.Trim();
        } while (!ValidCoordinate(input));

        return ParseInput(input!);
    }

    private (int Row, int Col) GenerateRandomInput()
    {
        Random random = new Random();
        int row = random.Next(0, Field.Height);
        int col = random.Next(0, Field.Width);
        return (row, col);
    }

    private void Logic(int row, int col)
    {
        if (!_opponent.PlayerField.IsCellAttackable(row, col))
            return;

        bool hit = _opponent.PlayerField.ProcessAttack(row, col);
        if (!hit)
            SwitchPlayers();
    }

    private void Render()
    {
        Console.Clear();
        Console.WriteLine($"Player 1 Wins: {_player1Wins}, Player 2 Wins: {_player2Wins}");
        Console.WriteLine($"Current Player: {(_currentPlayer == _player1 ? "Player 1" : "Player 2")}");
        _render.Render(_currentPlayer.PlayerField, false);
        _render.Render(_opponent.PlayerField, true);
    }

    private void DisplayWinner()
    {
        Console.WriteLine($"{(_currentPlayer == _player1 ? "Player 1" : "Player 2")} win");
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

    private bool ValidCoordinate(string? input)
    {
        if (string.IsNullOrEmpty(input) || input.Length < 3 || input.Length > 4)
            return false;

        char row = input[0];
        if (row < 'a' || row >= 'a' + Field.Height)
            return false;

        if (!int.TryParse(input.Substring(2), out int col) || col < 1 || col > Field.Width)
            return false;

        return true;
    }

}
