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
    private bool _saveEnabled;
    private GameDataManager _gameDataManager;

    public Game(Player player1, Player player2, RenderField render, int maxWinsToFinish, bool saveEnabled)
    {
        _player1 = player1;
        _player2 = player2;
        _render = render;
        _maxWinsToFinish = maxWinsToFinish;
        _saveEnabled = saveEnabled;

        _currentPlayer = _player1;
        _opponent = _player2;

        _gameDataManager = new GameDataManager(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "game_data.json"));

        LoadStats();
    }

    public void Start()
    {
        while (_player1Wins < _maxWinsToFinish && _player2Wins < _maxWinsToFinish)
        {
            PlayRound();
        }

        if (_saveEnabled)
            SaveGame();
    }

    private void PlayRound()
    {
        ResetGame();
        GameCycle();

        if (Winner(_player1))
        {
            _player1Wins++;
            _player1.RecordWin();
            _player2.RecordLoss();
        }
        else
        {
            _player2Wins++;
            _player2.RecordWin();
            _player1.RecordLoss();
        }

        if (_saveEnabled)
            SaveGame();
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
            Logic(ProcessInput());
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

    private void Logic((int Row, int Col) coordinates)
    {
        var (row, col) = coordinates;

        if (!_opponent.PlayerField.IsCellAttackable(row, col))
            return;

        bool hit = _opponent.PlayerField.ProcessAttack(row, col);
        if (!hit)
            SwitchPlayers();
    }

    private void Render()
    {
        Console.Clear();
        RenderPlayerStats(_player1, _player2);
        Console.WriteLine($"Current Player: {(_currentPlayer == _player1 ? "Player 1" : "Player 2")}");
        _render.Render(_currentPlayer.PlayerField, false);
        _render.Render(_opponent.PlayerField, true);
    }

    private void RenderPlayerStats(Player player1, Player player2)
    {
        double winRate1 = CalculateWinRate(player1);
        Console.WriteLine($"Player 1>Wins: {_player1Wins}, Losses> {player1.Losses}, Rating> {player1.Rating}, Win Rate>{winRate1:F2}%");

        double winRate2 = CalculateWinRate(player2);
        Console.WriteLine($"Player 2>Wins: {_player2Wins}, Losses> {player2.Losses}, Rating> {player2.Rating}, Win Rate> {winRate2:F2}%");

        Console.WriteLine();
    }

    private double CalculateWinRate(Player player)
    {
        int totalGames = player.Wins + player.Losses;
        if (totalGames == 0)
            return 0;
        return (double)player.Wins / totalGames * 100;
    }

    private void DisplayWinner()
    {
        Console.WriteLine($"{(_currentPlayer == _player1 ? "Player 1" : "Player 2")} wins the round");
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

    private void SaveGame()
    {
        var gameStats = new GameStats
        {
            Player1Wins = _player1Wins,
            Player2Wins = _player2Wins,
            Player1Stats = new PlayerStats
            {
                Wins = _player1.Wins,
                Losses = _player1.Losses,
                Rating = _player1.Rating
            },
            Player2Stats = new PlayerStats
            {
                Wins = _player2.Wins,
                Losses = _player2.Losses,
                Rating = _player2.Rating
            }
        };

        _gameDataManager.SaveGame(gameStats);
    }

    public void LoadStats()
    {
        var gameStats = _gameDataManager.LoadGame();

        _player1Wins = gameStats.Player1Wins;
        _player2Wins = gameStats.Player2Wins;

        _player1.Wins = gameStats.Player1Stats.Wins;
        _player1.Losses = gameStats.Player1Stats.Losses;
        _player1.Rating = gameStats.Player1Stats.Rating;

        _player2.Wins = gameStats.Player2Stats.Wins;
        _player2.Losses = gameStats.Player2Stats.Losses;
        _player2.Rating = gameStats.Player2Stats.Rating;
    }
}
