using System.Collections.Generic;
using LanguageServer.VsCode.Contracts;
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
            FunctionDefinition main = document.Functions[0];
            Assert.Equal("main", main.Name);
            Assert.Equal(2, main.LocalVariables.Count);
            Assert.Equal("result", main.LocalVariables[0]);
            Assert.Equal("code", main.LocalVariables[1]);
            Assert.Equal("test", document.Functions[1].Name);
            Assert.Empty(document.Functions[1].LocalVariables);

            {
                var items = new List<CompletionItem>();
                Assert.False(document.TriggerMethodNames(1, 1, items));
                Assert.Equal(2, items.Count);
            }

            {
                var items = new List<CompletionItem>();
                Assert.True(document.TriggerMethodNames(5, 5, items));
                Assert.Equal(4, items.Count);
            }
        }
    }
}
