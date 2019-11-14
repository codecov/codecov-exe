namespace Codecov.Services
{
    internal interface IRepository
    {
        string Branch { get; }

        string Commit { get; }

        string Pr { get; }

        string Project { get; }

        string ServerUri { get; }

        string Slug { get; }

        string Tag { get; }
    }
}
