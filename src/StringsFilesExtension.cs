using System;
using System.Diagnostics.Contracts;
using Soenneker.Extensions.DateTime;
using Soenneker.Extensions.DateTimeOffsets;
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
    public static string AppendDateTime(this string? value, TimeZoneInfo? timeZoneInfo = null, System.DateTime? utcNow = null)
    {
        utcNow ??= System.DateTime.UtcNow;
        timeZoneInfo ??= Tz.Eastern;

        return AppendCore(value, utcNow.Value.ToTzFileName(timeZoneInfo));
    }

    /// <summary>
    /// Removes whitespace and appends a DateTimeOffset in file-safe format
    /// </summary>
    [Pure]
    public static string AppendDateTimeOffset(this string? value, TimeZoneInfo? timeZoneInfo = null, DateTimeOffset? utcNow = null)
    {
        utcNow ??= DateTimeOffset.UtcNow;
        timeZoneInfo ??= Tz.Eastern;

        return AppendCore(value, utcNow.Value.ToTzFileName(timeZoneInfo));
    }

    [Pure]
    private static string AppendCore(string? value, string timestamp)
    {
        if (value == null)
            return timestamp;

        return $"{value.RemoveWhiteSpace()}-{timestamp}";
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