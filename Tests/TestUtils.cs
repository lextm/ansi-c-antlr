using System.IO;
using System.Reflection;

namespace Lextm.AnsiC.Tests
{
    public static class TestUtils
    {
        public static CompilationUnit Test(string fileName)
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Input", fileName);
            var result = CParser.ParseDocument(path);
            return result;
        }
    }
}