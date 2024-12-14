class Boot
{
    static void Main(string[] args)
    {
        int shipCount = 8;
        int fieldWidth = 10;
        int fieldHeight = 10;
        int maxWins = 3;
        bool saveEnabled = true;
        string gameMode = "eve"; //pvp, pve, eve

        var factory = new FactoryPlayer();
        var (player1, player2, render) = factory.CreateGame(shipCount, fieldWidth, fieldHeight, gameMode);

        var game = new Game(player1, player2, render, maxWins, saveEnabled);
        game.Start();
    }
}