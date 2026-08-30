using AwesomeAssertions;
using Soenneker.Tests.Unit;

namespace Soenneker.Extensions.Strings.Files.Tests;

public sealed class StringsFilesExtensionTests : UnitTest
{
    [Test]
    public void AppendDateTime_RemovesUnsafeFileNameCharacters()
    {
        string result = " report/../Q?:1 ".AppendDateTime(utcNow: new System.DateTime(2026, 1, 2, 3, 4, 5, System.DateTimeKind.Utc));

        result.Should().NotContain("/").And.NotContain(":").And.NotContain("?").And.NotContain(" ");
    }

    [Test]
    public void ToFileName_DoesNotAllowAPathInTheExtension()
    {
        string result = "report".ToFileName("../\\payload.exe");

        result.Should().NotContain("/").And.NotContain("\\");
        result.Should().EndWith(".payload.exe");
    }
}
