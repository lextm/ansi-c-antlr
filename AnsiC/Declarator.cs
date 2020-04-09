namespace Lextm.AnsiC
{
    public class Declarator
    {
        public Declarator(DirectDeclarator directDeclarator)
        {
            DirectDeclarator = directDeclarator;
        }

        public DirectDeclarator DirectDeclarator { get; }
    }
}