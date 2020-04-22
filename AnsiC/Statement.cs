namespace Lextm.AnsiC
{
    public class Statement : IBlockItem
    {
        public Statement(Scope scope)
        {
            Scope = scope;
        }

        public Scope Scope { get; }
    }
}