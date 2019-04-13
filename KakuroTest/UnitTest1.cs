using System.Linq;
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


    }
}
