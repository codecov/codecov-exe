using System.Collections.Generic;
using System.Linq;
using Codecov.Program;

namespace Codecov.Services.Helpers
{
    public class ServiceFactory
    {
        public ServiceFactory(Options options)
        {
            Options = options;
        }

        public IService CreateService => Services.FirstOrDefault(x => x.Detect);

        private Options Options { get; }

        private IEnumerable<IService> Services => new IService[] { new AppVeyor(Options), new TeamCity(Options), new Git(Options), new Service(Options) };
    }
}