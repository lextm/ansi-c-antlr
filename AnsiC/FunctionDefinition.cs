using System.Collections.Generic;
using LanguageServer.VsCode.Contracts;

namespace Lextm.AnsiC
{
    public class FunctionDefinition
    {
        public FunctionDefinition(Declarator declarator, CompoundStatement statement)
        {
            Name = declarator.DirectDeclarator.Name;
            BodyScope = statement.Scope;
            foreach (var blockItem in statement.Lists)
            {
                if (blockItem is Declaration declaration)
                {
                    foreach (var variable in declaration.Declarators)
                    {
                        LocalVariables.Add(new LocalVariable(variable, BodyScope));
                    }
                }
            }
        }

        public string Name { get; }
        public Scope BodyScope { get; }
        public IList<LocalVariable> LocalVariables { get; } = new List<LocalVariable>();

        internal bool TriggerLocalVariables(int line, int character, List<CompletionItem> items)
        {
            // TODO: should remove {} from scope.
            var inScope = BodyScope.InScope(line, character);
            if (inScope)
            {
                foreach (var local in LocalVariables)
                {
                    if (local.Scope.InScope(line, character))
                    {
                        items.Add(new CompletionItem(local.Name, CompletionItemKind.Variable, null));
                    }
                }
            }

            return inScope;
        }
    }
}