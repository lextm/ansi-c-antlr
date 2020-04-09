namespace Lextm.AnsiC
{
    internal class ExternalDelaration
    {
        public ExternalDelaration(FunctionDefinition functionDefinition)
        {
            FunctionDefinition = functionDefinition;
        }

        public ExternalDelaration(Declaration declaration)
        {
            Declaration = declaration;
        }

        public FunctionDefinition FunctionDefinition { get; }
        public Declaration Declaration { get; }
    }
}