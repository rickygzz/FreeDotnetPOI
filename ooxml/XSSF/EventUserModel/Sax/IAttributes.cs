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
namespace NPOI.XSSF.EventUserModel.Sax
{
    using System;

    /// <summary>
    /// The attributes of an element, passed to <see cref="IContentHandler.StartElement"/>.
    /// Namespace declarations (<c>xmlns</c> and <c>xmlns:*</c>) are not included.
    /// </summary>
    /// <remarks>
    /// The instance is only valid during the <see cref="IContentHandler.StartElement"/> call;
    /// the parser reuses it for the next element. Copy any values you need to keep.
    /// </remarks>
    public interface IAttributes
    {
        /// <summary>The number of attributes.</summary>
        int Length { get; }

        /// <summary>The namespace URI of the attribute at <paramref name="index"/>, or an empty string if it has none. Returns <c>null</c> if the index is out of range.</summary>
        String GetURI(int index);

        /// <summary>The local name (without prefix) of the attribute at <paramref name="index"/>, or <c>null</c> if the index is out of range.</summary>
        String GetLocalName(int index);

        /// <summary>The qualified name (with prefix, if any) of the attribute at <paramref name="index"/>, or <c>null</c> if the index is out of range.</summary>
        String GetQName(int index);

        /// <summary>The value of the attribute at <paramref name="index"/>, or <c>null</c> if the index is out of range.</summary>
        String GetValue(int index);

        /// <summary>The value of the attribute with the qualified name <paramref name="qName"/>, or <c>null</c> if there is none.</summary>
        String GetValue(String qName);

        /// <summary>The value of the attribute with the namespace URI <paramref name="uri"/> and local name <paramref name="localName"/>, or <c>null</c> if there is none.</summary>
        String GetValue(String uri, String localName);
    }
}
