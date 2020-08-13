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
            var project = new CProject();
            var document = TestUtils.Test("helloworld.c", project);
            Assert.NotNull(document);

            Assert.Single(document.Includes);
            var include = document.Includes[0];
            Assert.EndsWith("test.c", include.FileName);

            Assert.Equal(2, document.Functions.Count);
            FunctionDefinition main = document.Functions[0];
            Assert.Equal("main", main.Name);
            Assert.Equal(new Scope {
                    Start = new Position(2, 12),
                    End = new Position(8, 0)
                }, main.BodyScope);
            Assert.Equal(new Scope
            {
                Start = new Position(2, 12)
            }, main.Scope);
            Assert.Equal(3, main.LocalVariables.Count);
            LocalVariable result = main.LocalVariables[0];
            Assert.Equal("result", result.Name);
            Assert.Equal(new Scope
            {
                Start = new Position(3, 22),
                End = new Position(8, 0)
            }, result.Scope);

            Assert.Equal("code", main.LocalVariables[1].Name);

            FunctionDefinition test = document.Functions[1];
            Assert.Equal("test", test.Name);
            Assert.Equal(new Scope
            {
                Start = new Position(10, 23),
                End = new Position(12, 0)
            }, test.BodyScope);
            Assert.Equal(new Scope
            {
                Start = new Position(10, 23)
            }, test.Scope);
            Assert.Empty(test.LocalVariables);

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(2, 11, items, CancellationToken.None);
                Assert.Empty(items);
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(2, 12, items, CancellationToken.None);
                Assert.Equal(2, items.Count); // main
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(3, 21, items, CancellationToken.None);
                Assert.Equal(2, items.Count); // main
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(3, 22, items, CancellationToken.None);
                Assert.Equal(4, items.Count); // main, result, code
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(7, 4, items, CancellationToken.None);
                Assert.Equal(5, items.Count);
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(9, 4, items, CancellationToken.None);
                Assert.Empty(items);
            }

            {
                var items = new List<CompletionItem>();
                document.TriggerCompletion(11, 5, items, CancellationToken.None);
                Assert.Equal(3, items.Count);
            }
        }
    }
}
