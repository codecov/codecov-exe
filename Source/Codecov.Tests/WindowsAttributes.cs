using System.Runtime.InteropServices;
using Xunit;

// ReSharper disable VirtualMemberCallInConstructor

namespace Codecov.Tests
{
    public class WindowsFactAttribute : FactAttribute
    {
        public WindowsFactAttribute(string reason = null)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Skip = reason ?? "windows test.";
            }
        }
    }

    public class WindowsTheoryAttribute : TheoryAttribute
    {
        public WindowsTheoryAttribute(string reason = null)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Skip = reason ?? "windows test.";
            }
        }
    }
}
