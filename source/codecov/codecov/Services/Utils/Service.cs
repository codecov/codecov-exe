using System.Collections.Generic;
using System.Linq;

namespace codecov.Services.Utils
{
    public class Service : IService
    {
        private IEnumerable<IDetect> Services { get; }

        public Service(IEnumerable<IDetect> services)
        {
            Services = services;
        }

        public IUrl Find => (IUrl)Services.FirstOrDefault(x => x.Detect);
    }
}