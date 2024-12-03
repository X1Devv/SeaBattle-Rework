public static class FactoryPlayers
{
    public static (Player player1, Player player2, RenderField render) CreateGame(int shipCount, int fieldWidth, int fieldHeight, string gameMod)
    {
        Field.Configure(fieldWidth, fieldHeight);

        var players = gameMod.ToLower() switch//це дуже проста фабрика я поки тільки вчу абстракну фабрику і фабричний метод
        {
            "pvp" => (new Player(false, shipCount), new Player(false, shipCount)),
            "pve" => (new Player(false, shipCount), new Player(true, shipCount)),
            "eve" => (new Player(true, shipCount), new Player(true, shipCount)),
            _ => (new Player(false, shipCount), new Player(false, shipCount))
        };

        return (players.Item1, players.Item2, new RenderField());
    }
}
