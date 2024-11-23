namespace SeaBattleRework
{
    public class Logic
    {
        const int Width = 10;
        const int Height = 10;
        public static int CountShip = 5;
        public static int KilledShipsEnemy = 0;
        public static int KilledShipsPlayer = 0;
        public static bool YourStep = true;
        static bool Hit = false;

        public static char[,] PlayerField = new char[Width, Height];
        public static char[,] EnemyField = new char[Width, Height];

        public static void InitializationField()
        {
            DrawField(PlayerField);
            DrawField(EnemyField);
            PlaceShips(PlayerField);
            PlaceShips(EnemyField);
        }

        public enum Coordinates
        {
            a,
            b,
            c,
            d,
            e,
            f,
            g,
            h,
            i,
            j
        }

        public static void DrawField(char[,] field)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                    field[i, j] = '.';
            }
        }

        public static void DrawMarking(char[,] field)
        {
            Console.Write("  ");
            for (int i = 1; i <= Width; i++)
                Console.Write(i + " ");

            Console.WriteLine();

            for (int Row = 0; Row < Height; Row++)
            {
                Console.Write((char)('a' + Row) + " ");
                for (int Left = 0; Left < Width; Left++)
                    Console.Write(field[Row, Left] + " ");

                Console.WriteLine();
            }
        }

        public static void PlaceShips(char[,] field)
        {
            Random random = new Random();

            for (int i = 0; i < CountShip;)
            {
                int x = random.Next(0, Height);
                int y = random.Next(0, Width);

                if (field[x, y] == '.')
                {
                    field[x, y] = 'U';
                    i++;
                }
            }
        }

        public static void DrowField()
        {
            Console.Clear();
            Console.WriteLine(YourStep ? "Player:" : "Enemy:");
            DrawMarking(YourStep ? EnemyField : PlayerField);
        }

        public static void ControlStepPlayer()
        {
            string input = GetInput();
            HandleCoordinate(input);
        }

        public static string GetInput()
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

        public static bool ValidCoordinate(string input)
        {
            input = input.Replace(" ", "");

            if (input.Length < 2 || input.Length > 3)
                return false;

            char CharSymbol = input[0];
            if (CharSymbol < 'a' || CharSymbol > 'j')
                return false;

            if (!int.TryParse(input.AsSpan(1), out int Left))
                return false;

            return Left >= 1 && Left <= 10;
        }

        public static void HandleCoordinate(string input)
        {
            Hit = false;

            char CharSymbol = input[0];
            int Left = int.Parse(input.Substring(1)) - 1;
            int Row = (int)Enum.Parse(typeof(Coordinates), CharSymbol.ToString());

            LogicHitShips(Row, Left);
        }

        public static void LogicHitShips(int Row, int Left)
        {
            if (YourStep)
                UpdateField(EnemyField, Row, Left, ref KilledShipsEnemy);
            else
                UpdateField(PlayerField, Row, Left, ref KilledShipsPlayer);

            if (!Hit)
                YourStep = !YourStep;
        }

        private static void UpdateField(char[,] field, int Row, int Left, ref int KilledShips)
        {
            if (field[Row, Left] == 'U')
            {
                field[Row, Left] = 'X';
                Hit = true;
                KilledShips++;
                CheckShipsAndFillGridsAround(field, Row, Left, ref KilledShips);
            }
            else if (field[Row, Left] == '.')
            {
                field[Row, Left] = '#';
            }
        }

        public static void CheckShipsAndFillGridsAround(char[,] field, int Row, int Left, ref int KilledShips)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int newRow = Row + dx;
                    int newLeft = Left + dy;

                    if (newRow >= 0 && newRow < Height && newLeft >= 0 && newLeft < Width)
                    {
                        if (field[newRow, newLeft] == 'U')
                        {
                            field[newRow, newLeft] = 'X';
                            KilledShips++;
                        }
                        else if (field[newRow, newLeft] == '.')
                        {
                            field[newRow, newLeft] = '#';
                        }
                    }
                }
            }
        }

        public static bool CheckGame()
        {
            if (KilledShipsEnemy >= CountShip || KilledShipsPlayer >= CountShip)
            {
                EndGame();
                return false;
            }
            return true;
        }

        private static void EndGame()
        {
            if (KilledShipsEnemy >= CountShip)
            {
                Console.WriteLine("Player won this game!");
            }
            else if (KilledShipsPlayer >= CountShip)
            {
                Console.WriteLine("Enemy won this game!");
            }
            Environment.Exit(0);
        }
    }
}
