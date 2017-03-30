namespace Codecov.Services.Helpers
{
    public interface IService
    {
        bool Detect { get; }

        string Query { get; }

        string RepoRoot { get; }

        string SourceCodeFiles { get; }

        void SetQueryParams();
    }
}