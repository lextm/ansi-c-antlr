using Antlr4.Runtime;
using LanguageServer.VsCode.Contracts;

namespace Lextm.AnsiC
{
    internal static class ContextExtension
    {
        public static Scope ToScope(this ParserRuleContext context)
        {
            return new Scope
            {
                Start = new Position { Line = context.Start.Line - 2, Character = context.Start.Column },
                End = new Position { Line = context.Stop.Line - 2, Character = context.Stop.Column }
            };
        }
    }
}
