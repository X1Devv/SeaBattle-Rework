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
            Logic.GetInput();
        }
    }
}