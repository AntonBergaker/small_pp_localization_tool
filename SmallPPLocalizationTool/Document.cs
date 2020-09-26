using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;

namespace SmallPPLocalizationTool {
    class Document : IEnumerable<Language> {
        private readonly Language[] languages;

        private Document(Language[] languages) {
            this.languages = languages;
        }

        public static Document Parse(string csvData) {

            string[] languageIds;
            List<Language.Section>[] sections;

            using StringReader stringReader = new StringReader(csvData);

            using (var csv = new CsvReader(stringReader)) {
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

            return new Document(sections.Select((_, i) => new Language(languageIds[i], sections[i])).ToArray());
        }

        public IEnumerator<Language> GetEnumerator() {
            return (languages as IEnumerable<Language>).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
