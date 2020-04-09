namespace Lextm.AnsiC
{
    public class FunctionDefinition
    {
        private Declarator declarator;

        public FunctionDefinition(Declarator declarator)
        {
            this.declarator = declarator;
            Name = declarator.DirectDeclarator.Name;
        }

        public string Name { get; }
    }
}