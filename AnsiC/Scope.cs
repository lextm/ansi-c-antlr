namespace Lextm.AnsiC
{
    public struct Scope
    {
        public Position Start;
        public Position? End;

        public bool InScope(int line, int character)
        {
            if (line < Start.Row || (End != null && line > End.Value.Row))
            {
                return false;
            }

            if (line == Start.Row && character < Start.Column)
            {
                return false;
            }

            if (End != null && line == End.Value.Row && character > End.Value.Column)
            {
                return false;
            }

            return true;
        }
    }
}