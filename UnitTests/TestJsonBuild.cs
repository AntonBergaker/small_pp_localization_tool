using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NUnit.Framework;
using SmallPPLocalizationTool;

namespace UnitTests {

    internal class TestJsonBuild {
        private static readonly string ExampleCsvFile = string.Join("\r\n", new[] {
            ",Comments,en-US,sv-SE",
            "[meta],,,",
            "default,,Yes,No",
            "parent,,,",
            "[section],,,",
            "hello,im a comment,Hello,Hej"
        });

        [Test]
        public void BuildTest() {

            Document document = Document.Parse(ExampleCsvFile);

            BuilderJson json = new BuilderJson();

            if (document.TryGetLanguage("en-US", out Language? english) == false) {
                Assert.Fail("Failed to export english");
                return;
            }

            using MemoryStream stream = new MemoryStream();
            
            json.WriteToStream(english, stream);

            using JsonDocument jsonDocument = JsonDocument.Parse(stream.ToArray());
            Assert.AreEqual("Hello", jsonDocument.RootElement.GetProperty("section").GetProperty("hello").GetString());
        }
    }
}
