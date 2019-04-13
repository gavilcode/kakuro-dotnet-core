using System;
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

        public static IList<IList<T>> Transpose<T>(IList<IList<T>> m)
        {
            if (0 == m.Count)
            {
                return new List<IList<T>>();
            }
            else
            {
                return Enumerable.Range(0, m[0].Count)
                        .Select(i => m.Select(col => col[i]).ToList())
                        .Cast<IList<T>>()
                        .ToList();
            }
        }

        public static bool IsPossible(ValueCell v, int n) => v.Contains(n);

        public static IList<T> Drop<T>(int n, IList<T> coll) => coll.Skip(n).ToList();

        public static IList<T> Take<T>(int n, IList<T> coll) => coll.Take(n).ToList();


        public static IList<IList<T>> PartitionBy<T>(Predicate<T> f, IList<T> coll)
        {
            if (0 == coll.Count)
            {
                return Enumerable.Empty<IList<T>>().ToList();
            }
            else
            {
                T head = coll[0];
                bool fx = f(head);
                var group = coll.TakeWhile(y => fx == f(y)).ToList();
                return ConcatLists(AsList(group), PartitionBy(f, Drop(group.Count, coll)));
            }
        }

        public static IList<IList<T>> PartitionAll<T>(int n, int step, IList<T> coll)
        {
            if (0 == coll.Count)
            {
                return Enumerable.Empty<IList<T>>().ToList();
            }
            else
            {
                return ConcatLists(AsList(Take(n, coll).ToList()), PartitionAll(n, step, Drop(step, coll)));
            }
        }

        public static IList<IList<T>> PartitionN<T>(int n, IList<T> coll) => PartitionAll(n, n, coll);

        public static IList<ValueCell> SolveStep(IList<ValueCell> cells, int total)
        {
            int finalIndex = cells.Count - 1;
            var perms = PermuteAll(cells, total)
                    .Where(v => IsPossible(cells.Last(), v[finalIndex]))
                    .Where(AllDifferent)
                    .ToList();
            return Transpose(perms)
                    .Select(v)
                    .ToList();
        }

        // returns (non-vals, vals)*
        public static IList<IList<ICell>> GatherValues(IList<ICell> line) => PartitionBy(v => v is ValueCell, line);

        public static IList<SimplePair<IList<ICell>>> PairTargetsWithValues(IList<ICell> line)
        {
            return PartitionN(2, GatherValues(line))
                    .Select(part => new SimplePair<IList<ICell>>(part[0], (1 == part.Count) ? new List<ICell>() : part[1]))
                    .ToList();
        }

        public static IList<ICell> SolvePair(Func<ICell, int> f, SimplePair<IList<ICell>> pair)
        {
            var notValueCells = pair.Left;
            if (0 == pair.Right.Count)
            {
                return notValueCells;
            }
            else
            {
                var valueCells = pair.Right.Cast<ValueCell>().ToList();
                var newValueCells = SolveStep(valueCells, f(notValueCells.Last()));
                return notValueCells.Concat(newValueCells).ToList();
            }
        }

        public static IList<ICell> SolveLine(IList<ICell> line, Func<ICell, int> f)
        {
            return PairTargetsWithValues(line)
                    .SelectMany(pair => SolvePair(f, pair))
                    .ToList();
        }

        public static IList<ICell> SolveRow(IList<ICell> r) => SolveLine(r, x => ((IAcross)x).Across);

        public static IList<ICell> SolveColumn(IList<ICell> c) => SolveLine(c, x => ((IDown)x).Down);

        public static IList<IList<ICell>> SolveGrid(IList<IList<ICell>> grid)
        {
            var rowsDone = grid.Select(SolveRow).ToList();
            var colsDone = Transpose(rowsDone).Select(SolveColumn).ToList();
            return Transpose(colsDone);
        }

        public static bool GridEquals(IList<IList<ICell>> g1, IList<IList<ICell>> g2)
        {
            if (g1.Count == g2.Count)
            {
                return Enumerable.Range(0, g1.Count).All(i =>
                {
                    var xi = g1[i];
                    var yi = g2[i];
                    return Enumerable.Range(0, xi.Count).All(j => (xi.Count == yi.Count) && xi[j].Equals(yi[j]));
                });
            }
            else
            {
                return false;
            }
        }

        public static IList<IList<ICell>> Solver(IList<IList<ICell>> grid)
        {
            Console.WriteLine(DrawGrid(grid));
            var g = SolveGrid(grid);
            if (GridEquals(g, grid))
            {
                return g;
            }
            else
            {
                return Solver(g);
            }
        }


    }
}
