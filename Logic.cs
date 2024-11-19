using System;

namespace SeaBattleRework
{
    public class Logic
    {
        static int Width = 10;
        static int Height = 10;
        static int CountShip = 5;
        static bool YourStep = false;

        static char[,] PlayerField = new char[Width, Height];
        static char[,] EnemyField = new char[Width, Height];

        public static void DrawField(int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    PlayerField[i, j] = '.';
                    EnemyField[i, j] = '.';
                }
            }
        }

        public static void DrawMarking(char[,] field)
        {
            Console.Write("  ");
            for (int count = 1; count <= Width; count++)
            {
                Console.Write(count + " ");
            }
            Console.WriteLine();

            for (int row = 0; row < Height; row++)
            {
                Console.Write((char)('a' + row) + " ");
                for (int count = 0; count < Width; count++)
                {
                    Console.Write(field[row, count] + " ");
                }
                Console.WriteLine();
            }
        }

        public static void DrawShipsInField(char[,] field)
        {
            Random random = new Random();

            for (int i = 0; i < CountShip; i++)
            {
                int X = random.Next(0, Height);
                int Y = random.Next(0, Width);

                if (field[X, Y] == '.')
                {
                    field[X, Y] = 'U';
                }
            }
        }

        public static void InitializationField()
        {
            DrawField(Width, Height);
            
            DrawShipsInField(PlayerField);
            DrawShipsInField(EnemyField);
            
            if (YourStep != true)
                DrawMarking(EnemyField);
            else
                DrawMarking(PlayerField);
        }

        public static void StartGame()
        {
            InitializationField();
        }
    }
}
