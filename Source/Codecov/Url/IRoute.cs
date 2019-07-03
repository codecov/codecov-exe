namespace Codecov.Url
{
    internal interface IRoute
    {
        string GetRoute { get; }

        string GetFallbackRoute { get; }
    }
}
