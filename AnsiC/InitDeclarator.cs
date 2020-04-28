using LanguageServer.VsCode.Contracts;

namespace Lextm.AnsiC
{
    internal class InitDeclarator
    {
        public InitDeclarator(Declarator declarator, Scope scope)
        {
            Name = declarator.DirectDeclarator.Name;
            Scope = scope;
        }

        public string Name { get; }

        public Scope Scope { get; private set; }

        internal void OverrideScope(Scope scope)
        {
            Scope = new Scope
            {
                Start = scope.Start,
                End = new Position(scope.End.Value.Line, scope.End.Value.Character - 1)
            };
        }
    }
}