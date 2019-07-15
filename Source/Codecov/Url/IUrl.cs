using System;

namespace Codecov.Url
{
    internal interface IUrl
    {
        Uri GetFallbackUrl { get; }

        Uri GetUrl { get; }
    }
}
