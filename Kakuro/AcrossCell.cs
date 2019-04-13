namespace Kakuro
{
    public struct AcrossCell : ICell, IAcross
    {
        public int Across { get; set; }

        public AcrossCell(int total)
        {
            Across = total;
        }

        public string Draw()
        {
            return string.Format("   --\\{0,2:D}  ", Across);
        }
    }
}
