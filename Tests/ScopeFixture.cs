using System.Collections.Generic;
using LanguageServer.VsCode.Contracts;
using Xunit;

namespace Lextm.AnsiC.Tests
{
    public class ScopeFixture
    {
        [Fact]
        public void Normal()
        {
            var scope = new Scope {
                Start = new Position { Row = 5, Column = 10 },
                End = new Position { Row = 9, Column = 5}
            };

            Assert.False(scope.InScope(0, 0));
            Assert.False(scope.InScope(10, 10));
            Assert.True(scope.InScope(5, 11));
            Assert.True(scope.InScope(9, 4));            
        }

        [Fact]
        public void Function()
        {
            var scope = new Scope {
                Start = new Position { Row = 5, Column = 10 }
            };

            Assert.False(scope.InScope(0, 0));
            Assert.True(scope.InScope(5, 11));
            Assert.True(scope.InScope(9, 4));            
        }
    }
}
