using System.Collections.Generic;

namespace Lextm.AnsiC
{
    public class FunctionDefinition
    {

        public FunctionDefinition(Declarator declarator, CompoundStatement statement)
        {
            Name = declarator.DirectDeclarator.Name;
            foreach (var blockItem in statement.Lists)
            {
                if (blockItem is Declaration declaration)
                {
                    foreach (var variable in declaration.Declarators)
                    {
                        LocalVariables.Add(variable.Name);
                    }
                }
            }
        }

        public string Name { get; }
        public IList<string> LocalVariables { get; } = new List<string>();
    }
}