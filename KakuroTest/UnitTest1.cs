using System.Collections.Generic;
using System.Linq;

using Kakuro;

using Xunit;

using static Kakuro.Kakuro;

namespace KakuroTest
{
    public class UnitTest1
    {
        [Fact]
        public void TestDrawEmpty()
        {
            var result = e().Draw();
            Assert.Equal("   -----  ", result);
        }

        [Fact]
        public void TestDrawAcross()
        {
            var result = a(5).Draw();
            Assert.Equal("   --\\ 5  ", result);
        }

        [Fact]
        public void TestDrawDown()
        {
            var result = d(4).Draw();
            Assert.Equal("    4\\--  ", result);
        }

        [Fact]
        public void TestDrawDownAcross()
        {
            var result = da(3, 4).Draw();
            Assert.Equal("    3\\ 4  ", result);
        }


        [Fact]
        public void TestDrawValues()
        {
            var result = v().Draw();
            Assert.Equal(" 123456789", result);
            var result12 = v(1, 2).Draw();
            Assert.Equal(" 12.......", result12);
        }

        [Fact]
        public void TestDrawRow()
        {
            var line = AsList(da(3, 4), v(), v(1, 2), d(4), e(), a(5), v(4), v(1));
            var result = DrawRow(line);
            Assert.Equal("    3\\ 4   123456789 12.......    4\\--     -----     --\\ 5       4         1    \n", result);
        }


        [Fact]
        public void TestProduct()
        {
            var data = AsList(AsSet(1, 2), AsSet(10), AsSet(100, 200, 300));
            var expected = AsList(
              AsList(1, 10, 100),
              AsList(1, 10, 200),
              AsList(1, 10, 300),
              AsList(2, 10, 100),
              AsList(2, 10, 200),
              AsList(2, 10, 300));
            Assert.Equal(expected, Product(data));
        }

        [Fact]
        public void TestPermute()
        {
            var vs = AsList(v(), v(), v());
            var results = PermuteAll(vs, 6);
            Assert.Equal(10, results.Count);
            var diff = results.Where(k => AllDifferent(k)).ToList();
            Assert.Equal(6, diff.Count);
        }

        [Fact]
        public void TestTranspose()
        {
            var ints = Enumerable.Range(0, 3)
              .Select(_ => Enumerable.Range(0, 4).ToList())
              .Cast<IList<int>>()
              .ToList();
            var tr = Transpose(ints);
            Assert.Equal(ints.Count, tr[0].Count);
            Assert.Equal(ints[0].Count, tr.Count);
        }

        [Fact]
        public void TestValueEquality()
        {
            Assert.Equal(v(), v());
            Assert.Equal(v(1, 2), v(1, 2));
        }

        [Fact]
        public void TestIsPoss()
        {
            var vc = v(1, 2, 3);
            Assert.True(IsPossible(vc, 2));
            Assert.False(IsPossible(vc, 4));
        }

