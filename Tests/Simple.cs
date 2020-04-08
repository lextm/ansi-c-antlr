using Xunit;

namespace Lextm.AnsiC.Tests
{
    public class Simple
    {
        [Fact]
        public void HelloWorld()
        {
            var document = TestUtils.Test("helloworld");
            Assert.NotNull(document);
        }
    }
}
