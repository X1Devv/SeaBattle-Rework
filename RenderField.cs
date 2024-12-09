public class RenderField
{
    public void Render(Field field, bool hideShips)
    {
        Console.Write("  ");
        for (int col = 1; col <= Field.Width; col++)
        {
            Console.Write(col + " ");
        }
        Console.WriteLine();

        for (int row = 0; row < Field.Height; row++)
        {
            Console.Write((char)('a' + row) + " ");
            for (int col = 0; col < Field.Width; col++)
            {
                char cell = field.PeekCell(row, col);
                Console.Write(hideShips && cell == Field.ShipCell ? Field.EmptyCell : cell);
                Console.Write(" ");
            }
            Console.WriteLine();
        }
    }
}
