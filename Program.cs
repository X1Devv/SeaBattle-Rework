using SeaBattleRework;

class Program
{
    static void Main(string[] args)
    {
        Logic.InitializationField();

        while (Logic.CheckGame())
        {
            Logic.DrowField();
            Logic.ControlStepPlayer();
        }
    }
}
