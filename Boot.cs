class Boot
{
    static void Main(string[] args)
    {
        int shipCount = 8;
        int fieldWidth = 10;
        int fieldHeight = 10;
        int maxWins = 3;
        string gameMode = "pvp";

        var factory = new FactoryPlayer();
        var (player1, player2, render) = factory.CreateGame(shipCount, fieldWidth, fieldHeight, gameMode);

        new Game(player1, player2, render, maxWins).Start();
    }
}
