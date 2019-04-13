namespace Kakuro
{
    public class EmptyCell : ICell
    {
        public string Draw()
        {
            return "   -----  ";
        }
    }
}
