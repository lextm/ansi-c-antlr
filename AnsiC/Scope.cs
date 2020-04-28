using LanguageServer.VsCode.Contracts;

namespace Lextm.AnsiC
{
    public struct Scope
    {
        public Position Start;
        public Position? End;

        public bool InScope(int line, int character)
        {
            if (line < Start.Line || (End != null && line > End.Value.Line))
            {
                return false;
            }

            if (line == Start.Line && character < Start.Character)
            {
                return false;
            }

            if (End != null && line == End.Value.Line && character > End.Value.Character)
            {
                return false;
            }

            return true;
        }
    }
}