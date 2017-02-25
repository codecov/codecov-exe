using System;
using codecov.Services.Utils;

namespace codecov.Services
{
    internal class Travis : Service
    {
        public Travis()
        {
            QueryParameters["branch"] = Environment.GetEnvironmentVariable("TRAVIS_BRANCH");
            QueryParameters["commit"] = Environment.GetEnvironmentVariable("TRAVIS_COMMIT");
            QueryParameters["job"] = Environment.GetEnvironmentVariable("TRAVIS_JOB_ID");
            QueryParameters["build"] = Environment.GetEnvironmentVariable("TRAVIS_JOB_NUMBER");
            QueryParameters["pr"] = Environment.GetEnvironmentVariable("TRAVIS_PULL_REQUEST");
            QueryParameters["slug"] = Environment.GetEnvironmentVariable("TRAVIS_REPO_SLUG");
            QueryParameters["tag"] = Environment.GetEnvironmentVariable("TRAVIS_TAG");
            QueryParameters["service"] = "travis";
        }

        public override bool Detect
        {
            get
            {
                var travis = Environment.GetEnvironmentVariable("TRAVIS");
                var ci = Environment.GetEnvironmentVariable("CI");

                if (string.IsNullOrWhiteSpace(travis) || string.IsNullOrWhiteSpace(ci))
                {
                    return false;
                }

                var isTravis = travis.Equals("True") && ci.Equals("True");
                if (!isTravis)
                {
                    return false;
                }
                Console.WriteLine("==> Travis CI detected.");
                return true;
            }
        }
    }
}