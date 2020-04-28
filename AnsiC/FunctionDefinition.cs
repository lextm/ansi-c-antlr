using System.Collections.Generic;
using System.Threading;
using LanguageServer.VsCode.Contracts;

namespace Lextm.AnsiC
{
    public class FunctionDefinition
    {
        public FunctionDefinition(Declarator declarator, CompoundStatement statement)
        {
            Name = declarator.DirectDeclarator.Name;
            BodyScope = new Scope
            {
                Start = new Position
                {
                    Line = statement.Scope.Start.Line,
                    Character = statement.Scope.Start.Character + 1
                },
                End = statement.Scope.End
            };
            Scope = new Scope {
                Start = BodyScope.Start
            };
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
        public Scope Scope { get; }
        public IList<LocalVariable> LocalVariables { get; } = new List<LocalVariable>();

        internal void TriggerCompletion(int line, int character, List<CompletionItem> items, CancellationToken token)
        {
            if (Scope.InScope(line, character))
            {
                items.Add(new CompletionItem(Name, CompletionItemKind.Method, null));
            }

            var inScope = BodyScope.InScope(line, character);
            if (!inScope)
            {
                return;
            }

            foreach (var local in LocalVariables)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                if (local.Scope.InScope(line, character))
                {
                    items.Add(new CompletionItem(local.Name, CompletionItemKind.Variable, null));
                }
            }
        }
    }
}