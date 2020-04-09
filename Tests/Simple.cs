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
            Assert.Equal(2, document.Functions.Count);
            Assert.Equal("main", document.Functions[0].Name);
            Assert.Equal("test", document.Functions[1].Name);
        }
    }
}
