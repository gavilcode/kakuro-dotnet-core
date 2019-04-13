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
    }
}
