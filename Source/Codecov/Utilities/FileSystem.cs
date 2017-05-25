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

            var absolutePath = Path.GetFullPath(path);

            return !string.IsNullOrWhiteSpace(absolutePath) ? Path.GetFullPath(new Uri(absolutePath).LocalPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) : string.Empty;
        }
    }
}
