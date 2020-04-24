namespace Lextm.AnsiC
{
    internal class BrokenBlock : IBlockItem
    {
        public BrokenBlock(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}