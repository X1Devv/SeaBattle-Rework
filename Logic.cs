namespace SeaBattleRework
{
    public class Logic
    {
        static int Width = 10;
        static int Height = 10;
        public static int CountShip = 5;
        public static bool YourStep = true;
        static bool Hit = false;
        static int X, Y;

        public static char[,] PlayerField = new char[Width, Height];
        public static char[,] EnemyField = new char[Width, Height];

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

        public static void DrawField(int Width, int Height)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    PlayerField[i, j] = '.';
                    EnemyField[i, j] = '.';
                }
            }
        }

        public static void DrawMarking(char[,] field)
        {
            Console.Write("  ");
            for (int i = 1; i <= Width; i++)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();

            for (int Row = 0; Row < Height; Row++)
            {
                Console.Write((char)('a' + Row) + " ");
                for (int i = 0; i < Width; i++)
                {
                    Console.Write(field[Row, i] + " ");
                }
                Console.WriteLine();
            }
        }

        public static void ShipCoords()
        {
            Random random = new Random();
            X = random.Next(0, Height);
            Y = random.Next(0, Width);
        }

        public static void DrawShipsInField(char[,] field)
        {

            for (int i = 0; i < CountShip; i++)
            {
                ShipCoords();
                if (field[X, Y] == '.')
                {
                    field[X, Y] = 'U';
                }
            }
        }

        public static bool ValidCoordinate(string input)
        {
            input = input.Replace(" ", "");

            if (input.Length < 2 || input.Length > 3) 
                return false;

            char rowChar = input[0];
            if (rowChar < 'a' || rowChar > 'j') 
                return false;

            if (!int.TryParse(input.AsSpan(1), out int Col))
                return false;

            return Col >= 1 && Col <= 10;
        }

        public static void CoordinateHandler(string input)
        {
            if (!ValidCoordinate(input))
                return;
            
            Hit = false;

            char Word = input[0];
            int number = int.Parse(input.Substring(1)) - 1;
            int Row = (int)Enum.Parse(typeof(Coordinates), Word.ToString());
            int Left = number;

            if (Row < 0 || Row >= Height || Left < 0 || Left >= Width)
                return;

            if (YourStep)
            {
                if (EnemyField[Row, Left] == 'U')
                {
                    EnemyField[Row, Left] = 'X';
                    Hit = true;
                }
                else if (EnemyField[Row, Left] == '.')
                {
                    EnemyField[Row, Left] = '#';
                }
            }
            else
            {
                if (PlayerField[Row, Left] == 'U')
                {
                    PlayerField[Row, Left] = 'X';
                    Hit = true;
                }
                else if (PlayerField[Row, Left] == '.')
                {
                    PlayerField[Row, Left] = '#';
                }
            }
            if (!Hit)
            {
                YourStep = !YourStep;
            }
        }

        public static void GetInput()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (ValidCoordinate(input))
                {
                    CoordinateHandler(input);
                    break;
                }
            }
        }

        public static void InitializationField()
        {
                DrawField(Width, Height);
                DrawShipsInField(PlayerField);
                DrawShipsInField(EnemyField);

        }

        public static void ControlStepPlayer()
        {
            Console.Clear();
            if (Logic.YourStep)
            {
                Console.WriteLine("Your turn:");
                Logic.DrawMarking(EnemyField);
            }
            else
            {
                Console.WriteLine("Enemy turn:");
                Logic.DrawMarking(PlayerField);
            }
        }
    }
}
