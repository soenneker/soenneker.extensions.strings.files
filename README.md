[![](https://img.shields.io/nuget/v/soenneker.extensions.strings.files.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.strings.files/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.strings.files/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.strings.files/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.strings.files.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.strings.files/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.strings.files/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.strings.files/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.Strings.Files
Build timestamped, collision-resistant filenames from human-readable labels.

## Installation

```bash
dotnet add package Soenneker.Extensions.Strings.Files
```

## Create a filename

```csharp
using Soenneker.Extensions.Strings.Files;

string fileName = "Quarterly Report".ToFileName("csv");
// QuarterlyReport-2026-08-30--14-22-05-<guid>.csv
```

`ToFileName()` removes whitespace and filename-invalid characters from the label and extension, adds a timestamp, and adds a GUID to prevent collisions. A leading dot on the extension is optional. Path separators in either input are removed, so the result is a filename rather than a caller-controlled path.

The timestamp uses the supplied time zone, or Eastern time when none is provided. The exact time and GUID are generated for each call.

## Append a timestamp

```csharp
string dated = "Daily Export".AppendDateTime();
string offsetDated = "Daily Export".AppendDateTimeOffset();
```

Both methods produce `DailyExport-yyyy-MM-dd--HH-mm-ss`. Pass `timeZoneInfo` to control the display zone, and pass `utcNow` when deterministic output is needed in tests.

These helpers sanitize individual filename components; they do not choose a directory, create a file, check reserved device names, or guarantee that the final path fits a filesystem's length limit. Combine the result with an application-controlled directory and apply any storage-specific policy before writing.
