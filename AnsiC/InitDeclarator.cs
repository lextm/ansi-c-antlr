namespace Lextm.AnsiC
{
    internal class InitDeclarator
    {
        private Declarator declarator;

        public InitDeclarator(Declarator declarator)
        {
            this.declarator = declarator;
        }

        public string Name => declarator.DirectDeclarator.Name;
    }
}