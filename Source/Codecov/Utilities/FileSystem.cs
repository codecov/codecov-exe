using System;
using System.IO;

namespace Codecov.Utilities
{
    internal static class FileSystem
    {
        internal static string NormalizedPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            // We only need to replace the windows specific seperator, as these do not work on Unix
            var absolutePath = Path.GetFullPath(path.Replace('\\', Path.DirectorySeparatorChar));

            return !string.IsNullOrWhiteSpace(absolutePath)
              ? Path.GetFullPath(new Uri(absolutePath).LocalPath).TrimEnd('\\', '/')
              : string.Empty;
        }
    }
}
