using Codecov.Utilities;
using FluentAssertions;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;

namespace Codecov.Tests.Utilities
{
    public class PathFilterBuilderTests
    {
        [Theory]
        [MemberData(nameof(ExtensionFilterTestData))]
        public void ExtensionFilter_Should_Return_True_When_ExtensionsMatch(string path, string extensionFilter, bool expected)
        {
            var filter = new PathFilterBuilder()
                .FileHasExtension(extensionFilter)
                .Build();

            filter.Matches(path).Should().Be(expected);
        }

        public static TheoryData<string, string, bool> ExtensionFilterTestData
        {
            get
            {
                var data = new TheoryData<string, string, bool>();
                data.Add(CreatePath("file.txt"), ".txt", true);
                data.Add(CreatePath("file.txt"), ".jpg", false);
                data.Add(CreatePath("file.TxT"), ".txt", true);
                data.Add(CreatePath("file"), ".txt", false);
                data.Add(CreatePath("file.txt"), "txt", false);

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(PathFilterTestData))]
        public void PathFilter_Should_Return_True_When_ExtensionsMatch(string path, string subPath, bool expected)
        {
            var filter = new PathFilterBuilder()
                .PathContains(subPath)
                .Build();

            filter.Matches(path).Should().Be(expected);
        }
        
        public static TheoryData<string, string, bool> PathFilterTestData
        {
            get
            {
                var data = new TheoryData<string, string, bool>();
                data.Add(CreatePath("ChildPath"), @"\Foo", true);
                data.Add(CreatePath("ChildPath"), @"/Foo", true);
                data.Add(CreatePath("ChildPath"), @"Foo/", true);
                data.Add(CreatePath("ChildPath"), @"Foo", true);
                data.Add(CreatePath("ChildPath"), @"Bar", false);
                data.Add(CreatePath("ChildPath"), @"USERS/Foo", true);

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(CompositeFilterData))]
        public void CompoundFilter_Should_Return_True_When_MatchIsFound(string path, string pathFilter, string fileFilter, bool expected)
        {
            var filter = new PathFilterBuilder()
                .PathContains(pathFilter)
                .FileHasExtension(fileFilter)
                .Build();

            filter.Matches(path).Should().Be(expected);
        }

        public static TheoryData<string, string, string, bool> CompositeFilterData
        {
            get
            {
                var data = new TheoryData<string, string, string, bool>();
                data.Add(CreatePath("file.txt"), @"\Users", ".txt", true);
                data.Add(CreatePath("file.txt"), @"\Groups", ".txt", true);
                data.Add(CreatePath("file.txt"), @"\Groups", ".zip", false);
                data.Add("", @"\Users", ".txt", false);

                return data;
            }
        }

        private static string CreatePath(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Path.Combine(@"C:\Users\Foo\Documents", path);
            }
            else
            {
                return Path.Combine(@"/users/Foo/files", path);
            }
        }
    }
}
