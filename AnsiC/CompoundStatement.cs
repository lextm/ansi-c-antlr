using System.Collections.Generic;

namespace Lextm.AnsiC
{
    public class CompoundStatement
    {
        public List<IBlockItem> Lists { get; }
        public Scope Scope { get; }

        public CompoundStatement(List<IBlockItem> lists, Scope scope)
        {
            Lists = lists;
            Scope = scope;
        }

        public CompoundStatement(Scope scope)
        : this(new List<IBlockItem>(), scope)
        {
        }
    }
}