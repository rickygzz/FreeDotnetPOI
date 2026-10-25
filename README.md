FreeDotnetPOI
===================
[![NuGet](https://img.shields.io/nuget/v/FreeDotnetPOI.svg)](https://www.nuget.org/packages/FreeDotnetPOI)
[![CI](https://github.com/rickygzz/FreeDotnetPOI/actions/workflows/CI.yml/badge.svg?branch=main)](https://github.com/rickygzz/FreeDotnetPOI/actions/workflows/CI.yml)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg?style=flat-square&logo=Apache)](LICENSE)

**FreeDotnetPOI is a community-maintained fork of [NPOI](https://github.com/nissl-lab/npoi) at version 2.7.6 (commit `a0f50a01a`), the last release published under the Apache License 2.0.** NPOI 2.8.0 and later are released under a different license; this fork continues the Apache-2.0 2.7.x line, keeping dependencies current and fixing security vulnerabilities.

Like NPOI, it is the .NET version of the Apache POI project: it reads and writes xls (Excel 97-2003), xlsx (Excel 2007+) and docx (Word 2007+) files.

## Install

```
dotnet add package FreeDotnetPOI
```

### Switching from NPOI 2.7.x

FreeDotnetPOI is a drop-in replacement for the `NPOI` 2.7.x package. Assembly names, namespaces (`NPOI.*`) and the strong-name key are unchanged, so no code changes are needed.

1. Replace the `NPOI` package reference with `FreeDotnetPOI`. Don't reference both packages in the same project.
2. If your project references `SixLabors.ImageSharp` directly, replace it with [FreeDotnetImageSharp](https://github.com/rickygzz/FreeDotnetImageSharp). It provides the same `SixLabors.ImageSharp` assembly.
3. If your project references `SixLabors.Fonts` directly, replace it with [FreeDotnetFonts](https://github.com/rickygzz/FreeDotnetFonts). It provides the same `SixLabors.Fonts` assembly.
4. If your code drives `XSSFSheetXMLHandler` or `ReadOnlySharedStringsTable` with NSax types itself, replace `using NSAX;` and `using NSAX.Helpers;` with `using NPOI.XSSF.EventUserModel.Sax;`, replace the `SAXDriver` setup with `SaxParser.Parse(stream, handler)`, and catch `System.Xml.XmlException` instead of `SAXException`. Code that uses `XSSFReader`, the extractors or `XSSFWorkbook` needs no change.

## Supported frameworks

.NET 8 and later, .NET Standard 2.1, .NET Standard 2.0 and .NET Framework 4.7.2 and later (package targets `net8.0`, `netstandard2.1`, `netstandard2.0`, `net472`).

## Differences from NPOI 2.7.6

- Image handling uses [FreeDotnetImageSharp](https://github.com/rickygzz/FreeDotnetImageSharp) 2.1.15 instead of `SixLabors.ImageSharp` 2.1.11. It is an Apache-2.0 fork of ImageSharp 2.1.13 that fixes GHSA-j9gm-c75j-xc9q, GHSA-jjfr-hcj7-qf5w, GHSA-j3p4-wp97-rph4, GHSA-gwg2-r3hj-4w44 and GHSA-wmxv-xphr-5c9g, and hardens ICC profile parsing.
- Text measurement uses [FreeDotnetFonts](https://github.com/rickygzz/FreeDotnetFonts) 1.0.2 instead of `SixLabors.Fonts` 1.0.1 (the same code under Apache-2.0). FreeDotnetPOI does not depend on any Six Labors package.
- `System.Security.Cryptography.Xml` is updated to 8.0.5 and `System.Formats.Asn1` to 8.0.2 (security fixes).
- The streaming XSSF reader (`XSSFReader`, `XSSFSheetXMLHandler`, `ReadOnlySharedStringsTable`, `XSSFEventBasedExcelExtractor`) uses its own `XmlReader`-based SAX layer in `NPOI.XSSF.EventUserModel.Sax` instead of the LGPL-3.0 `NSax` package. It never resolves external entities or expands DTD entities. All FreeDotnetPOI dependencies are under the MIT or Apache-2.0 license.
- Fixed data loss when opening an OOXML file (`.xlsx`, `.docx`) that can't be opened as a zip file right away (for example, briefly locked by another process): the fallback path truncated the file to 0 bytes instead of reading it.
- `TempFile.GetTempFilePath` uses GUID-based names, so processes creating temp files at the same moment no longer get the same path.

## Release history

| Version | Change |
|---|---|
| 2.7.11 | Security: `System.Security.Cryptography.Xml` 8.0.5 (CVE-2026-96222, CVE-2026-96958, CVE-2026-96959, CVE-2026-96960). Removed the LGPL-3.0 `NSax` dependency; code that uses NSax types with the SAX handlers directly needs a small change (see [Switching from NPOI 2.7.x](#switching-from-npoi-27x)). |
| 2.7.10 | FreeDotnetImageSharp 2.1.15: hardens ICC profile parsing against untrusted counts (gigabyte-sized allocations from a small malformed profile when reading `IccProfile.Entries`). NPOI itself does not read ICC profiles. |
| 2.7.9 | FreeDotnetFonts 1.0.2 replaces `SixLabors.Fonts` 1.0.1. |
| 2.7.8 | OOXML open data-loss fix; GUID-based temp file names. |
| 2.7.7 | First release: FreeDotnetImageSharp 2.1.14 replaces `SixLabors.ImageSharp` 2.1.11; `System.Security.Cryptography.Xml` 8.0.4. |

Details for each version are in the [GitHub releases](https://github.com/rickygzz/FreeDotnetPOI/releases). Report bugs and security issues in [GitHub issues](https://github.com/rickygzz/FreeDotnetPOI/issues).

This project is not affiliated with or endorsed by Nissl Lab or The Apache Software Foundation. All credit for the original work goes to Tony Qu and the NPOI contributors. See [NOTICE](NOTICE).

The original NPOI README follows. Its system requirements describe upstream NPOI; for this package see [Supported frameworks](#supported-frameworks).

---

Documentation
============

[NPOI Tutorials on Youtube](https://www.youtube.com/watch?v=H4FzMWkZNH8&list=PL7J6yRMWV1ot32hsgCZJ5sI4Fp2QKovCM&index=9)

[Getting Started with NPOI](https://github.com/nissl-lab/npoi/wiki/Getting-Started-with-NPOI)

[How NPOI library succeeds](https://tonyqus.medium.com/how-npoi-succeeds-67ceb7333eb)

[How to use NPOI on Linux](https://github.com/nissl-lab/npoi/wiki/How-to-use-NPOI-on-Linux)

[Useful Packages based on NPOI](https://github.com/nissl-lab/npoi/wiki/ORM-on-NPOI)

[NPOI code examples (This example repo is private. You can get access by sponsoring me)](https://github.com/sponsors/tonyqus)

Advantage of NPOI
=================
a. It's totally free to use

b. Cover most features of Excel (cell style, data format, formula and so on)

c. Supported formats: xls, xlsx, docx.

d. Designed to be interface-oriented (take a look at NPOI.SS namespace)

e. Support not only export but also import

f. Real successful cases all over the world

g. Works on both Windows and Linux 

System Requirement
===================
.NET 8

.NET Standard 2.1 (.NET Core 3.x, .NET 5, .NET 6)

.NET Standard 2.0 (.NET Core 2.x)

.NET Framework 4.0 and above

Contributors
===================
![Contributors](https://contrib.rocks/image?repo=nissl-lab/npoi)
