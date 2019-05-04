using System.Runtime.InteropServices;
using Xunit;

namespace Codecov.Tests
{
    public class UnixFactAttribute : FactAttribute
    {
        public UnixFactAttribute(string reason = null)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Skip = reason ?? "non-windows test";
            }
        }
    }
}
