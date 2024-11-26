class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.InitializeGame();

        Player player1 = new Player(game, 1);
        Player player2 = new Player(game, 2);

        while (!game.CheckEndGame())
        {
            if (game.YourStep)
            {
                player1.HandleTurn();
            }
            else
            {
                player2.HandleTurn();
            }
        }
    }
}
