namespace Codecov.Terminal
{
    internal interface ITerminal
    {
        bool Exits { get; }

        string Run(string command, params string[] commandArguments);

        string RunScript(string script);
    }
}
