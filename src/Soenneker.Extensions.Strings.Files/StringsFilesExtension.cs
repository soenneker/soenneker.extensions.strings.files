using Soenneker.Extensions.DateTime;
using Soenneker.Utils.PooledStringBuilders;
using Soenneker.Utils.TimeZones;
using System;
using System.Diagnostics.Contracts;
using System.Globalization;
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
    /// <returns>Removes whitespace, appends datetime in file format.</returns>
    [Pure]
    public static string AppendDateTime(this string? value, TimeZoneInfo? timeZoneInfo = null, System.DateTime? utcNow = null)
    {
        utcNow ??= System.DateTime.UtcNow;
        timeZoneInfo ??= Tz.Eastern;

        System.DateTime converted = utcNow.Value.ToTz(timeZoneInfo);
        Span<char> timestamp = stackalloc char[19];
        converted.TryFormat(timestamp, out int written, "yyyy-MM-dd--HH-mm-ss", CultureInfo.InvariantCulture);
        return AppendCore(value, timestamp[..written]);
    }

    /// <summary>
    /// Removes whitespace and appends a DateTimeOffset in file-safe format
    /// </summary>
    /// <returns>Removes whitespace and appends a DateTimeOffset in file-safe format.</returns>
    [Pure]
    public static string AppendDateTimeOffset(this string? value, TimeZoneInfo? timeZoneInfo = null, DateTimeOffset? utcNow = null)
    {
        utcNow ??= DateTimeOffset.UtcNow;
        timeZoneInfo ??= Tz.Eastern;

        DateTimeOffset converted = TimeZoneInfo.ConvertTime(utcNow.Value, timeZoneInfo);
        Span<char> timestamp = stackalloc char[19];
        converted.TryFormat(timestamp, out int written, "yyyy-MM-dd--HH-mm-ss", CultureInfo.InvariantCulture);
        return AppendCore(value, timestamp[..written]);
    }

    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string AppendCore(string? value, ReadOnlySpan<char> timestamp)
    {
        if (value == null)
            return timestamp.ToString();

        var sb = new PooledStringBuilder(value.Length + timestamp.Length + 1);
        try
        {
            AppendSafeFileNameText(value, ref sb);
            if (sb.Length > 0)
                sb.Append('-');

            sb.Append(timestamp);
            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    /// <summary>
    /// Removes whitespace, appends datetime in file format, and appends the extension
    /// </summary>
    /// <returns>Removes whitespace, appends datetime in file format, and appends the extension.</returns>
    [Pure]
    public static string ToFileName(this string? value, string extension, TimeZoneInfo? timeZoneInfo = null)
    {
        timeZoneInfo ??= Tz.Eastern;
        System.DateTime converted = System.DateTime.UtcNow.ToTz(timeZoneInfo);
        Span<char> timestamp = stackalloc char[19];
        converted.TryFormat(timestamp, out int written, "yyyy-MM-dd--HH-mm-ss", CultureInfo.InvariantCulture);

        int capacity = (value?.Length ?? 0) + written + (extension?.Length ?? 0) + 39;
        var sb = new PooledStringBuilder(capacity);
        try
        {
            if (value is not null)
            {
                AppendSafeFileNameText(value, ref sb);
                if (sb.Length > 0)
                    sb.Append('-');
            }

            for (var i = 0; i < written; i++)
                sb.Append(timestamp[i]);

            sb.Append('-');
            sb.Append(Guid.NewGuid());

            ReadOnlySpan<char> extensionSpan = extension.AsSpan().Trim().TrimStart('.');
            int beforeExtension = sb.Length;
            sb.Append('.');
            AppendSafeFileNameText(extensionSpan, ref sb);

            if (sb.Length == beforeExtension + 1)
                sb.Shrink(1);

            return sb.ToString();
        }
        finally
        {
            sb.Dispose();
        }
    }

    private static void AppendSafeFileNameText(ReadOnlySpan<char> value, ref PooledStringBuilder builder)
    {
        var segmentStart = 0;

        for (var i = 0; i < value.Length; i++)
        {
            if (!char.IsWhiteSpace(value[i]) && !IsInvalidFileNameChar(value[i]))
                continue;

            if (i > segmentStart)
                builder.Append(value[segmentStart..i]);

            segmentStart = i + 1;
        }

        if (segmentStart < value.Length)
            builder.Append(value[segmentStart..]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsInvalidFileNameChar(char value)
    {
        return char.IsControl(value) || value is '<' or '>' or ':' or '"' or '/' or '\\' or '|' or '?' or '*';
    }
}
