using System;

namespace Codecov.Url
{
    internal interface IUrl
    {
        Uri GetUrl { get; }

        Uri GetFallbackUrl { get; }
    }
}
