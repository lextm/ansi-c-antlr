using System;

namespace Lextm.AnsiC
{
    public class LocalVariable
    {
        internal LocalVariable(InitDeclarator variable, Scope methodScope)
        {
            Name = variable.Name;
            Scope = new Scope {
                Start = variable.Scope.End.Value,
                End = methodScope.End
            };
        }

        public Scope Scope { get; }

        public string Name { get; }
    }
}