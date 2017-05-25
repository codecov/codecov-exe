using System;
using Codecov.Utilities;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Utilities
{
    public class EnviornmentVariableTests
    {
        [Fact]
        public void GetEnviornmentVariable_Should_Return_Empty_String_If_It_Does_Not_Exit()
        {
            // Given
            Environment.SetEnvironmentVariable("foo", null);

            // When
            var foo = EnviornmentVariable.GetEnviornmentVariable("foo");

            // Then
            foo.Should().BeEmpty();
        }

        [Fact]
        public void GetEnviornmentVariable_Should_Return_EnviornmentVariable_If_It_Exits()
        {
            // Given
            Environment.SetEnvironmentVariable("foo", "bar");

            // When
            var foo = EnviornmentVariable.GetEnviornmentVariable("foo");

            // Then
            foo.Should().Be("bar");
        }
    }
}
