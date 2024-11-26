public class Field
{
    public const int Width = 10;
    public const int Height = 10;
    const char EmptyCell = '.';
    const char ShipCell = 'U';
    const char HitCell = 'X';
    const char FillCell = '#';

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
            
            int Row = random.Next(0, Height);
            int Col = random.Next(0, Width);

            if (grid[Row, Col] == EmptyCell)
            {
                grid[Row, Col] = ShipCell;
                i++;
            }
        }
    }

    public bool LogicHit(int Row, int Col, out bool Hit)
    {
        Hit = false;

        if (grid[Row, Col] == ShipCell)
        {
            grid[Row, Col] = HitCell;
            Hit = true;

            DrawGridAround(Row, Col);
            CheckShipsAround(Row, Col);

            return true;
        }
        else if (grid[Row, Col] == EmptyCell)
        {
            grid[Row, Col] = FillCell;
            return true;
        }

        return false;
    }

    public void DrawGridAround(int Row, int Col)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int NewRow = Row + dx;
                int NewCol = Col + dy;

                if (NewRow >= 0 && NewRow < Height && NewCol >= 0 && NewCol < Width)
                {
                    if (grid[NewRow, NewCol] == EmptyCell)
                    {
                        grid[NewRow, NewCol] = FillCell;
                    }
                }
            }
        }
    }
    public void CheckShipsAround(int Row, int Col)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int NewRow = Row + dx;
                int NewCol = Col + dy;

                if (NewRow >= 0 && NewRow < Height && NewCol >= 0 && NewCol < Width)
                {
                    if (grid[NewRow, NewCol] == ShipCell)
                    {
                        grid[NewRow, NewCol] = HitCell;
                        DrawGridAround(NewRow, NewCol);
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

        for (int Row = 0; Row < Height; Row++)
        {
            Console.Write((char)('a' + Row) + " ");
            for (int Col = 0; Col < Width; Col++)
            {
                char cell = grid[Row, Col];
                Console.Write((hideShips && cell == ShipCell ? EmptyCell : cell) + " ");
            }

            Console.WriteLine();
        }
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
