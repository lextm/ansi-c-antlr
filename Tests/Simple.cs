using System.Collections.Generic;
using System.Threading;
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
            Assert.Equal(3, main.LocalVariables.Count);
            Assert.Equal("result", main.LocalVariables[0].Name);
            Assert.Equal("code", main.LocalVariables[1].Name);
            Assert.Equal("test", document.Functions[1].Name);
            Assert.Empty(document.Functions[1].LocalVariables);

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(1, 1, items, CancellationToken.None);
                Assert.Equal(0, items.Count);
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(5, 5, items, CancellationToken.None);
                Assert.Equal(1, items.Count);
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(7, 5, items, CancellationToken.None);
                Assert.Equal(3, items.Count);
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(9, 5, items, CancellationToken.None);
                Assert.Equal(4, items.Count);
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(13, 5, items, CancellationToken.None);
                Assert.Equal(2, items.Count);
            }
        }
    }
}
