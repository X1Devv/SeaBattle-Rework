namespace SeaBattleRework
{
    public class Logic
    {
        static int Width = 10;
        static int Height = 10;
        public static int CountShip = 5;
        public static bool YourStep = true;
        static int X, Y;

        public static char[,] PlayerField = new char[Width, Height];
        public static char[,] EnemyField = new char[Width, Height];

        public enum Coordinates
        {
            a, b, c, d, e, f, g, h, i, j
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

        public static void ShipPlace()
        {
            Random random = new Random();
            X = random.Next(0, Height);
            Y = random.Next(0, Width);
        }

        public static void DrawShipsInField(char[,] field)
        {
            Random random = new Random();

            for (int i = 0; i < CountShip; i++)
            {
                ShipPlace();
                if (field[X, Y] == '.')
                {
                    field[X, Y] = 'U';
                }
            }
        }

        public static bool IsValidCoordinate(string input)
        {
            input = input.Replace(" ", "");

            return input.Length == 2 || input.Length == 3 && input[0] >= 'a' && input[0] <= 'j'
                   && input[1] >= '1' && input[1] <= '9' && (input.Length == 2 || (input[2] >= '0' && input[2] <= '9'));
        }


        public static void GetInput(string input)
        {
            if (!IsValidCoordinate(input)) 
                return;

            char Word = input[0];
            int number = int.Parse(input.Substring(1)) - 1;
            int Row = (int)Enum.Parse(typeof(Coordinates), Word.ToString());
            int Left = number;

            if (YourStep)
            {
                if (EnemyField[Row, Left] == 'U')
                    EnemyField[Row, Left] = 'X';
                else
                    EnemyField[Row, Left] = '#';
            }
            else
            {
                if (PlayerField[Row, Left] == 'U')
                    PlayerField[Row, Left] = 'X';
                else
                    PlayerField[Row, Left] = '#';
            }

            YourStep = !YourStep;
        }
        public static void InitializationField()
        {
                DrawField(Width, Height);
                DrawShipsInField(PlayerField);
                DrawShipsInField(EnemyField);

        }
    }
}
