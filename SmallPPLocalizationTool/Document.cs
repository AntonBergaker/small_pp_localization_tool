using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;

namespace SmallPPLocalizationTool {
    public class Document : IEnumerable<Language> {
        private readonly Dictionary<string, Language> languages;

        private Document(IEnumerable<Language> languages) {
            this.languages = languages.ToDictionary(x => x.ID);
        }

        public bool TryGetLanguage(string languageId, [NotNullWhen(true)] out Language? language) {
            return languages.TryGetValue(languageId, out language);
        }

        public int LanguageCount => languages.Count;

        public static Document Parse(string csvData) {

            string[] languageIds;
            List<Language.Section>[] sections;

            using StringReader stringReader = new StringReader(csvData);

            using (var csv = new CsvReader(stringReader, CultureInfo.InvariantCulture)) {
                csv.Read();

                int columnCount = 0;
                while (csv.TryGetField(columnCount, out string id)) {
                    columnCount++;
                }

                int languageCount = columnCount - 2;

                languageIds = new string[languageCount];


                sections = new List<Language.Section>[languageCount];
                List<Language.Entry>[] entries = new List<Language.Entry>[languageCount];
                for (int i = 0; i < languageCount; i++) {
                    languageIds[i] = csv.GetField(i + 2);
                    sections[i] = new List<Language.Section>();
                    entries[i] = new List<Language.Entry>();
                }

                string sectionName = "";
                int sectionLine = 0;

                int line = 1;
                while (csv.Read()) {
                    line++;
                    string tag = csv.GetField(0).Trim();
                    if (tag.Length == 0) {
                        continue;
                    }

                    if (tag.Length > 0 && tag[0] == '[') {


                        for (int i = 0; i < languageCount; i++) {
                            if (entries[i].Count != 0) {
                                Language.Section section = new Language.Section(sectionName, sectionLine, entries[i]);
                                sections[i].Add(section);
                                entries[i].Clear();
                            }
                        }

                        sectionLine = line;
                        sectionName = tag.Replace("[", "").Replace("]", "");
                        continue;
                    }

                    for (int i = 2; i < columnCount; i++) {
                        string field = csv.GetField(i);
                        if (field.Length != 0) {
                            entries[i - 2].Add(new Language.Entry(tag, field, line));
                        }
                    }

                }

                // Add the trailing list
                for (int i = 0; i < languageCount; i++) {
                    if (entries[i].Count != 0) {
                        Language.Section section = new Language.Section(sectionName, sectionLine, entries[i]);
                        sections[i].Add(section);
                    }
                }


            }
            
            return new Document(sections.Select((_, i) => new Language(languageIds[i], sections[i])));
        }

        public IEnumerator<Language> GetEnumerator() {
            return languages.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
