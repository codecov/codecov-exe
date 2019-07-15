namespace Codecov.Url
{
    internal class Route : IRoute
    {
        public string GetFallbackRoute => "upload/v2";
        public string GetRoute => "upload/v4";
    }
}
