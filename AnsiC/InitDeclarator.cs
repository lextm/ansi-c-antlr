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

        public Scope Scope { get; }
    }
}