using System;

namespace Kakuro
{
    public class Pair<T, U>
    {
        public T Left { get; set; }
        public U Right { get; set; }

        public Pair(T left, U right)
        {
            this.Left = left;
            this.Right = right;
        }

        override public int GetHashCode()
        {
            int hash = 7;
            hash = (79 * hash) + Left.GetHashCode();
            return (79 * hash) + Right.GetHashCode();
        }

        override public bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var that = obj as Pair<T, U>;
            if (that == null)
            {
                return false;
            }
            else
            {
                return Left.Equals(that.Left) && Right.Equals(that.Right);
            }
        }

        override public String ToString()
        {
            return "Pair[left=" + Left.ToString() + ", right=" + Right.ToString() + "]";
        }

    }
}
