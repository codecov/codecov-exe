using System.Collections.Generic;
using System.Linq;
using codecov.Program;

namespace codecov.Services.Helpers
{
    public class ServiceFactory
    {
        public ServiceFactory(Options options)
        {
            Options = options;
        }

        public IService CreateService => Services.FirstOrDefault(x => x.Detect);

        private Options Options { get; }

        private IEnumerable<IService> Services => new IService[] { new AppVeyor(Options), new Travis(Options), new TeamCity(Options), new Git(Options) };
    }
}