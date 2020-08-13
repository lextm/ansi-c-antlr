using System;

namespace Lextm.AnsiC
{
    public class CacheItem
    {
        public CacheItem(CompilationUnit document, DateTime time)
        {
            Document = document;
            LastModifiedTime = time;
        }

        public DateTime LastModifiedTime { get; }
        public CompilationUnit Document { get; }
    }
}