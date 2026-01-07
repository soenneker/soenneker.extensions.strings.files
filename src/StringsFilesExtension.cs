using Soenneker.Extensions.DateTime;
using Soenneker.Extensions.DateTimeOffsets;
using Soenneker.Extensions.String;
using Soenneker.Utils.PooledStringBuilders;
using Soenneker.Utils.TimeZones;
using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string AppendCore(string? value, string timestamp)
    {
        if (value == null)
            return timestamp;

        using var sb = new PooledStringBuilder(64);

        sb.Append(value.RemoveWhiteSpace());
        sb.Append('-');
        sb.Append(timestamp);

        return sb.ToString();
    }

    /// <summary>
    /// Removes whitespace, appends datetime in file format, and appends the extension
    /// </summary>
    [Pure]
    public static string ToFileName(this string? value, string extension, TimeZoneInfo? timeZoneInfo = null)
    {
        using var sb = new PooledStringBuilder(96);

        sb.Append(value.AppendDateTime(timeZoneInfo));
        sb.Append('-');
        sb.Append(Guid.NewGuid());
        sb.Append('.');
        sb.Append(extension);

        return sb.ToString();
    }
}