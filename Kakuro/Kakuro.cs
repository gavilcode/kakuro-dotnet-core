using System.Collections.Generic;
using System.Linq;

namespace Kakuro
{
    public static class Kakuro
    {
        public static EmptyCell e() => new EmptyCell();

        public static DownCell d(int d) => new DownCell(d);

        public static AcrossCell a(int a) => new AcrossCell(a);

        public static DownAcrossCell da(int d, int a) => new DownAcrossCell(d, a);

        public static ValueCell v() => new ValueCell();

        public static ValueCell v(ICollection<int> values) => new ValueCell(values);

        public static ValueCell v(params int[] values) => new ValueCell(values);

        public static IList<T> AsList<T>(params T[] values) => new List<T>(values);

        public static IList<ICell> AsList(params ICell[] values) => AsList<ICell>(values);

        public static ISet<T> AsSet<T>(params T[] values) => new SortedSet<T>(values);

        public static string Join(string a, string b) => a + b;

        public static string DrawRow(IList<ICell> r) => r.Select(c => c.Draw()).Aggregate("", Join) + "\n";

        public static string DrawGrid(IList<IList<ICell>> g) => g.Select(DrawRow).Aggregate("", Join);

        public static bool AllDifferent<T>(ICollection<T> nums) => nums.Count == new HashSet<T>(nums).Count;

        public static IList<T> ConcatLists<T>(IEnumerable<T> a, IEnumerable<T> b) => a.Concat(b).ToList();

        public static IList<IList<T>> Product<T>(IList<ISet<T>> colls)
        {
            switch (colls.Count)
            {
                case 0:
                    return new List<IList<T>>();
                case 1:
                    return colls[0].Select(a => AsList(a)).ToList();
                default:
                    var head = colls[0];
                    var tail = colls.Skip(1).ToList();
                    var tailProd = Product(tail);
                    return head.SelectMany(x => tailProd.Select(ys => ConcatLists(AsList(x), ys)))
                               .ToList();
            }
        }

        public static IList<IList<int>> PermuteAll(IList<ValueCell> vs, int target)
        {
            var values = vs.Select(v => v.values).ToList();
            return Product(values)
                    .Where(x => target == x.Sum())
                    .ToList();
        }

    }
}
