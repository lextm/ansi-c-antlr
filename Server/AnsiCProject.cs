using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LanguageServer.VsCode.Contracts;
using LanguageServer.VsCode.Server;

namespace Lextm.ReStructuredText.LanguageServer
{
    public class AnsiCProject
    {
        private string _root;

        public void Refresh(AnsiCSettings sessionSettings)
        {
            // TODO: check conf.py for include files.
             var setting = sessionSettings.ConfPath;
             var workspaceRoot = sessionSettings.WorkspaceRoot;
             _root = setting.Replace("${workspaceRoot}", workspaceRoot);
        }

        public void RefreshDocument(TextDocument doc)
        {
//            var key = doc.Uri.ToString();
//            if (!Files.ContainsKey(key))
//            {
//                var path = key;
//                Files.Add(key, path);
//            }
        }

        internal Hover GetHover(DocumentState doc, Position position)
        {
            // TODO:
            return null;
        }

        public CompletionList GetCompletionList(DocumentState textDocument, Position position)
        {
            var document = textDocument.LintedDocument;
            if (document.TriggerDocumentList(position.Line, position.Character))
            {
                var Files = new Dictionary<string, string>();
                foreach (string file in Directory.EnumerateFiles(
                    _root, "*.c", SearchOption.AllDirectories))
                {
                    Files.Add(GetPath(file), file);
                }

                foreach (string file in Directory.EnumerateFiles(
                    _root, "*.h", SearchOption.AllDirectories))
                {
                    Files.Add(GetPath(file), file);
                }

                return new CompletionList(Files.Select(_ =>
                    new CompletionItem(_.Key, CompletionItemKind.Text, _.Value, null)), true);
            }

            var items = new List<CompletionItem>();
            if (document.TriggerCompletion(position.Line, position.Character, items))
            {
                return new CompletionList(items, true);
            }

            return new CompletionList();
        }

        private string GetPath(string file)
        {
            var part = file.Substring(_root.Length);
            return part.TrimStart('\\', '/').Replace('\\', '/').Replace(".rst", null).Replace(".rest", null);
        }
    }
}