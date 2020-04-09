using System.Collections.Generic;

namespace Lextm.AnsiC
{
    public class CompilationUnit
    {
        public IList<FunctionDefinition> Functions { get; } = new List<FunctionDefinition>();

        internal void Handle(List<ExternalDelaration> declarations)
        {
            foreach (var declaration in declarations)
            {
                if (declaration.FunctionDefinition != null)
                {
                    Functions.Add(declaration.FunctionDefinition);
                }
            }
        }
    }
}