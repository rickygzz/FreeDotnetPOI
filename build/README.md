FreeDotnetPOI is a community-maintained fork of [NPOI](https://github.com/nissl-lab/npoi) 2.7.6, the last release published under the Apache License 2.0.

It reads and writes xls (Excel 97-2003), xlsx (Excel 2007+) and docx (Word 2007+) files. Assembly names and namespaces (`NPOI.*`) are unchanged, so it is a drop-in replacement for the `NPOI` 2.7.x package. Do not reference both packages in the same project.

## Differences from NPOI 2.7.6

- Image handling uses [FreeDotnetImageSharp](https://www.nuget.org/packages/FreeDotnetImageSharp) 2.1.14 instead of `SixLabors.ImageSharp` 2.1.11. It is an Apache-2.0 fork of ImageSharp 2.1.13 with fixes for GHSA-j9gm-c75j-xc9q, GHSA-jjfr-hcj7-qf5w, GHSA-j3p4-wp97-rph4, GHSA-gwg2-r3hj-4w44 and GHSA-wmxv-xphr-5c9g. It provides the same `SixLabors.ImageSharp` assembly and namespaces. **If your project references `SixLabors.ImageSharp` directly, replace it with `FreeDotnetImageSharp`.**
- `System.Security.Cryptography.Xml` is updated to 8.0.4 (security fixes).

Source and issues: https://github.com/rickygzz/FreeDotnetPOI

Credit to Tony Qu and the NPOI contributors for the original work. This project is not affiliated with or endorsed by Nissl Lab or The Apache Software Foundation.
