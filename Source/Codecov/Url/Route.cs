namespace Codecov.Url
{
    internal class Route : IRoute
    {
        public string GetRoute => "upload/v4";

        public string GetFallbackRoute => "upload/v2";
    }
}
