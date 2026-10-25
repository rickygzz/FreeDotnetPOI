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
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Streams an XML document to an <see cref="IContentHandler"/>, SAX style, using
    /// <see cref="XmlReader"/>. The whole document is never held in memory.
    /// </summary>
    /// <remarks>
    /// DTDs are ignored and external resources are never resolved. Comments and processing
    /// instructions are skipped. Malformed XML throws <see cref="XmlException"/>.
    /// </remarks>
    public static class SaxParser
    {
        private const String XmlnsNamespace = "http://www.w3.org/2000/xmlns/";

        /// <summary>
        /// Parses <paramref name="input"/> and reports its content to <paramref name="handler"/>.
        /// The stream is not closed.
        /// </summary>
        /// <exception cref="XmlException">The XML is malformed.</exception>
        public static void Parse(Stream input, IContentHandler handler)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            XmlReaderSettings settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Ignore,
                XmlResolver = null,
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = false,
                CloseInput = false,
            };
            using (XmlReader reader = XmlReader.Create(input, settings))
            {
                Parse(reader, handler);
            }
        }

        private static void Parse(XmlReader reader, IContentHandler handler)
        {
            Attributes attributes = new Attributes();
            char[] buffer = new char[256];

            handler.StartDocument();
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        String uri = reader.NamespaceURI;
                        String localName = reader.LocalName;
                        String qName = reader.Name;
                        bool isEmpty = reader.IsEmptyElement;
                        attributes.Load(reader);
                        handler.StartElement(uri, localName, qName, attributes);
                        if (isEmpty)
                        {
                            handler.EndElement(uri, localName, qName);
                        }
                        break;
                    case XmlNodeType.EndElement:
                        handler.EndElement(reader.NamespaceURI, reader.LocalName, reader.Name);
                        break;
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.Whitespace:
                    case XmlNodeType.SignificantWhitespace:
                        String text = reader.Value;
                        if (text.Length > 0)
                        {
                            if (buffer.Length < text.Length)
                            {
                                buffer = new char[Math.Max(text.Length, buffer.Length * 2)];
                            }
                            text.CopyTo(0, buffer, 0, text.Length);
                            handler.Characters(buffer, 0, text.Length);
                        }
                        break;
                }
            }
            handler.EndDocument();
        }

        private sealed class Attributes : IAttributes
        {
            private readonly List<String> uris = new List<String>();
            private readonly List<String> localNames = new List<String>();
            private readonly List<String> qNames = new List<String>();
            private readonly List<String> values = new List<String>();

            public int Length => values.Count;

            // Reads the attributes of the element the reader is on, then moves back to the element.
            public void Load(XmlReader reader)
            {
                uris.Clear();
                localNames.Clear();
                qNames.Clear();
                values.Clear();

                if (reader.MoveToFirstAttribute())
                {
                    do
                    {
                        if (reader.NamespaceURI == XmlnsNamespace)
                        {
                            continue;
                        }
                        uris.Add(reader.NamespaceURI);
                        localNames.Add(reader.LocalName);
                        qNames.Add(reader.Name);
                        values.Add(reader.Value);
                    } while (reader.MoveToNextAttribute());
                    reader.MoveToElement();
                }
            }

            public String GetURI(int index) => InRange(index) ? uris[index] : null;

            public String GetLocalName(int index) => InRange(index) ? localNames[index] : null;

            public String GetQName(int index) => InRange(index) ? qNames[index] : null;

            public String GetValue(int index) => InRange(index) ? values[index] : null;

            public String GetValue(String qName)
            {
                int index = qNames.IndexOf(qName);
                return index >= 0 ? values[index] : null;
            }

            public String GetValue(String uri, String localName)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (localNames[i] == localName && uris[i] == (uri ?? String.Empty))
                    {
                        return values[i];
                    }
                }
                return null;
            }

            private bool InRange(int index) => index >= 0 && index < values.Count;
        }
    }
}
