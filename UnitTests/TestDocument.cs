using System.Globalization;
using System.Linq;
using NUnit.Framework;
using SmallPPLocalizationTool;

namespace UnitTests {
    public class Tests {

        private static readonly string ExampleCsvFile = string.Join("\r\n", new [] {
            ",Comments,en-US,sv-SE",
            "[meta],,,",
            "default,,Yes,No",
            "parent,,,",
            "[section],,,",
            "hello,im a comment,Hello,Hej"
        });

        [Test]
        public void TestParse() {

            Document document = Document.Parse(ExampleCsvFile);
            
            Assert.AreEqual(2, document.LanguageCount);

            if (document.TryGetLanguage("en-US", out Language? englishLanguage) == false) {
                Assert.Fail("Could not extract english language");
                return;
            }

            Assert.AreEqual("Hello", englishLanguage["section"]["hello"].Value);

            if (document.TryGetLanguage("sv-SE", out Language? swedishLanguage) == false) {
                Assert.Fail("Could not extract swedish language");
                return;
            }

            Assert.AreEqual("Hej", swedishLanguage["section"]["hello"].Value);
        }

        [Test]
        public void TestCultureParse() {
            // Russian uses ; as the csv seperator for whatever reason, make sure the program still works with a russian culture
            var preCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru");

            Document document = Document.Parse(ExampleCsvFile);
            
            Assert.AreEqual(2, document.LanguageCount);

            CultureInfo.CurrentCulture = preCulture;
        }
    }
}