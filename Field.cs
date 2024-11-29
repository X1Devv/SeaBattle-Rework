public class Field
{
    public const int Width = 10;
    public const int Height = 10;
    public const char EmptyCell = '.';
    public const char ShipCell = 'U';
    public const char HitCell = 'X';
    public const char FillCell = '#';

    char[,] grid = new char[Width, Height];

    public Field()
    {
        InitializeField();
    }

    public void InitializeField()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
                grid[i, j] = EmptyCell;
        }
    }

    public void PlaceRandomShips(int count)
    {
        Random random = new Random();
        for (int i = 0; i < count;)
        {
            int row = random.Next(0, Height);
            int col = random.Next(0, Width);

            if (grid[row, col] == EmptyCell)
            {
                grid[row, col] = ShipCell;
                i++;
            }
        }
    }

    public bool LogicHit(int row, int col, out bool ShipHit)
    {
        ShipHit = false;

        if (grid[row, col] == ShipCell)
        {
            grid[row, col] = HitCell;
            ShipHit = true;
            DrawGridAround(row, col);
            CheckShipsAround(row, col);
            return true;
        }
        else if (grid[row, col] == EmptyCell)
        {
            grid[row, col] = FillCell;
            return true;
        }

        return false;
    }


    public void DrawGridAround(int row, int col)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int newRow = row + dx;
                int newCol = col + dy;

                if (newRow >= 0 && newRow < Height && newCol >= 0 && newCol < Width)
                {
                    if (grid[newRow, newCol] == EmptyCell)
                    {
                        grid[newRow, newCol] = FillCell;
                    }
                }
            }
        }
    }

    public void CheckShipsAround(int row, int col)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int newRow = row + dx;
                int newCol = col + dy;

                if (newRow >= 0 && newRow < Height && newCol >= 0 && newCol < Width)
                {
                    if (grid[newRow, newCol] == ShipCell)
                    {
                        grid[newRow, newCol] = HitCell;
                        DrawGridAround(newRow, newCol);
                    }
                }
            }
        }
    }

    public void Draw(bool hideShips = true)
    {
        Console.Write("  ");
        for (int i = 1; i <= Width; i++)
            Console.Write(i + " ");

        Console.WriteLine();

        for (int row = 0; row < Height; row++)
        {
            Console.Write((char)('a' + row) + " ");
            for (int col = 0; col < Width; col++)
            {
                char cell = grid[row, col];
                Console.Write((hideShips && cell == ShipCell ? EmptyCell : cell) + " ");
            }

            Console.WriteLine();
        }
    }

    public char PeekCell(int row, int col)
    {
        return grid[row, col];
    }

    public bool AllShipsKilled()
    {
        foreach (var cell in grid)
        {
            if (cell == ShipCell)
                return false;
        }
        return true;
    }
}
