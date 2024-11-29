class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.InitializeGame();

        while (!game.IsGameOver)
        {
            game.Render();
            game.HandleInput();
            game.CheckEndGame();
        }
    }
}
