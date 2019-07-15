namespace Codecov.Url
{
    internal interface IQueryOptions
    {
        string Branch { get; }

        string Build { get; }

        string Commit { get; }

        string Flags { get; }

        string Name { get; }

        string Pr { get; }

        string Slug { get; }

        string Tag { get; }

        string Token { get; }
    }
}
