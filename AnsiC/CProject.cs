using System.Collections.Generic;
using System.IO;

namespace Lextm.AnsiC
{
    public class CProject
    {
        private IDictionary<string, CacheItem> Cache = new Dictionary<string, CacheItem>();

        internal CompilationUnit GetDocument(string fileName)
        {
            var fromDisk = new FileInfo(fileName).LastWriteTimeUtc;
            if (Cache.ContainsKey(fileName))
            {
                var record = Cache[fileName];
                var lastModifiedTime = record.LastModifiedTime;

                if (fromDisk <= lastModifiedTime)
                {
                    return record.Document;
                }
            }

            var result = CParser.ParseDocument(fileName, this);
            Cache.Add(fileName, new CacheItem(result, fromDisk));
            return result;
        }
    }
}