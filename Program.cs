using SeaBattleRework;

class Program
{
    static void Main(string[] args)
    {
        Logic.InitializationField();

        while (true)
        {
            Console.Clear();
            Logic.ControlStepPlayer();
            Console.WriteLine("Enter coordinates (a-j 1-10):\t");
            Logic.GetInput();
        }
    }
}