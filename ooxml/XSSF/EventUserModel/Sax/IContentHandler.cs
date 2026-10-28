/* ====================================================================
   Copyright FreeDotnetPOI contributors

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
==================================================================== */
namespace NPOI.XSSF.EventUserModel.Sax;

using System;

/// <summary>
/// Receives the content of an XML document as a stream of events from <see cref="SaxParser"/>.
/// </summary>
public interface IContentHandler
{
    /// <summary>Called once, before any other event.</summary>
    void StartDocument();

    /// <summary>Called once, after all other events.</summary>
    void EndDocument();

    /// <summary>
    /// Called at the start of an element. Empty elements (<c>&lt;a/&gt;</c>) produce a
    /// <see cref="StartElement"/> call followed by an <see cref="EndElement"/> call.
    /// </summary>
    /// <param name="uri">The namespace URI, or an empty string if the element has none.</param>
    /// <param name="localName">The local name, without prefix.</param>
    /// <param name="qName">The qualified name, with prefix if any.</param>
    /// <param name="attributes">The element's attributes, valid only during this call.</param>
    void StartElement(String uri, String localName, String qName, IAttributes attributes);

    /// <summary>Called at the end of an element.</summary>
    /// <param name="uri">The namespace URI, or an empty string if the element has none.</param>
    /// <param name="localName">The local name, without prefix.</param>
    /// <param name="qName">The qualified name, with prefix if any.</param>
    void EndElement(String uri, String localName, String qName);

    /// <summary>
    /// Called for character data, including whitespace and CDATA sections. The text of one
    /// element may arrive in several calls. The array is reused after the call returns,
    /// so copy what you need.
    /// </summary>
    void Characters(char[] ch, int start, int length);
}
