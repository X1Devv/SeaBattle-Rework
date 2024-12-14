public class FactoryPlayer
{
    private Player CreateAIPlayer(int shipCount) => new Player(true, shipCount);

    private Player CreateHumanPlayer(int shipCount) => new Player(false, shipCount);

    public (Player player1, Player player2, RenderField render) CreateGame(int shipCount, int fieldWidth, int fieldHeight, string gameMod)
    {
        Field.Configure(fieldWidth, fieldHeight);

        var players = gameMod.ToLower() switch
        {
            "pvp" => (CreateHumanPlayer(shipCount), CreateHumanPlayer(shipCount)),
            "pve" => (CreateHumanPlayer(shipCount), CreateAIPlayer(shipCount)),
            "eve" => (CreateAIPlayer(shipCount), CreateAIPlayer(shipCount)),
            _ => (CreateHumanPlayer(shipCount), CreateHumanPlayer(shipCount))
        };

        return (players.Item1, players.Item2, new RenderField());
    }
}