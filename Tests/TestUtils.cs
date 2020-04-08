using System.IO;

namespace Lextm.AnsiC.Tests
{
    public static class TestUtils
    {
        public static CompilationUnit Test(string fileName)
        {
            var path = Path.Combine("Input", fileName);
            var result = CParser.ParseDocument(path);
            return result;
        }
    }
}