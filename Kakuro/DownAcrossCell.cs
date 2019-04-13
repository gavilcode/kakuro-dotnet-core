namespace Kakuro
{
    public class DownAcrossCell : ICell, IAcross, IDown
    {
        public int Down { get; set; }
        public int Across { get; set; }

        public DownAcrossCell() { }

        public DownAcrossCell(int down, int across)
        {
            Down = down;
            Across = across;
        }

        public string Draw()
        {
            return string.Format("   {0,2:D}\\{1,2:D}  ", Down, Across);
        }
    }
}
