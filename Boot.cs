class Boot
{
    static void Main(string[] args)
    {
        //config=================
        int shipCount = 8;
        int fieldWidth = 10;
        int fieldHeight = 10;
        int MaxWin = 3;
        string gameMod = "pve";//pvp pve eve
        //config=================

        var (player1, player2, render) = FactoryPlayers.CreateGame(shipCount, fieldWidth, fieldHeight, gameMod);

        new Game(player1, player2, render, MaxWin).Start();
    }
}
