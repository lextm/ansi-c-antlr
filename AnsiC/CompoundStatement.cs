using System.Collections.Generic;

namespace Lextm.AnsiC
{
    public class CompoundStatement
    {
        public List<IBlockItem> Lists { get; }

        public CompoundStatement(List<IBlockItem> lists)
        {
            Lists = lists;
        }

        public CompoundStatement()
        {
            Lists = new List<IBlockItem>();
        }
    }
}