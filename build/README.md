FreeDotnetPOI is a community-maintained fork of [NPOI](https://github.com/nissl-lab/npoi) 2.7.6, the last release published under the Apache License 2.0. NPOI 2.8.0 and later are released under a different license; this fork continues the Apache-2.0 2.7.x line, keeping dependencies current and fixing security vulnerabilities.

It reads and writes xls (Excel 97-2003), xlsx (Excel 2007+) and docx (Word 2007+) files. Supported frameworks: .NET 8+, .NET Standard 2.0/2.1 and .NET Framework 4.7.2+.

## Switching from NPOI 2.7.x

Assembly names, namespaces (`NPOI.*`) and the strong-name key are unchanged, so it is a drop-in replacement for the `NPOI` 2.7.x package.

- Replace the `NPOI` package reference with `FreeDotnetPOI`. Don't reference both packages in the same project.
- **If your project references `SixLabors.ImageSharp` directly, replace it with [FreeDotnetImageSharp](https://www.nuget.org/packages/FreeDotnetImageSharp).**
- **If your project references `SixLabors.Fonts` directly, replace it with [FreeDotnetFonts](https://www.nuget.org/packages/FreeDotnetFonts).**
- If your code drives `XSSFSheetXMLHandler` or `ReadOnlySharedStringsTable` with NSax types itself, switch to `NPOI.XSSF.EventUserModel.Sax` and `SaxParser.Parse(stream, handler)`, and catch `System.Xml.XmlException` instead of `SAXException`.

## Differences from NPOI 2.7.6

- Image handling uses [FreeDotnetImageSharp](https://www.nuget.org/packages/FreeDotnetImageSharp) 2.1.15 instead of `SixLabors.ImageSharp` 2.1.11. It is an Apache-2.0 fork of ImageSharp 2.1.13 that fixes GHSA-j9gm-c75j-xc9q, GHSA-jjfr-hcj7-qf5w, GHSA-j3p4-wp97-rph4, GHSA-gwg2-r3hj-4w44 and GHSA-wmxv-xphr-5c9g, and hardens ICC profile parsing. It provides the same `SixLabors.ImageSharp` assembly and namespaces.
- Text measurement uses [FreeDotnetFonts](https://www.nuget.org/packages/FreeDotnetFonts) 1.0.2 instead of `SixLabors.Fonts` 1.0.1 (same code under Apache-2.0, same `SixLabors.Fonts` assembly). FreeDotnetPOI does not depend on any Six Labors package.
- `System.Security.Cryptography.Xml` is updated to 8.0.5 and `System.Formats.Asn1` to 8.0.2 (security fixes).
- The streaming XSSF reader uses its own `XmlReader`-based SAX layer (`NPOI.XSSF.EventUserModel.Sax`) instead of the LGPL-3.0 `NSax` package. It never resolves external entities. All dependencies are under the MIT or Apache-2.0 license.
- Fixed data loss when opening an OOXML file that can't be opened as a zip file right away (for example, briefly locked by another process). The fallback path truncated the file to 0 bytes instead of reading it.
- Temp file names are GUID-based, so processes creating temp files at the same moment no longer collide.

Release notes: https://github.com/rickygzz/FreeDotnetPOI/releases

Source and issues: https://github.com/rickygzz/FreeDotnetPOI

Credit to Tony Qu and the NPOI contributors for the original work. This project is not affiliated with or endorsed by Nissl Lab or The Apache Software Foundation.
