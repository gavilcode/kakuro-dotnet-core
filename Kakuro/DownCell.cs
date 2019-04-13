namespace Kakuro
{
    public class DownCell : ICell, IDown
    {
        public int Down { get; set; }

        public DownCell(int total)
        {
            Down = total;
        }

        public string Draw()
        {
            return string.Format("   {0,2:D}\\--  ", Down);
        }
    }
}
