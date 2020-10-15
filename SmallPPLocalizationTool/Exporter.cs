using System.IO;
using System.Text;
namespace SmallPPLocalizationTool {

    enum ExporterType {
        Buffer,
        BufferBase64,
        Json
    }

    class Exporter {
        private readonly Document document;
        private readonly ExporterType type;

        public Exporter(Document document, ExporterType type) {
            this.document = document;
            this.type = type;
        }

        public int Export(string targetDirectory) {

            int filesMade = 0;

            if (!Directory.Exists(targetDirectory)) {
                Directory.CreateDirectory(targetDirectory);
            }

            IBuilder builder = type switch {
                ExporterType.Json => new BuilderJson(),
                ExporterType.Buffer => new BuilderBuffer(false),
                ExporterType.BufferBase64 => new BuilderBuffer(true),
                _ => new BuilderBuffer(false),
            };

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

                using FileStream stream = new FileStream(path, FileMode.Create);

                builder.WriteToStream(language, stream);
            }


            return filesMade;
        }
    }
}
