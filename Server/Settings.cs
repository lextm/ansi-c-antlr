﻿namespace Lextm.ReStructuredText.LanguageServer
{

    public class SettingsRoot
    {
        public AnsiCSettings ReStructuredText { get; set; }
    }

    public class AnsiCSettings
    {
        public string ConfPath { get; set; }
        public string WorkspaceRoot { get; set; }
        public LanguageServerSettings LanguageServer { get; set; } = new LanguageServerSettings();
    }

    public class LanguageServerSettings
    {
        public int MaxNumberOfProblems { get; set; } = 10;

        public LanguageServerTraceSettings Trace { get; } = new LanguageServerTraceSettings();
    }

    public class LanguageServerTraceSettings
    {
        public string Server { get; set; }
    }
}
