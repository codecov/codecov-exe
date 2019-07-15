using System.IO;
using System.Runtime.InteropServices;
using Codecov.Utilities;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Codecov.Tests.Utilities
{
    public class PathFilterBuilderTests
    {
        [UsedImplicitly]
        public static TheoryData<string, string, string, bool> CompositeFilterData
        {
            get
            {
                var data = new TheoryData<string, string, string, bool>
                {
                    { CreatePath("file.txt"), @"\Users", ".txt", true },
                    { CreatePath("file.txt"), @"\Groups", ".txt", true },
                    { CreatePath("file.txt"), @"\Groups", ".zip", false },
                    { "", @"\Users", ".txt", false }
                };

                return data;
            }
        }

        [UsedImplicitly]
        public static TheoryData<string, string, bool> ExtensionFilterTestData
        {
            get
            {
                var data = new TheoryData<string, string, bool>
                {
                    { CreatePath("file.txt"), ".txt", true },
                    { CreatePath("file.txt"), ".jpg", false },
                    { CreatePath("file.TxT"), ".txt", true },
                    { CreatePath("file"), ".txt", false },
                    { CreatePath("file.txt"), "txt", false }
                };

                return data;
            }
        }

        [UsedImplicitly]
        public static TheoryData<string, string, bool> PathFilterTestData
        {
            get
            {
                var data = new TheoryData<string, string, bool>
                {
                    { CreatePath("ChildPath"), @"\Foo", true },
                    { CreatePath("ChildPath"), @"/Foo", true },
                    { CreatePath("ChildPath"), @"Foo/", true },
                    { CreatePath("ChildPath"), @"Foo", true },
                    { CreatePath("ChildPath"), @"Bar", false },
                    { CreatePath("ChildPath"), @"USERS/Foo", true }
                };

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

        [Theory]
        [MemberData(nameof(ExtensionFilterTestData))]
        public void ExtensionFilter_Should_Return_True_When_ExtensionsMatch(string path, string extensionFilter, bool expected)
        {
            var filter = new PathFilterBuilder()
                .FileHasExtension(extensionFilter)
                .Build();

            filter.Matches(path).Should().Be(expected);
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

        private static string CreatePath(string path)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Path.Combine(@"C:\Users\Foo\Documents", path)
                : Path.Combine(@"/users/Foo/files", path);
        }
    }
}
