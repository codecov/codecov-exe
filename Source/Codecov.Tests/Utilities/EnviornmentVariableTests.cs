using System;
using Codecov.Utilities;
using FluentAssertions;
using Xunit;

namespace Codecov.Tests.Utilities
{
    public class EnviornmentVariableTests : IDisposable
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

        [Fact]
        public void GetFirstExistingEnvironmentVariable_Should_Return_First_Value_If_Variable_Exists()
        {
            // Given
            Environment.SetEnvironmentVariable("foo", "fooValue");

            // When
            var foo = EnviornmentVariable.GetFirstExistingEnvironmentVariable("foo", "bar");

            // Then
            foo.Should().Be("fooValue");
        }

        [Fact]
        public void GetFirstExistingEnvironmentVariable_Should_Return_Second_Value_If_First_Doesnt_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("bar", "barValue");

            // When
            var foo = EnviornmentVariable.GetFirstExistingEnvironmentVariable("foo", "bar");

            // Then
            foo.Should().Be("barValue");
        }

        [Fact]
        public void GetFirstExistingEnvironmentVariable_Should_Return_Empty_String_If_None_Exist()
        {
            // Given
            Environment.SetEnvironmentVariable("foo", null);
            Environment.SetEnvironmentVariable("bar", null);

            // When
            var foo = EnviornmentVariable.GetFirstExistingEnvironmentVariable("foo", "bar");

            // Then
            foo.Should().BeEmpty();
        }

        public void Dispose()
        {
            // We will remove all environment variables that could have been set during unit test
            var envVariable = new[]
            {
                "foo",
                "bar",

            };

            foreach (var variable in envVariable)
            {
                Environment.SetEnvironmentVariable(variable, null);
            }
        }
    }
}
