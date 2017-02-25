using System.Collections.Generic;
using System.Linq;

namespace codecov.Services.Utils
{
    public static class ServiceFactory
    {
        private static readonly IEnumerable<IService> Services = new IService[] { new AppVeyor(), new Travis(), new TeamCity(), new Git() };

        public static IService CreateService => Services.FirstOrDefault(x => x.Detect);
    }
}