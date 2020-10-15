using System.IO;
using System.Text;

namespace SmallPPLocalizationTool {
    class Exporter {
        private readonly Document document;
        private readonly bool useBase64;

        public Exporter(Document document, bool useBase64) {
            this.document = document;
            this.useBase64 = useBase64;
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
                using MemoryStream stream = new MemoryStream();

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

                string path = Path.Join(targetDirectory, language.ID + ".lang");
                if (useBase64) {
                    File.WriteAllText(path, System.Convert.ToBase64String(stream.ToArray()));
                }
                else {
                    File.WriteAllBytes(path, stream.ToArray());
                }
            }


            return filesMade;
        }
    }
}
