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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace TestCases.XSSF.EventUserModel
{
    using NPOI.XSSF.EventUserModel.Sax;
    using NUnit.Framework;

    /// <summary>
    /// Tests for <see cref="SaxParser" />, which replaced the NSax dependency.
    /// </summary>
    [TestFixture]
    public sealed class TestSaxParser
    {
        private const String Main = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        private const String Rel = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

        private sealed class RecordingHandler : DefaultHandler
        {
            public readonly List<String> Events = new List<String>();
            private readonly StringBuilder text = new StringBuilder();

            public override void StartDocument() => Events.Add("startDocument");

            public override void EndDocument()
            {
                FlushText();
                Events.Add("endDocument");
            }

            public override void StartElement(String uri, String localName, String qName, IAttributes attributes)
            {
                FlushText();
                StringBuilder sb = new StringBuilder();
                sb.Append("start {").Append(uri).Append('}').Append(localName).Append(' ').Append(qName);
                for (int i = 0; i < attributes.Length; i++)
                {
                    sb.Append(" [{").Append(attributes.GetURI(i)).Append('}').Append(attributes.GetLocalName(i))
                        .Append(' ').Append(attributes.GetQName(i)).Append('=').Append(attributes.GetValue(i)).Append(']');
                }
                Events.Add(sb.ToString());
            }

            public override void EndElement(String uri, String localName, String qName)
            {
                FlushText();
                Events.Add("end {" + uri + "}" + localName + " " + qName);
            }

            public override void Characters(char[] ch, int start, int length) => text.Append(ch, start, length);

            private void FlushText()
            {
                if (text.Length > 0)
                {
                    Events.Add("text '" + text + "'");
                    text.Length = 0;
                }
            }
        }

        private static List<String> Parse(String xml)
        {
            RecordingHandler handler = new RecordingHandler();
            SaxParser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(xml)), handler);
            return handler.Events;
        }

        [Test]
        public void ReportsElementsAttributesAndText()
        {
            List<String> events = Parse(
                "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                "<workbook xmlns=\"" + Main + "\" xmlns:r=\"" + Rel + "\">" +
                "<sheet name=\"Sheet1\" r:id=\"rId1\"/>" +
                "<t>a &amp; b</t>" +
                "</workbook>");

            Assert.That(events, Is.EqualTo(new[]
            {
                "startDocument",
                "start {" + Main + "}workbook workbook",
                "start {" + Main + "}sheet sheet [{}name name=Sheet1] [{" + Rel + "}id r:id=rId1]",
                "end {" + Main + "}sheet sheet",
                "start {" + Main + "}t t",
                "text 'a & b'",
                "end {" + Main + "}t t",
                "end {" + Main + "}workbook workbook",
                "endDocument",
            }));
        }

        [Test]
        public void AttributeLookups()
        {
            IAttributes captured = null;
            String byQName = null, byUri = null, missing = null, outOfRange = null;
            CallbackHandler handler = new CallbackHandler((uri, localName, qName, attributes) =>
            {
                if (localName == "sheet")
                {
                    captured = attributes;
                    byQName = attributes.GetValue("r:id");
                    byUri = attributes.GetValue(Rel, "id");
                    missing = attributes.GetValue("nope");
                    outOfRange = attributes.GetValue(5);
                }
            });

            SaxParser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(
                "<workbook xmlns=\"" + Main + "\" xmlns:r=\"" + Rel + "\"><sheet name=\"S\" r:id=\"rId7\"/></workbook>")), handler);

            Assert.That(captured, Is.Not.Null);
            Assert.That(byQName, Is.EqualTo("rId7"));
            Assert.That(byUri, Is.EqualTo("rId7"));
            Assert.That(missing, Is.Null);
            Assert.That(outOfRange, Is.Null);
        }

        [Test]
        public void ReportsWhitespaceAndCData()
        {
            List<String> events = Parse("<sst><t xml:space=\"preserve\">  </t><t> x </t><t><![CDATA[<b>]]></t></sst>");

            Assert.That(events, Does.Contain("text '  '"));
            Assert.That(events, Does.Contain("text ' x '"));
            Assert.That(events, Does.Contain("text '<b>'"));
        }

        [Test]
        public void LongTextIsReportedCompletely()
        {
            String longText = new String('x', 100_000);
            List<String> events = Parse("<t>" + longText + "</t>");

            Assert.That(events, Does.Contain("text '" + longText + "'"));
        }

        [Test]
        public void DoesNotResolveExternalEntities()
        {
            String secretFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(secretFile, "SECRET");
                String xml =
                    "<?xml version=\"1.0\"?>" +
                    "<!DOCTYPE t [<!ENTITY xxe SYSTEM \"" + new Uri(secretFile).AbsoluteUri + "\">]>" +
                    "<t>&xxe;</t>";

                RecordingHandler handler = new RecordingHandler();
                Assert.Throws<XmlException>(() => SaxParser.Parse(new MemoryStream(Encoding.UTF8.GetBytes(xml)), handler));
                Assert.That(String.Join("\n", handler.Events), Does.Not.Contain("SECRET"));
            }
            finally
            {
                File.Delete(secretFile);
            }
        }

        [Test]
        public void DoesNotExpandInternalEntities()
        {
            String xml =
                "<?xml version=\"1.0\"?>" +
                "<!DOCTYPE t [<!ENTITY a \"aaaaaaaaaa\"><!ENTITY b \"&a;&a;&a;&a;&a;&a;&a;&a;&a;&a;\">]>" +
                "<t>&b;</t>";

            Assert.Throws<XmlException>(() => Parse(xml));
        }

        [Test]
        public void MalformedXmlThrowsXmlException()
        {
            Assert.Throws<XmlException>(() => Parse("<a><b></a>"));
        }

        [Test]
        public void LeavesStreamOpen()
        {
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes("<a/>"));
            SaxParser.Parse(stream, new DefaultHandler());

            Assert.That(stream.CanRead, Is.True);
        }

        [Test]
        public void RejectsNullArguments()
        {
            Assert.Throws<ArgumentNullException>(() => SaxParser.Parse(null, new DefaultHandler()));
            Assert.Throws<ArgumentNullException>(() => SaxParser.Parse(new MemoryStream(), null));
        }

        private sealed class CallbackHandler : DefaultHandler
        {
            private readonly Action<String, String, String, IAttributes> onStart;

            public CallbackHandler(Action<String, String, String, IAttributes> onStart)
            {
                this.onStart = onStart;
            }

            public override void StartElement(String uri, String localName, String qName, IAttributes attributes)
            {
                onStart(uri, localName, qName, attributes);
            }
        }
    }
}