        [Fact]
        public void TestTakeWhile()
        {
            var result = Enumerable.Range(0, 10).TakeWhile(n => n < 4).ToList();
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void TestTakeWhile2()
        {
            var result = Enumerable.Range(0, 10).TakeWhile(n => (n < 4) || (n > 6)).ToList();
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void TestConcat()
        {
            var a = AsList(1, 2, 3);
            var b = AsList(4, 5, 6, 1, 2, 3);
            var result = ConcatLists(a, b);
            Assert.Equal(9, result.Count);
        }

        [Fact]
        public void TestDrop()
        {
            var a = AsList(1, 2, 3, 4, 5, 6);
            var result = Drop(4, a);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void TestTake()
        {
            var a = AsList(1, 2, 3, 4, 5, 6);
            var result = Take(4, a);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public void TestPartBy()
        {
            var data = AsList(1, 2, 2, 2, 3, 4, 5, 5, 6, 7, 7, 8, 9);
            var result = PartitionBy(n => 0 == (n % 2), data);
            Assert.Equal(9, result.Count);
        }

        [Fact]
        public void TestPartAll()
        {
            var data = AsList(1, 2, 2, 2, 3, 4, 5, 5, 6, 7, 7, 8, 9);
            var result = PartitionAll(5, 3, data);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void TestPartN()
        {
            var data = AsList(1, 2, 2, 2, 3, 4, 5, 5, 6, 7, 7, 8, 9);
            var result = PartitionN(5, data);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void TestSolveStep()
        {
            IList<ValueCell> result = SolveStep(AsList(v(1, 2), v()), 5);
            Assert.Equal(v(1, 2), result[0]);
            Assert.Equal(v(3, 4), result[1]);
        }

        [Fact]
        public void TestGatherValues()
        {
            var line = AsList(da(3, 4), v(), v(), d(4), e(), a(4), v(), v());
            var result = GatherValues(line);
            Assert.Equal(4, result.Count);
            Assert.Equal(da(3, 4), result[0][0]);
            Assert.Equal(d(4), result[2][0]);
            Assert.Equal(e(), result[2][1]);
            Assert.Equal(a(4), result[2][2]);
        }

        [Fact]
        public void TestPairTargets()
        {
            var line = AsList(da(3, 4), v(), v(), d(4), e(), a(4), v(), v());
            var result = PairTargetsWithValues(line);
            Assert.Equal(2, result.Count);
            Assert.Equal(da(3, 4), result[0].Left[0]);
            Assert.Equal(d(4), result[1].Left[0]);
            Assert.Equal(e(), result[1].Left[1]);
            Assert.Equal(a(4), result[1].Left[2]);
        }

        [Fact]
        public void TestSolvePair()
        {
            var line = AsList(da(3, 4), v(), v(), d(4), e(), a(4), v(), v());
            var pairs = PairTargetsWithValues(line);
            var pair = pairs[0];
            var result = SolvePair(cell => ((IDown)cell).Down, pair);
            Assert.Equal(3, result.Count);
            Assert.Equal(v(1, 2), result[1]);
            Assert.Equal(v(1, 2), result[2]);
        }


        [Fact]
        public void TestSolveLine()
        {
            var line = AsList(da(3, 4), v(), v(), d(4), e(), a(5), v(), v());
            var result = SolveLine(line, x => ((IAcross)x).Across);
            Assert.Equal(8, result.Count);
            Assert.Equal(v(1, 3), result[1]);
            Assert.Equal(v(1, 3), result[2]);
            Assert.Equal(v(1, 2, 3, 4), result[6]);
            Assert.Equal(v(1, 2, 3, 4), result[7]);
        }

        [Fact]
        public void TestSolveRow()
        {
            var result = SolveRow(AsList(a(3), v(1, 2, 3), v(1)));
            Assert.Equal(v(2), result[1]);
            Assert.Equal(v(1), result[2]);
        }

        [Fact]
        public void TestSolveCol()
        {
            var result = SolveColumn(AsList(da(3, 12), v(1, 2, 3), v(1)));
            Assert.Equal(v(2), result[1]);
            Assert.Equal(v(1), result[2]);
        }

        [Fact]
        public void TestGridEquals()
        {
            var grid1 = AsList(
                    AsList(e(), d(4), d(22), e(), d(16), d(3)),
                    AsList(a(3), v(), v(), da(16, 6), v(), v()),
                    AsList(a(18), v(), v(), v(), v(), v()),
                    AsList(e(), da(17, 23), v(), v(), v(), d(14)),
                    AsList(a(9), v(), v(), a(6), v(), v()),
                    AsList(a(15), v(), v(), a(12), v(), v()));
            var grid2 = AsList(
                    AsList(e(), d(4), d(22), e(), d(16), d(3)),
                    AsList(a(3), v(), v(), da(16, 6), v(), v()),
                    AsList(a(18), v(), v(), v(), v(), v()),
                    AsList(e(), da(17, 23), v(), v(), v(), d(14)),
                    AsList(a(9), v(), v(), a(6), v(), v()),
                    AsList(a(15), v(), v(), a(12), v(), v()));
            Assert.True(GridEquals(grid1, grid2));
        }

        [Fact]
        public void TestGridEquals2()
        {
            var grid1 = AsList(
                    AsList(e(), d(4), d(22), e(), d(16), d(3)),
                    AsList(a(3), v(), v(), da(16, 6), v(), v()),
                    AsList(a(18), v(), v(), v(), v(), v()),
                    AsList(e(), da(17, 23), v(), v(), v(), d(14)),
                    AsList(a(9), v(), v(), a(6), v(), v()),
                    AsList(a(15), v(), v(), a(12), v(), v()));
            var grid2 = AsList(
                    AsList(e(), d(4), d(22), e(), d(16), d(3)),
                    AsList(a(3), v(), v(), da(16, 6), v(), v()),
                    AsList(a(18), v(), v(), v(), v(), v()),
                    AsList(e(), da(17, 23), v(), v(), v(), d(14)),
                    AsList(a(15), v(), v(), a(12), v(), v()));
            Assert.False(GridEquals(grid1, grid2));
        }

        [Fact]
        public void TestGridEquals3()
        {
            var grid1 = AsList(
                    AsList(e(), d(4), d(22), e(), d(16), d(3)),
                    AsList(a(3), v(), v(), da(16, 6), v(), v()),
                    AsList(a(18), v(), v(), v(), v(), v()),
                    AsList(e(), da(17, 23), v(), v(), v(), d(14)),
                    AsList(a(9), v(), v(), a(6), v(), v()),
                    AsList(a(15), v(), v(), a(12), v(), v()));
            var grid2 = AsList(
                    AsList(e(), d(4), d(22), e(), d(16)),
                    AsList(a(3), v(), v(), da(16, 6), v(), v()),
                    AsList(a(18), v(), v(), v(), v(), v()),
                    AsList(e(), da(17, 23), v(), v(), v(), d(14)),
                    AsList(a(9), v(), v(), a(6), v(), v()),
                    AsList(a(15), v(), v(), a(12), v(), v()));
            Assert.False(GridEquals(grid1, grid2));
        }

        [Fact]
        public void TestSolver()
        {
            var grid1 = AsList(
                    AsList(e(), d(4), d(22), e(), d(16), d(3)),
                    AsList(a(3), v(), v(), da(16, 6), v(), v()),
                    AsList(a(18), v(), v(), v(), v(), v()),
                    AsList(e(), da(17, 23), v(), v(), v(), d(14)),
                    AsList(a(9), v(), v(), a(6), v(), v()),
                    AsList(a(15), v(), v(), a(12), v(), v()));
            var result = Solver(grid1);
            Assert.Equal("   --\\ 3       1         2       16\\ 6       4         2    \n", DrawRow(result[1]));
            Assert.Equal("   --\\18       3         5         7         2         1    \n", DrawRow(result[2]));
            Assert.Equal("   -----     17\\23       8         9         6       14\\--  \n", DrawRow(result[3]));
            Assert.Equal("   --\\ 9       8         1       --\\ 6       1         5    \n", DrawRow(result[4]));
            Assert.Equal("   --\\15       9         6       --\\12       3         9    \n", DrawRow(result[5]));
        }

    }
}
