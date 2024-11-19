using SeaBattleRework;

class Program
{
    static void Main(string[] args)
    {
        Logic.InitializationField();

        while (true)
        {
            Console.Clear();
            if (Logic.YourStep)
            {
                Console.WriteLine("Your turn:");
                Logic.DrawMarking(Logic.EnemyField);
            }
            else
            {
                Console.WriteLine("Enemy turn:");
                Logic.DrawMarking(Logic.PlayerField);
            }

            string input;
            while (true)
            {
                Console.WriteLine("Enter coordinates (a-j 1-10):\t");
                input = Console.ReadLine();

                if (Logic.IsValidCoordinate(input))
                {
                    Logic.GetInput(input);
                    break;
                }
            }
        }
    }
}