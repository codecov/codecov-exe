using System.Collections.Generic;
using Codecov.Terminal;

namespace Codecov.Factories
{
    internal static class TerminalFactory
    {
        public static IDictionary<TerminalName, ITerminal> Create()
        {
            return new Dictionary<TerminalName, ITerminal> { { TerminalName.Generic, new Terminal.Terminal() } };
        }
    }
}
