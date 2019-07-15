namespace Codecov.Url
{
    internal interface IRoute
    {
        string GetFallbackRoute { get; }

        string GetRoute { get; }
    }
}
