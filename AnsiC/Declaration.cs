using System.Collections.Generic;

namespace Lextm.AnsiC
{
    internal class Declaration : IBlockItem
    {
        public Declaration(Scope scope)
            : this(new List<DeclarationSpecifier>(), scope)
        {
        }

        public Declaration(List<DeclarationSpecifier> specifiers, Scope scope)
            : this(specifiers, null, scope)
        {
        }

        public Declaration(List<DeclarationSpecifier> specifiers, List<InitDeclarator> declarators, Scope scope)
        {
            Specifiers = specifiers;
            Declarators = declarators ?? new List<InitDeclarator>();
            Scope = scope;
        }

        public Scope Scope { get; }

        internal List<DeclarationSpecifier> Specifiers { get; }
        internal List<InitDeclarator> Declarators { get; }
    }
}