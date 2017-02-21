using codecov.Program;

namespace codecov.Services.Utils
{
    public interface IService
    {
        bool Detect { get; }

        string CreateQuery(Options options);
    }
}