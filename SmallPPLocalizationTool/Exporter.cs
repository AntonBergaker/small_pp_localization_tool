using System.IO;
using System.Text;
namespace SmallPPLocalizationTool {



    class Exporter {
        private readonly Document document;
        private readonly IBuilder builder;

        public Exporter(Document document, IBuilder builder) {
            this.document = document;
            this.builder = builder;
        }

        public int Export(string targetDirectory) {

            if (!Directory.Exists(targetDirectory)) {
                Directory.CreateDirectory(targetDirectory);
            }

            int filesMade = 0;

            foreach (Language language in document) {
                bool success = ExportLanguage(targetDirectory, language, builder);
                if (success) {
                    filesMade++;
                }
            }


            return filesMade;
        }

        private static bool ExportLanguage(string targetDirectory, Language language, IBuilder builder) {
            if (language.HasSection("meta") == false) {
                return false;
            }

            if (language["meta"].HasEntry("completed") == false) {
                return false;
            }

            if (language["meta"]["completed"].Value != "Yes") {
                return false;
            }

            string path = Path.Join(targetDirectory, language.ID + ".lang");

            using FileStream stream = new FileStream(path, FileMode.Create);

            builder.WriteToStream(language, stream);
            return true;
        }
    }
}
