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
                document.TriggerCompletion(3, 12, items, CancellationToken.None);
                Assert.Empty(items);
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(3, 13, items, CancellationToken.None);
                Assert.Single(items); // main
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(4, 22, items, CancellationToken.None);
                Assert.Single(items); // main
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(4, 23, items, CancellationToken.None);
                Assert.Equal(3, items.Count); // main, result, code
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(8, 5, items, CancellationToken.None);
                Assert.Equal(4, items.Count);
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(10, 5, items, CancellationToken.None);
                Assert.Empty(items);
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(12, 5, items, CancellationToken.None);
                Assert.Equal(2, items.Count);
            }
        }
    }
}
