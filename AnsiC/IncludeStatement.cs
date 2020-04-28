using System.IO;
using System.Text.RegularExpressions;

namespace Lextm.AnsiC
{
    public class IncludeStatement
    {
        public IncludeStatement(string text, Scope scope, string fileName)
        {
            var match = Regex.Match(text, "\\#\\s?include\\s+\"(?<filename>.*)\"\\s*");
            if (match == null)
            {
                match = Regex.Match(text, "\\#\\s?include\\s+<(?<filename>.*)>\\s*");
                var includeFolder = "C:\\";
                var sourceFile = match.Groups["filename"].Value;
                sourceFile = Path.Combine(includeFolder, sourceFile);
                if (File.Exists(sourceFile))
                {
                    FileName = sourceFile;
                }
            }
            else
            {
                var sourceFile = match.Groups["filename"].Value;
                if (File.Exists(sourceFile))
                {
                    FileName = sourceFile;
                }
                else
                {
                    sourceFile = Path.Combine(Path.GetDirectoryName(fileName), sourceFile);
                    if (File.Exists(sourceFile))
                    {
                        FileName = sourceFile;
                    }
                }
            }

            Scope = scope;
        }

        public string FileName { get; }

        public Scope Scope { get; }
    }
}