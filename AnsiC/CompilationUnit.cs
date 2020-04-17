using Antlr4.Runtime;
using System;
using System.Collections.Generic;

namespace Lextm.AnsiC
{
    public class CompilationUnit
    {
        public IList<FunctionDefinition> Functions { get; } = new List<FunctionDefinition>();
        public IList<string> Includes { get; } = new List<string>();

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

        internal void Process(IList<IToken> channel)
        {
            foreach (var line in channel)
            {
                Includes.Add(line.Text);
            }
        }

        public bool TriggerDocumentList(int line, int character)
        {
            return false;
        }

        public bool TriggerMethodNames(int line, int character)
        {
            return true;
        }
    }
}