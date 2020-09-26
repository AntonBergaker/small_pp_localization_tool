using System.IO;

namespace SmallPPLocalizationTool {
    class Exporter {
        private readonly Document document;

        public Exporter(Document document) {
            this.document = document;
        }

        public int Export(string targetDirectory) {

            int filesMade = 0;

            if (!Directory.Exists(targetDirectory)) {
                Directory.CreateDirectory(targetDirectory);
            }

            foreach (Language language in document) {
                if (language.HasSection("meta") == false) {
                    continue;
                }

                if (language["meta"].HasEntry("completed") == false) {
                    continue;
                }
                if (language["meta"]["completed"].Value != "Yes") {
                    continue;
                }

                filesMade++;
                string path = Path.Join(targetDirectory, language.ID + ".lang");
                using FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                using BufferWriter writer = new BufferWriter(stream);
                
                stream.SetLength(0);

                Language.Section[] sections = language.GetSections();
                writer.Write(sections.Length);

                foreach (Language.Section section in sections) {
                    writer.Write(section.Name);

                    Language.Entry[] entries = section.GetEntries();
                    writer.Write(entries.Length);

                    foreach (Language.Entry entry in entries) {
                        writer.Write(entry.Key);
                        writer.Write(entry.Value);
                    }
                }
            }

            return filesMade;
        }
    }
}
