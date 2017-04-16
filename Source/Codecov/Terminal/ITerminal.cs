namespace Codecov.Terminal
{
    internal interface ITerminal
    {
        string Run(string command, string commandArguments);
    }
}
