namespace Codecov.Utilities
{
    internal interface IPathFilter
    {
        bool Matches(string path);
    }
}
