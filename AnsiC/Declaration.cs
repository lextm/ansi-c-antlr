using System.Collections.Generic;

namespace Lextm.AnsiC
{
    internal class Declaration : IBlockItem
    {
        public Declaration(List<DeclarationSpecifier> specifiers)
            : this(specifiers, null)
        {
        }

        public Declaration(List<DeclarationSpecifier> specifiers, List<InitDeclarator> declarators)
        {
            Specifiers = specifiers;
            Declarators = declarators ?? new List<InitDeclarator>();
        }

        internal List<DeclarationSpecifier> Specifiers { get; }
        internal List<InitDeclarator> Declarators { get; }
    }
}