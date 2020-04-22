namespace Lextm.AnsiC
{
    public struct Scope
    {
        public Position Start;
        public Position End;

        public bool InScope(int line, int character)
        {
            if (line < Start.Line || line > End.Line)
            {
                return false;
            }

            if (line == Start.Line && character < Start.Character)
            {
                return false;
            }

            if (line == End.Line && character > End.Character)
            {
                return false;
            }

            return true;
        }
    }
}