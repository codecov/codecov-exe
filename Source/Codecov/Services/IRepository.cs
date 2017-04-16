namespace Codecov.Services
{
    internal interface IRepository
    {
        string Branch { get; }

        string Commit { get; }

        string Pr { get; }

        string Slug { get; }

        string Tag { get; }
    }
}
