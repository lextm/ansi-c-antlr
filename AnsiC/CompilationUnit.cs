using Antlr4.Runtime;
using LanguageServer.VsCode.Contracts;
using System.Collections.Generic;
using System.Threading;

namespace Lextm.AnsiC
{
    public class CompilationUnit
    {
        public IList<FunctionDefinition> Functions { get; } = new List<FunctionDefinition>();
        public IList<IncludeStatement> Includes { get; } = new List<IncludeStatement>();

        internal void Handle(List<ExternalDelaration> declarations)
        {
            foreach (var declaration in declarations)
            {
                if (declaration?.FunctionDefinition != null)
                {
                    Functions.Add(declaration.FunctionDefinition);
                }
            }
        }

        internal void Process(IList<IToken> channel, string fileName)
        {
            foreach (var line in channel)
            {
                Includes.Add(new IncludeStatement(line.Text, 
                    new Scope
                    {
                        Start = new Position(line.Line - 1, line.Column + line.Text.Length)
                    },
                    fileName));
            }
        }

        public bool TriggerDocumentList(int line, int character)
        {
            return false;
        }

        public void TriggerCompletion(int line, int character, List<CompletionItem> items, CancellationToken token)
        {
            var inMethod = false;
            foreach (var method in Functions)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                method.TriggerCompletion(line, character, items, token);
                inMethod |= method.BodyScope.InScope(line, character);
            }

            if (inMethod)
            {
                foreach (var include in Includes)
                {
                    if (include.Scope.InScope(line, character))
                    {
                        var document = CParser.ParseDocument(include.FileName);
                        foreach (var method in document.Functions)
                        {
                            items.Add(new CompletionItem(method.Name, CompletionItemKind.Method, null));
                        }
                    }
                }
            }

            if (!inMethod)
            {
                items.Clear();
            }
        }
    }
}