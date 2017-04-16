using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Url
{
    public class RouteTests
    {
        [Fact]
        public void Should_Be_Version_Four()
        {
            // Given
            var route = new Codecov.Url.Route();

            // When
            var getRoute = route.GetRoute;

            // Then
            getRoute.Should().Be("upload/v4");
        }
    }
}
