namespace Kakuro
{
    public struct EmptyCell : ICell
    {
        public string Draw()
        {
            return "   -----  ";
        }
    }
}
