using System;
using System.Diagnostics.Contracts;
using Soenneker.Extensions.DateTime;
using Soenneker.Extensions.String;
using Soenneker.Utils.TimeZones;

namespace Soenneker.Extensions.Strings.Files;

/// <summary>
/// A collection of helpful file related string extension methods
/// </summary>
public static class StringsFilesExtension
{
    /// <summary>
    /// Removes whitespace, appends datetime in file format
    /// </summary>
    [Pure]
    public static string AppendDateTime(this string? value, System.TimeZoneInfo? timeZoneInfo = null, System.DateTime? utcNow = null)
    {
        utcNow ??= System.DateTime.UtcNow;
        timeZoneInfo ??= Tz.Eastern;

        string result;

        if (value != null)
        {
            string removedWhiteSpaced = value.RemoveWhiteSpace();
            result = $"{removedWhiteSpaced}-{utcNow.Value.ToTzFileName(timeZoneInfo)}";
        }
        else
        {
            result = utcNow.Value.ToTzFileName(timeZoneInfo);
        }

        return result;
    }

    /// <summary>
    /// Removes whitespace, appends datetime in file format, and appends the extension
    /// </summary>
    [Pure]
    public static string ToFileName(this string? value, string extension, System.TimeZoneInfo? timeZoneInfo = null)
    {
        return $"{value.AppendDateTime(timeZoneInfo)}-{Guid.NewGuid()}.{extension}";
    }
}
