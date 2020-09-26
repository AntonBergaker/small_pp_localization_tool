using System.Collections.Generic;
using System.Linq;

namespace SmallPPLocalizationTool {
    class Language {
        public string ID { protected set; get; }
        public Section this[string key] => sections[key];

        private readonly Dictionary<string, Section> sections;

        public Language(string id, IEnumerable<Section> sections) {
            ID = id;
            this.sections = new Dictionary<string, Section>();

            foreach (Section section in sections) {
                if (this.sections.ContainsKey(section.Name)) {
                    throw new DuplicateSectionException(this.sections[section.Name], section);
                }
                this.sections.Add(section.Name, section);
            }
        }


        public bool HasSection(string sectionName) {
            return sections.ContainsKey(sectionName);
        }

        public Section[] GetSections() => sections.Values.ToArray();
        


        public class Entry {
            public string Key { get; }
            public string Value { get; }
            public int Line { get; }

            public Entry(string key, string value, int line) {
                Key = key;
                Value = value;
                Line = line;
            }
        }

        public class Section {
            public string Name {  get; }
            public int Line { get; }
            private readonly Dictionary<string, Entry> entries;

            public Entry this[string key] => entries[key];

            public Entry[] GetEntries() => entries.Values.ToArray();

            public Section(string name, int line, IEnumerable<Entry> entries) {
                Name = name;
                Line = line;
                this.entries = new Dictionary<string, Entry>();

                foreach (Entry entry in entries) {
                    if (this.entries.ContainsKey(entry.Key)) {
                        throw new DuplicateEntryException(this.entries[entry.Key], entry);
                    }
                    this.entries.Add(entry.Key, entry);
                }
            }

            public bool HasEntry(string key) {
                return entries.ContainsKey(key);
            }
        }

    }
}
