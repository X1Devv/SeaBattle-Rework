public class Field
{
    public const char EmptyCell = '.';
    public const char ShipCell = 'U';
    const char HitCell = 'X';
    const char MissCell = '#';

    private char[,] _grid;

    public static int Width { get; private set; }
    public static int Height { get; private set; }

    public Field()
    {
        _grid = new char[Height, Width];
        InitializeField();
    }

    public static void Configure(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public void InitializeField()
    {
        for (int i = 0; i < Height; i++)
            for (int j = 0; j < Width; j++)
                _grid[i, j] = EmptyCell;
    }

    public void PlaceRandomShips(int count)
    {
        Random random = new Random();
        for (int i = 0; i < count;)
        {
            int row = random.Next(0, Height);
            int col = random.Next(0, Width);

            if (_grid[row, col] == EmptyCell)
            {
                _grid[row, col] = ShipCell;
                i++;
            }
        }
    }

    public bool ProcessAttack(int row, int col)
    {
        if (_grid[row, col] == ShipCell)
        {
            _grid[row, col] = HitCell;
            return true;
        }

        if (_grid[row, col] == EmptyCell)
        {
            _grid[row, col] = MissCell;
            return false;
        }

        return false;
    }

    public bool IsCellAttackable(int row, int col) => _grid[row, col] != HitCell && _grid[row, col] != MissCell;

    public bool AllShipsDestroyed() => !_grid.Cast<char>().Any(cell => cell == ShipCell);

    public char PeekCell(int row, int col) => _grid[row, col];
}
