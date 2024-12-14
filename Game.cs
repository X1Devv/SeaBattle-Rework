using System;
using System.IO;

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
    private string _file;

    public Game(Player player1, Player player2, RenderField render, int maxWinsToFinish, bool saveEnabled)
    {
        _player1 = player1;
        _player2 = player2;
        _render = render;
        _maxWinsToFinish = maxWinsToFinish;
        _saveEnabled = saveEnabled;

        _currentPlayer = _player1;
        _opponent = _player2;

        _file = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "game_data.txt");

        LoadStats();
    }

    public void Start()
    {
        if (!File.Exists(_file))
        {
            using (var stream = File.Create(_file)) { }
            InitializeStats();
        }

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
    private void InitializeStats()
    {
        _player1.MMR = 1000;
        _player1.Wins = 0;
        _player1.Losses = 0;

        _player2.MMR = 1000;
        _player2.Wins = 0;
        _player2.Losses = 0;
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
        Console.WriteLine($"Player 1>Wins: {_player1Wins}, Losses> {player1.Losses}, MMR> {player1.MMR}, Win Rate>{winRate1:F2}%");

        double winRate2 = CalculateWinRate(player2);
        Console.WriteLine($"Player 2>Wins: {_player2Wins}, Losses> {player2.Losses}, MMR> {player2.MMR}, Win Rate> {winRate2:F2}%");

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
        string data = $"{_player1Wins}|{_player2Wins}|{_player1.Wins},{_player1.Losses},{_player1.MMR}|{_player2.Wins},{_player2.Losses},{_player2.MMR}\n";
        using (StreamWriter writer = new StreamWriter(_file, append: true))
        {
            writer.Write(data);
        }
    }

    public void LoadStats()
    {
        if (File.Exists(_file))
        {
            var lines = File.ReadAllLines(_file);
            foreach (var line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length > 3)
                {
                    string[] player1Stats = parts[2].Split(',');
                    _player1.MMR = int.Parse(player1Stats[2]);
                    _player1.Wins = int.Parse(player1Stats[0]);
                    _player1.Losses = int.Parse(player1Stats[1]);

                    string[] player2Stats = parts[3].Split(',');
                    _player2.MMR = int.Parse(player2Stats[2]);
                    _player2.Wins = int.Parse(player2Stats[0]);
                    _player2.Losses = int.Parse(player2Stats[1]);
                }
            }
        }
        else
            InitializeStats();
    }
}
